CREATE TYPE [dbo].[PDetalle_Prestamo] AS TABLE(
    IdEquipo INT NOT NULL,
    Cantidad INT NOT NULL,
    NumeroSerial NVARCHAR(100),
    EstadoDevolucion NVARCHAR(50)
);
GO


CREATE PROCEDURE SP_REGISTRAR_PRESTAMO
(
    @IdUsuarioSolicita INT,
    @IdFarmacia INT,
    @TipoDocumento VARCHAR(50),
    @NumeroDocumento VARCHAR(50),
    @DetallePrestamo PDetalle_Prestamo READONLY,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @IdPrestamo INT;
        SET @Resultado = 1;
        SET @Mensaje = '';

        BEGIN TRANSACTION;

        INSERT INTO PRESTAMO(IdUsuarioSolicita, IdFarmacia, TipoDocumento, NumeroDocumento, EstadoPrestamo)
        VALUES(@IdUsuarioSolicita, @IdFarmacia, @TipoDocumento, @NumeroDocumento, 'PENDIENTE');

        SET @IdPrestamo = SCOPE_IDENTITY();

        INSERT INTO DETALLE_PRESTAMO(IdPrestamo, IdEquipo, Cantidad, NumeroSerial, EstadoDevolucion)
        SELECT @IdPrestamo, IdEquipo, Cantidad, NumeroSerial, ISNULL(EstadoDevolucion, 'PENDIENTE')
        FROM @DetallePrestamo;

        COMMIT TRANSACTION;
        SET @Mensaje = 'Préstamo registrado correctamente.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO



/****************** PROCEDIMIENTO PARA OBTENER CORRELATIVO DE PRÉSTAMO POR FARMACIA ******************/
CREATE PROCEDURE SP_OBTENER_CORRELATIVO_PRESTAMO_POR_FARMACIA
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
    END;

    -- Buscar el máximo número correlativo con base en los últimos 5 dígitos del campo NumeroDocumento
    SELECT @Correlativo = ISNULL(
        MAX(TRY_CAST(RIGHT(NumeroDocumento, 5) AS INT)),
        0
    )
    FROM PRESTAMO
    WHERE NumeroDocumento LIKE @CodigoFarmacia + '-%';

    -- Incrementar correlativo para usar el siguiente disponible
    SET @Correlativo = @Correlativo + 1;
END;
GO

/****************** PROCEDIMIENTO PARA OBTENER PRÉSTAMO POR NÚMERO DE DOCUMENTO ******************/
/* Consulta detalles e info general del préstamo dada su clave NumeroDocumento */
CREATE PROCEDURE SP_OBTENER_PRESTAMO_POR_NUMERO_DOCUMENTO
(
    @NumeroDocumento VARCHAR(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si el documento existe
    IF NOT EXISTS (SELECT 1 FROM PRESTAMO WHERE NumeroDocumento = @NumeroDocumento)
    BEGIN
        RAISERROR('No se encontró un préstamo con ese número de documento.', 16, 1);
        RETURN;
    END

    -- Información general del préstamo
    SELECT 
        P.IdPrestamo,
        P.IdUsuarioSolicita,
        U.NombreCompleto AS UsuarioSolicita,
        P.TipoDocumento,
        P.NumeroDocumento,
        P.IdFarmacia,
        F.Codigo AS CodigoFarmacia,
        F.Nombre AS NombreFarmacia,
        P.FechaPrestamo,
        P.FechaDevolucion,
        P.EstadoPrestamo,
        ISNULL(UA.NombreCompleto, '-') AS UsuarioAutoriza
    FROM PRESTAMO P
    INNER JOIN FARMACIA F ON P.IdFarmacia = F.IdFarmacia
    INNER JOIN USUARIO U ON P.IdUsuarioSolicita = U.IdUsuario
    LEFT JOIN USUARIO UA ON P.IdUsuarioAutoriza = UA.IdUsuario
    WHERE P.NumeroDocumento = @NumeroDocumento;

    -- Detalles del préstamo (incluyendo marca)
    SELECT
        DP.IdDetallePrestamo,
        DP.IdPrestamo,
        DP.IdEquipo,
        E.Nombre AS NombreEquipo,
        E.IdMarca,
        M.Descripcion AS NombreMarca,
        DP.Cantidad,
        DP.NumeroSerial,
        DP.EstadoDevolucion
    FROM DETALLE_PRESTAMO DP
    INNER JOIN EQUIPO E ON DP.IdEquipo = E.IdEquipo
    INNER JOIN MARCA M ON E.IdMarca = M.IdMarca
    INNER JOIN PRESTAMO P ON DP.IdPrestamo = P.IdPrestamo
    WHERE P.NumeroDocumento = @NumeroDocumento;
END
GO




CREATE PROCEDURE SP_OBTENER_PRESTAMOS_PENDIENTES_POR_FARMACIA
    @CodigoFarmacia VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT
            P.NumeroDocumento,
            P.FechaPrestamo AS FechaRegistro,
            F.Nombre AS NombreFarmacia,
            E.Codigo AS CodigoEquipo,
            E.Nombre AS NombreEquipo,
            M.Descripcion AS MarcaEquipo,
            ES.Descripcion AS EstadoEquipo,
            DP.Cantidad,
            DP.NumeroSerial,
            P.EstadoPrestamo AS EstadoAutorizacion,
            P.IdUsuarioSolicita AS IdUsuario,
            U.NombreCompleto AS CreadorActa
        FROM PRESTAMO P
        INNER JOIN DETALLE_PRESTAMO DP ON P.IdPrestamo = DP.IdPrestamo
        INNER JOIN EQUIPO E ON E.IdEquipo = DP.IdEquipo
        INNER JOIN MARCA M ON E.IdMarca = M.IdMarca
        INNER JOIN ESTADO ES ON ES.IdEstado = E.IdEstado
        LEFT JOIN FARMACIA F ON P.IdFarmacia = F.IdFarmacia
        INNER JOIN USUARIO U ON P.IdUsuarioSolicita = U.IdUsuario
        WHERE LTRIM(RTRIM(UPPER(P.EstadoPrestamo))) = 'PENDIENTE'
          AND (@CodigoFarmacia IS NULL OR F.Codigo = @CodigoFarmacia)
        ORDER BY P.FechaPrestamo DESC;
    END TRY
    BEGIN CATCH
        PRINT 'Error al obtener préstamos pendientes por farmacia: ' + ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTO PARA ELIMINAR EQUIPO DE PRESTAMO ******************/
/* Elimina un equipo específico de un préstamo y elimina el préstamo si queda vacío */
CREATE PROCEDURE SP_ELIMINAR_EQUIPO_DE_PRESTAMO
(
    @NumeroDocumento   VARCHAR(100),
    @CodigoEquipo      VARCHAR(100),
    @NumeroSerial      VARCHAR(100),
    @Resultado         BIT OUTPUT,
    @Mensaje           VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @IdPrestamo INT;

        SET @Resultado = 1;
        SET @Mensaje = '';

        -- Obtener IdPrestamo relacionado
        SELECT TOP 1 @IdPrestamo = P.IdPrestamo
        FROM PRESTAMO P
        INNER JOIN DETALLE_PRESTAMO DP ON P.IdPrestamo = DP.IdPrestamo
        INNER JOIN EQUIPO E ON E.IdEquipo = DP.IdEquipo
        WHERE 
            P.NumeroDocumento = @NumeroDocumento AND
            E.Codigo = @CodigoEquipo AND
            DP.NumeroSerial = @NumeroSerial;

        -- Validación de existencia
        IF @IdPrestamo IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró el registro del equipo en el préstamo.';
            RETURN;
        END

        -- Eliminar el detalle del préstamo
        DELETE DP
        FROM DETALLE_PRESTAMO DP
        INNER JOIN EQUIPO E ON E.IdEquipo = DP.IdEquipo
        WHERE 
            DP.IdPrestamo = @IdPrestamo AND
            E.Codigo = @CodigoEquipo AND
            DP.NumeroSerial = @NumeroSerial;

        -- Si ya no quedan equipos en el préstamo ? eliminar préstamo completo
        IF NOT EXISTS (SELECT 1 FROM DETALLE_PRESTAMO WHERE IdPrestamo = @IdPrestamo)
        BEGIN
            DELETE FROM PRESTAMO WHERE IdPrestamo = @IdPrestamo;
        END

        SET @Mensaje = 'Equipo eliminado del préstamo correctamente.';

    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE SP_AUTORIZAR_PRESTAMO_PENDIENTE
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
        DECLARE @StockActual INT;

        SET @Resultado = 1;
        SET @Mensaje = '';

        -- Obtener IdEquipo
        SELECT TOP 1 @IdEquipo = E.IdEquipo
        FROM PRESTAMO P
        INNER JOIN DETALLE_PRESTAMO DP ON DP.IdPrestamo = P.IdPrestamo
        INNER JOIN EQUIPO E ON E.IdEquipo = DP.IdEquipo
        WHERE 
            P.NumeroDocumento = @NumeroDocumento AND
            E.Codigo = @CodigoEquipo AND
            DP.NumeroSerial = @NumeroSerial;

        IF @IdEquipo IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró el equipo en el préstamo.';
            RETURN;
        END

        -- Validar stock
        SELECT @StockActual = Stock FROM EQUIPO WHERE IdEquipo = @IdEquipo;

        IF @StockActual <= 0
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No hay stock disponible para este equipo.';
            RETURN;
        END

        -- Autorizar préstamo y registrar usuario que autoriza
        UPDATE P
        SET EstadoPrestamo = 'AUTORIZADO',
            IdUsuarioAutoriza = @IdUsuario
        FROM PRESTAMO P
        INNER JOIN DETALLE_PRESTAMO DP ON DP.IdPrestamo = P.IdPrestamo
        WHERE 
            P.NumeroDocumento = @NumeroDocumento AND
            DP.IdEquipo = @IdEquipo AND
            DP.NumeroSerial = @NumeroSerial;

        -- Descontar stock
        UPDATE EQUIPO
        SET Stock = Stock - 1
        WHERE IdEquipo = @IdEquipo;

        SET @Mensaje = 'Préstamo autorizado correctamente.';

    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO



CREATE PROCEDURE SP_LISTAR_PRESTAMOS_AUTORIZADOS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.NumeroDocumento,
        P.FechaPrestamo AS FechaRegistro,
        F.Nombre AS NombreFarmacia,
        E.Codigo AS CodigoEquipo,
        E.Nombre AS NombreEquipo,
        M.Descripcion AS MarcaEquipo,
        DP.Cantidad,
        DP.NumeroSerial,
        U.NombreCompleto AS CreadorActa
    FROM PRESTAMO P
    INNER JOIN DETALLE_PRESTAMO DP ON P.IdPrestamo = DP.IdPrestamo
    INNER JOIN EQUIPO E ON E.IdEquipo = DP.IdEquipo
    INNER JOIN MARCA M ON E.IdMarca = M.IdMarca
    INNER JOIN ESTADO ES ON ES.IdEstado = E.IdEstado
    LEFT JOIN FARMACIA F ON P.IdFarmacia = F.IdFarmacia
    INNER JOIN USUARIO U ON P.IdUsuarioSolicita = U.IdUsuario
    WHERE P.EstadoPrestamo = 'AUTORIZADO'  -- Solo préstamos autorizados
    ORDER BY P.FechaPrestamo DESC;
END
GO

CREATE PROCEDURE SP_DEVOLVER_EQUIPO_PRESTAMO
(
    @NumeroDocumento VARCHAR(100),
    @CodigoEquipo VARCHAR(100),
    @NumeroSerial VARCHAR(100),
    @IdUsuarioDevuelve INT,
    @Resultado BIT OUTPUT,
    @Mensaje NVARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdPrestamo INT;
        DECLARE @IdEquipo INT;
        DECLARE @Cantidad INT;

        -- Obtener ID del préstamo
        SELECT @IdPrestamo = IdPrestamo
        FROM PRESTAMO
        WHERE NumeroDocumento = @NumeroDocumento;

        IF @IdPrestamo IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró el préstamo.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Obtener el IdEquipo y Cantidad del detalle
        SELECT 
            @IdEquipo = DP.IdEquipo,
            @Cantidad = DP.Cantidad
        FROM DETALLE_PRESTAMO DP
        INNER JOIN EQUIPO E ON DP.IdEquipo = E.IdEquipo
        WHERE DP.IdPrestamo = @IdPrestamo
          AND E.Codigo = @CodigoEquipo
          AND DP.NumeroSerial = @NumeroSerial;

        IF @IdEquipo IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró el equipo dentro del préstamo.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Actualizar solo ese detalle
        UPDATE DETALLE_PRESTAMO
        SET EstadoDevolucion = 'DEVUELTO',
            IdUsuarioDevuelve = @IdUsuarioDevuelve
        WHERE IdPrestamo = @IdPrestamo
          AND IdEquipo = @IdEquipo
          AND NumeroSerial = @NumeroSerial;

        -- Sumar stock solo del equipo devuelto
        UPDATE EQUIPO
        SET Stock = Stock + @Cantidad
        WHERE IdEquipo = @IdEquipo;

        -- Verificar si todos los detalles ya fueron devueltos
        IF NOT EXISTS (
            SELECT 1
            FROM DETALLE_PRESTAMO
            WHERE IdPrestamo = @IdPrestamo
              AND EstadoDevolucion <> 'DEVUELTO'
        )
        BEGIN
            UPDATE PRESTAMO
            SET EstadoPrestamo = 'DEVUELTO',
                FechaDevolucion = GETDATE()
            WHERE IdPrestamo = @IdPrestamo;
        END

        SET @Resultado = 1;
        SET @Mensaje = 'Equipo devuelto correctamente.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO
