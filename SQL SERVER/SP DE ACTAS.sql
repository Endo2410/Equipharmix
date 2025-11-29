USE Inventario_IT
GO

/****************** TIPO PARA DETALLE DE ACTA ******************/
/* Tabla tipo para pasar los detalles del acta al SP de registro */
CREATE TYPE [dbo].[PDetalle_Acta] AS TABLE(
	[IdEquipo] INT NULL,
	[Cantidad] INT NULL,
    NumeroSerial NVARCHAR(100),
    Caja NVARCHAR(100)
)
GO


/****************** PROCEDIMIENTO PARA OBTENER CORRELATIVO POR FARMACIA ******************/
/* Obtiene el siguiente número correlativo para el documento de acta basado en el código de farmacia */
CREATE PROCEDURE SP_OBTENER_CORRELATIVO_POR_FARMACIA
(
    @IdFarmacia INT,
    @Correlativo INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CodigoFarmacia VARCHAR(50);

    -- Obtener el código de la farmacia
    SELECT @CodigoFarmacia = RTRIM(LTRIM(Codigo))
    FROM FARMACIA
    WHERE IdFarmacia = @IdFarmacia;

    -- Si la farmacia no existe, retornar 1 como correlativo inicial
    IF @CodigoFarmacia IS NULL
    BEGIN
        SET @Correlativo = 1;
        RETURN;
    END

    -- Buscar el máximo número correlativo con base en los últimos 5 dígitos del campo NumeroDocumento
    SELECT @Correlativo = ISNULL(
        MAX(TRY_CAST(RIGHT(NumeroDocumento, 5) AS INT)),
        0
    )
    FROM ACTA
    WHERE NumeroDocumento LIKE @CodigoFarmacia + '-%';

    -- Incrementar correlativo para usar el siguiente disponible
    SET @Correlativo = @Correlativo + 1;
END
GO


/****************** PROCEDIMIENTO PARA REGISTRAR UNA ACTA ******************/
/* Inserta una nueva acta junto a sus detalles y valida existencia de farmacia y equipos */
CREATE PROCEDURE SP_REGISTRAR_ACTA
(
    @IdUsuario INT,
    @TipoDocumento VARCHAR(500),
    @NumeroDocumento VARCHAR(500),
    @Codigo VARCHAR(100), -- Código de la farmacia (ej. F001)
    @DetalleActa PDetalle_Acta READONLY,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        DECLARE @IdFarmacia INT;
        DECLARE @IdActa INT;

        SET @Resultado = 1;
        SET @Mensaje = '';

        -- Buscar IdFarmacia por el código
        SELECT @IdFarmacia = IdFarmacia
        FROM FARMACIA
        WHERE Codigo = @Codigo;

        -- Validar que exista la farmacia
        IF @IdFarmacia IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró una farmacia con ese código.';
            RETURN;
        END

        -- Validar existencia de equipos
        IF EXISTS (
            SELECT d.IdEquipo
            FROM @DetalleActa d
            LEFT JOIN EQUIPO e ON e.IdEquipo = d.IdEquipo
            WHERE e.IdEquipo IS NULL
        )
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Uno o más equipos no existen en la base de datos.';
            RETURN;
        END

        BEGIN TRANSACTION registro;

        -- Insertar acta
        INSERT INTO ACTA(IdUsuario, TipoDocumento, NumeroDocumento, IdFarmacia, EstadoAutorizacion)
        VALUES(@IdUsuario, @TipoDocumento, @NumeroDocumento, @IdFarmacia, 'PENDIENTE');

        SET @IdActa = SCOPE_IDENTITY();

        -- Insertar detalles
        INSERT INTO DETALLE_ACTA(IdActa, IdEquipo, Cantidad, NumeroSerial, Caja)
        SELECT @IdActa, IdEquipo, Cantidad, NumeroSerial, Caja FROM @DetalleActa;

        COMMIT TRANSACTION registro;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
        ROLLBACK TRANSACTION registro;
    END CATCH
END
GO


/****************** PROCEDIMIENTO PARA OBTENER ACTA POR NÚMERO DE DOCUMENTO ******************/
/* Consulta detalles e info general del acta dada su clave NumeroDocumento */
CREATE PROCEDURE SP_OBTENER_ACTA_POR_NUMERO_DOCUMENTO
(
    @NumeroDocumento VARCHAR(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si el documento existe
    IF NOT EXISTS (SELECT 1 FROM ACTA WHERE NumeroDocumento = @NumeroDocumento)
    BEGIN
        RAISERROR('No se encontró un acta con ese número de documento.', 16, 1);
        RETURN;
    END

    -- Información general del acta
    SELECT 
        A.IdActa,
        A.IdUsuario,
        U.NombreCompleto AS Usuario,
        A.TipoDocumento,
        A.NumeroDocumento,
        A.IdFarmacia,
        F.Codigo AS CodigoFarmacia,
        F.Nombre AS NombreFarmacia,
        A.FechaRegistro,
        A.EstadoAutorizacion,
        A.Estado
    FROM ACTA A
    INNER JOIN FARMACIA F ON A.IdFarmacia = F.IdFarmacia
    INNER JOIN USUARIO U ON A.IdUsuario = U.IdUsuario
    WHERE A.NumeroDocumento = @NumeroDocumento;

    -- Detalles del acta (incluyendo marca)
    SELECT
        DA.IdDetalleActa,
        DA.IdActa,
        DA.IdEquipo,
        E.Nombre AS NombreEquipo,
        E.IdMarca,
        M.Descripcion AS NombreMarca,
        DA.Cantidad,
        DA.NumeroSerial,
        DA.Caja,
        DA.MotivoBaja,
        DA.EstadoBaja
    FROM DETALLE_ACTA DA
    INNER JOIN EQUIPO E ON DA.IdEquipo = E.IdEquipo
    INNER JOIN MARCA M ON E.IdMarca = M.IdMarca
    INNER JOIN ACTA A ON DA.IdActa = A.IdActa
    WHERE A.NumeroDocumento = @NumeroDocumento;
END
GO


/****************** PROCEDIMIENTO PARA OBTENER EQUIPOS POR FARMACIA ******************/
/* Devuelve las actas y detalles autorizados o rechazados (sin pendientes) para una farmacia */
CREATE PROCEDURE SP_OBTENER_EQUIPOS_POR_FARMACIA
    @CodigoFarmacia VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        A.NumeroDocumento,
        A.FechaRegistro,
        E.Codigo AS CodigoEquipo,
        E.Nombre AS NombreEquipo,
		M.Descripcion AS MarcaEquipo,
        ES.Descripcion AS EstadoEquipo,
        DA.Cantidad,
        DA.NumeroSerial,
        DA.Caja,
        DA.MotivoBaja,
        DA.EstadoBaja,
        A.EstadoAutorizacion
    FROM ACTA A
    INNER JOIN FARMACIA F ON A.IdFarmacia = F.IdFarmacia
    INNER JOIN DETALLE_ACTA DA ON A.IdActa = DA.IdActa
    INNER JOIN EQUIPO E ON DA.IdEquipo = E.IdEquipo
	INNER JOIN MARCA M ON E.IdMarca = M.IdMarca 
    INNER JOIN ESTADO ES ON E.IdEstado = ES.IdEstado
    WHERE F.Codigo = @CodigoFarmacia
      AND ISNULL(DA.EstadoBaja, '') <> 'Autorizado'       -- Evita NULL y compara directamente
      AND ISNULL(A.EstadoAutorizacion, '') <> 'PENDIENTE' -- Solo actas ya revisadas
    ORDER BY A.FechaRegistro DESC
END
GO


/****************** PROCEDIMIENTO PARA CAMBIAR EL ESTADO DE UN EQUIPO ******************/
/* Actualiza estado y motivo de baja de un equipo en un acta */
CREATE PROCEDURE SP_CAMBIAR_ESTADO_EQUIPO
    @NumeroDocumento VARCHAR(50),
    @CodigoEquipo VARCHAR(50),
    @NumeroSerial VARCHAR(50),
    @NuevoEstado VARCHAR(50),
    @MotivoBaja VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS (
            SELECT 1
            FROM DETALLE_ACTA DA
            INNER JOIN ACTA A ON A.IdActa = DA.IdActa
            INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
            WHERE A.NumeroDocumento = @NumeroDocumento
              AND E.Codigo = @CodigoEquipo
              AND DA.NumeroSerial = @NumeroSerial
        )
        BEGIN
            UPDATE DA
            SET EstadoBaja = @NuevoEstado,
                MotivoBaja = @MotivoBaja
            FROM DETALLE_ACTA DA
            INNER JOIN ACTA A ON A.IdActa = DA.IdActa
            INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
            WHERE A.NumeroDocumento = @NumeroDocumento
              AND E.Codigo = @CodigoEquipo
              AND DA.NumeroSerial = @NumeroSerial;
        END
        ELSE
        BEGIN
            RAISERROR('No se encontró el equipo con esos datos.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        PRINT ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTO PARA AUTORIZAR UN EQUIPO ******************/
/* Cambia estado a "Autorizado" y registra usuario que autorizó */
CREATE PROCEDURE SP_AUTORIZAR_EQUIPO
    @NumeroDocumento VARCHAR(50),
    @CodigoEquipo VARCHAR(50),
    @NumeroSerial VARCHAR(50),
    @IdUsuarioAutorizo INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS (
            SELECT 1
            FROM DETALLE_ACTA DA
            INNER JOIN ACTA A ON A.IdActa = DA.IdActa
            INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
            WHERE A.NumeroDocumento = @NumeroDocumento
              AND E.Codigo = @CodigoEquipo
              AND DA.NumeroSerial = @NumeroSerial
        )
        BEGIN
            UPDATE DA
            SET EstadoBaja = 'Autorizado',
                IdUsuarioAutorizo = @IdUsuarioAutorizo
            FROM DETALLE_ACTA DA
            INNER JOIN ACTA A ON A.IdActa = DA.IdActa
            INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
            WHERE A.NumeroDocumento = @NumeroDocumento
              AND E.Codigo = @CodigoEquipo
              AND DA.NumeroSerial = @NumeroSerial;
        END
        ELSE
        BEGIN
            RAISERROR('No se encontró el equipo con los datos proporcionados.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        PRINT ERROR_MESSAGE();
    END CATCH
END
GO



/****************** PROCEDIMIENTO PARA OBTENER EQUIPOS PENDIENTES POR FARMACIA ******************/
/* Obtiene los equipos con estado pendiente para una farmacia (o todas si es NULL) */
CREATE PROCEDURE SP_OBTENER_EQUIPOS_PENDIENTES_POR_FARMACIA
    @CodigoFarmacia VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            A.NumeroDocumento,
            A.FechaRegistro,
            F.Nombre AS NombreFarmacia,
            E.Codigo AS CodigoEquipo,
            E.Nombre AS NombreEquipo,
			M.Descripcion AS MarcaEquipo,
            ES.Descripcion AS EstadoEquipo,
            DA.Cantidad,
            DA.NumeroSerial,
            DA.Caja,
            A.EstadoAutorizacion,
            A.IdUsuario,                                
            U.NombreCompleto AS CreadorActa
        FROM ACTA A
        INNER JOIN FARMACIA F ON A.IdFarmacia = F.IdFarmacia
        INNER JOIN DETALLE_ACTA DA ON A.IdActa = DA.IdActa
        INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
		INNER JOIN MARCA M ON E.IdMarca = M.IdMarca 
        INNER JOIN ESTADO ES ON ES.IdEstado = E.IdEstado
        INNER JOIN USUARIO U ON A.IdUsuario = U.IdUsuario
        WHERE LTRIM(RTRIM(UPPER(A.EstadoAutorizacion))) = 'PENDIENTE'
          AND (@CodigoFarmacia IS NULL OR F.Codigo = @CodigoFarmacia)
        ORDER BY A.FechaRegistro DESC;
    END TRY
    BEGIN CATCH
        PRINT 'Error al obtener los equipos pendientes por farmacia: ' + ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTO PARA OBTENER EQUIPOS EN ESPERA ******************/
/* Lista equipos marcados como "En espera" */
CREATE PROCEDURE SP_OBTENER_EQUIPOS_EN_ESPERA
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            A.NumeroDocumento,
            A.FechaRegistro,
            F.Nombre AS NombreFarmacia,
            E.Codigo AS CodigoEquipo,
            E.Nombre AS NombreEquipo,
		    M.Descripcion AS MarcaEquipo,
            DA.Cantidad,
            DA.NumeroSerial,
            DA.Caja,
            DA.MotivoBaja,
            DA.EstadoBaja,
            US.NombreCompleto AS UsuarioSolicitante
        FROM ACTA A
        INNER JOIN FARMACIA F ON A.IdFarmacia = F.IdFarmacia
        INNER JOIN DETALLE_ACTA DA ON A.IdActa = DA.IdActa
        INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
		INNER JOIN MARCA M ON E.IdMarca = M.IdMarca 
        LEFT JOIN USUARIO US ON DA.IdUsuarioSolicita = US.IdUsuario
        WHERE LTRIM(RTRIM(UPPER(DA.EstadoBaja))) = 'EN ESPERA'
        ORDER BY A.FechaRegistro DESC;
    END TRY
    BEGIN CATCH
        PRINT 'Error al obtener los equipos en espera: ' + ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTO PARA MARCAR EQUIPO EN ESPERA ******************/
/* Marca un equipo con estado "En espera" y registra motivo y usuario solicitante */
CREATE PROCEDURE SP_MARCAR_EQUIPO_EN_ESPERA
    @NumeroDocumento     VARCHAR(50),
    @CodigoEquipo        VARCHAR(50),
    @NumeroSerial        VARCHAR(50),
    @MotivoBaja          VARCHAR(MAX),
    @IdUsuarioSolicita   INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE DA
        SET 
            MotivoBaja = @MotivoBaja,
            EstadoBaja = 'En espera',
            IdUsuarioSolicita = @IdUsuarioSolicita
        FROM DETALLE_ACTA DA
        INNER JOIN ACTA A ON A.IdActa = DA.IdActa
        INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
        WHERE 
            A.NumeroDocumento = @NumeroDocumento
            AND E.Codigo = @CodigoEquipo
            AND DA.NumeroSerial = @NumeroSerial;
    END TRY
    BEGIN CATCH
        PRINT 'Error al marcar el equipo en espera: ' + ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTO PARA AUTORIZAR EQUIPO PENDIENTE ******************/
/* Autoriza un equipo pendiente, valida stock y actualiza estado y stock */
CREATE PROCEDURE SP_AUTORIZAR_EQUIPO_PENDIENTE
(
    @NumeroDocumento   VARCHAR(100),
    @CodigoEquipo      VARCHAR(100),
    @NumeroSerial      VARCHAR(100),
    @IdUsuario         INT,
    @Resultado         BIT OUTPUT,
    @Mensaje           VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @IdEquipo INT;
        DECLARE @IdDetalle INT;
        DECLARE @IdActa INT;

        SET @Resultado = 1;
        SET @Mensaje = '';

        -- Obtener IdEquipo, IdDetalleActa e IdActa
        SELECT TOP 1
            @IdEquipo = E.IdEquipo,
            @IdDetalle = DA.IdDetalleActa,
            @IdActa = DA.IdActa
        FROM DETALLE_ACTA DA
        INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
        INNER JOIN ACTA A ON A.IdActa = DA.IdActa
        WHERE A.NumeroDocumento = @NumeroDocumento
          AND E.Codigo = @CodigoEquipo
          AND DA.NumeroSerial = @NumeroSerial;

        IF @IdDetalle IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró el equipo en el acta.';
            RETURN;
        END

        -- Registrar usuario que autoriza en DETALLE_ACTA
        UPDATE DETALLE_ACTA
        SET IdUsuarioAutorizo = @IdUsuario,
            FechaRegistro = GETDATE()
        WHERE IdDetalleActa = @IdDetalle;

        -- Actualizar estado de autorización en ACTA
        UPDATE ACTA
        SET EstadoAutorizacion = 'AUTORIZADO'
        WHERE IdActa = @IdActa;

        SET @Mensaje = 'Equipo autorizado correctamente.';

    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO




/****************** PROCEDIMIENTO PARA OBTENER EQUIPOS AUTORIZADOS ******************/
/* Obtiene los equipos con estado 'Autorizado' */
CREATE PROCEDURE SP_OBTENER_EQUIPOS_AUTORIZADOS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        A.NumeroDocumento,
        A.FechaRegistro,
        F.Nombre AS NombreFarmacia,
        E.Codigo AS CodigoEquipo,
        E.Nombre AS NombreEquipo,
        M.Descripcion AS MarcaEquipo,
        DA.Cantidad,
        DA.NumeroSerial,
        DA.Caja,
        DA.MotivoBaja,
        DA.EstadoBaja,

        -- SOLICITANTE
        SOL.NombreCompleto AS UsuarioSolicitante,

        -- AUTORIZADOR
        AUT.NombreCompleto AS UsuarioAutorizador
    FROM DETALLE_ACTA DA
    INNER JOIN ACTA A ON DA.IdActa = A.IdActa
    INNER JOIN FARMACIA F ON A.IdFarmacia = F.IdFarmacia
    INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
    INNER JOIN MARCA M ON E.IdMarca = M.IdMarca

    INNER JOIN USUARIO SOL ON DA.IdUsuarioSolicita = SOL.IdUsuario

    LEFT JOIN USUARIO AUT ON DA.IdUsuarioAutorizo = AUT.IdUsuario
    WHERE DA.EstadoBaja = 'Autorizado';
END

/****************** PROCEDIMIENTO PARA ELIMINAR EQUIPO AUTORIZADO ******************/
/* Elimina un equipo autorizado y elimina acta si queda vacía */

CREATE PROCEDURE SP_ELIMINAR_EQUIPO_AUTORIZADO
    @NumeroDocumento VARCHAR(50),
    @CodigoEquipo VARCHAR(50),
    @NumeroSerial VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdActa INT;

    -- Obtener el IdActa según el documento
    SELECT @IdActa = A.IdActa
    FROM ACTA A
    WHERE A.NumeroDocumento = @NumeroDocumento;

    -- Eliminar el detalle específico
    DELETE DA
    FROM DETALLE_ACTA DA
    INNER JOIN EQUIPO E ON DA.IdEquipo = E.IdEquipo
    WHERE DA.IdActa = @IdActa
      AND E.Codigo = @CodigoEquipo
      AND DA.NumeroSerial = @NumeroSerial
      AND DA.EstadoBaja = 'Autorizado';

    -- Verificar si quedan más detalles asociados a ese acta
    IF NOT EXISTS (SELECT 1 FROM DETALLE_ACTA WHERE IdActa = @IdActa)
    BEGIN
        DELETE FROM ACTA WHERE IdActa = @IdActa;
    END
END



/****************** PROCEDIMIENTO PARA LIMPIAR ESTADO Y MOTIVO ******************/
/* Limpia los campos EstadoBaja y MotivoBaja para un detalle específico */
CREATE PROCEDURE SP_LIMPIAR_ESTADO_Y_MOTIVO
(
    @NumeroDocumento VARCHAR(50),
    @CodigoEquipo VARCHAR(50),
    @NumeroSerial VARCHAR(50),
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;
        SET @Resultado = 1;
        SET @Mensaje = '';

        DECLARE @Existe INT;

        -- Verificar si existe el detalle
        SELECT @Existe = COUNT(*)
        FROM DETALLE_ACTA DA
        INNER JOIN ACTA A ON A.IdActa = DA.IdActa
        INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
        WHERE A.NumeroDocumento = @NumeroDocumento
          AND E.Codigo = @CodigoEquipo
          AND DA.NumeroSerial = @NumeroSerial;

        IF @Existe = 0
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró el detalle del equipo especificado.';
            RETURN;
        END

        -- Limpiar estado y motivo
        UPDATE DA
        SET MotivoBaja = '',
            EstadoBaja = ''
        FROM DETALLE_ACTA DA
        INNER JOIN ACTA A ON A.IdActa = DA.IdActa
        INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
        WHERE A.NumeroDocumento = @NumeroDocumento
          AND E.Codigo = @CodigoEquipo
          AND DA.NumeroSerial = @NumeroSerial;

    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO
