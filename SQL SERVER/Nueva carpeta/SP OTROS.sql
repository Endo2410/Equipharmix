USE Inventario_IT
GO

/****************** TIPOS DE DATOS ******************/
/* Tabla para detalle de registros (entrada de equipos) */
CREATE TYPE [dbo].[EDetalle_Registrar] AS TABLE(
	[IdEquipo] INT NULL,
	[Cantidad] INT NULL
)
GO

/****************** PROCEDIMIENTOS ALMACENADOS PARA USUARIOS ******************/
/*-----------------------------------------------------------------------------------*/

/*
 * SP_REGISTRARUSUARIO
 * Sirve para registrar un nuevo usuario validando que no exista otro con el mismo documento.
 */
CREATE PROCEDURE SP_REGISTRAR_USUARIO (
    @Documento         VARCHAR(50),
    @NombreCompleto    VARCHAR(100),
    @NombreUsuario     VARCHAR(50),
    @Correo            VARCHAR(100),
    @Clave             VARCHAR(100),
    @IdRol             INT,
    @Estado            BIT,
    @IdUsuarioResultado INT OUTPUT,
    @Mensaje            VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SET @IdUsuarioResultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM USUARIO WHERE UPPER(Documento) = UPPER(@Documento)
        )
        BEGIN
            INSERT INTO USUARIO (
                Documento, NombreCompleto, NombreUsuario, Correo, Clave, IdRol, Estado
            ) VALUES (
                @Documento, @NombreCompleto, @NombreUsuario, @Correo, @Clave, @IdRol, @Estado
            );

            SET @IdUsuarioResultado = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se puede repetir el documento para más de un usuario.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/*
 * SP_EDITARUSUARIO
 * Sirve para modificar datos de un usuario existente validando que no exista otro con el mismo documento.
 */
CREATE PROCEDURE SP_EDITAR_USUARIO (
    @IdUsuario INT,
    @Documento VARCHAR(50),
    @NombreCompleto VARCHAR(100),
    @NombreUsuario VARCHAR(50),
    @Correo VARCHAR(100),
    @Clave VARCHAR(100),
    @IdRol INT,
    @Estado BIT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Respuesta = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM USUARIO 
            WHERE UPPER(Documento) = UPPER(@Documento) 
              AND IdUsuario != @IdUsuario
        )
        BEGIN
            UPDATE USUARIO
            SET
                Documento = @Documento,
                NombreCompleto = @NombreCompleto,
                NombreUsuario = @NombreUsuario,
                Correo = @Correo,
                Clave = @Clave,
                IdRol = @IdRol,
                Estado = @Estado
            WHERE IdUsuario = @IdUsuario;

            SET @Respuesta = 1;
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se puede repetir el documento para más de un usuario.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

select * from USUARIO

/****************** PROCEDIMIENTOS ALMACENADOS PARA MARCAS ******************/
/*---------------------------------------------------------------------------------*/

/*
 * SP_RegistrarMarca
 * Sirve para registrar una nueva marca validando que no exista otra con la misma descripción (insensible a mayúsculas).
 */
CREATE PROCEDURE SP_REGISTRAR_MARCA (
    @Descripcion VARCHAR(50),
    @Estado BIT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM MARCA WHERE UPPER(RTRIM(LTRIM(Descripcion))) = UPPER(RTRIM(LTRIM(@Descripcion)))
        )
        BEGIN
            INSERT INTO MARCA (Descripcion, Estado)
            VALUES (@Descripcion, @Estado);

            SET @Resultado = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se puede repetir la descripción de una marca.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/*
 * sp_EditarMarca
 * Sirve para modificar una marca existente validando que no exista otra con la misma descripción.
 */
CREATE PROCEDURE SP_EDITAR_MARCA (
    @IdMarca INT,
    @Descripcion VARCHAR(50),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM MARCA 
            WHERE UPPER(RTRIM(LTRIM(Descripcion))) = UPPER(RTRIM(LTRIM(@Descripcion)))
              AND IdMarca != @IdMarca
        )
        BEGIN
            UPDATE MARCA
            SET 
                Descripcion = @Descripcion,
                Estado = @Estado
            WHERE IdMarca = @IdMarca;

            SET @Resultado = 1;
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se puede repetir la descripción de una marca.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/****************** PROCEDIMIENTOS ALMACENADOS PARA ROLES ******************/
/*---------------------------------------------------------------------------------*/

/*
 * SP_RegistrarRoles
 * Sirve para registrar una nueva marca validando que no exista otra con la misma descripción (insensible a mayúsculas).
 */
CREATE PROCEDURE SP_REGISTRAR_ROLES (
    @Descripcion VARCHAR(50),
    @Estado BIT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM ROL WHERE UPPER(RTRIM(LTRIM(Descripcion))) = UPPER(RTRIM(LTRIM(@Descripcion)))
        )
        BEGIN
            INSERT INTO ROL (Descripcion, Estado)
            VALUES (@Descripcion, @Estado);

            SET @Resultado = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se puede repetir la descripción de un rol.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/*
 * sp_EditarMarca
 * Sirve para modificar una marca existente validando que no exista otra con la misma descripción.
 */
CREATE PROCEDURE SP_EDITAR_ROL (
    @IdRol INT,
    @Descripcion VARCHAR(50),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM ROL 
            WHERE UPPER(RTRIM(LTRIM(Descripcion))) = UPPER(RTRIM(LTRIM(@Descripcion)))
              AND IdRol != @IdRol
        )
        BEGIN
            UPDATE ROL
            SET 
                Descripcion = @Descripcion,
                Estado = @Estado
            WHERE IdRol = @IdRol;

            SET @Resultado = 1;
        END
        ELSE
        BEGIN
            SET @Mensaje = 'No se puede repetir la descripción de un rol.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTOS ALMACENADOS PARA EQUIPOS ******************/
/*----------------------------------------------------------------------------------*/

/*
 * sp_RegistrarEquipo
 * Sirve para registrar un nuevo equipo validando que no exista otro con el mismo código.
 */
CREATE PROCEDURE SP_REGISTRAR_EQUIPO (
    @Codigo VARCHAR(20),
    @Nombre VARCHAR(30),
    @Descripcion VARCHAR(30),
    @IdMarca INT,
    @IdEstado INT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM EQUIPO 
            WHERE UPPER(RTRIM(LTRIM(Codigo))) = UPPER(RTRIM(LTRIM(@Codigo)))
        )
        BEGIN
            INSERT INTO EQUIPO (
                Codigo, Nombre, Descripcion, IdMarca, IdEstado
            )
            VALUES (
                @Codigo, @Nombre, @Descripcion, @IdMarca, @IdEstado
            );

            SET @Resultado = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @Mensaje = 'Ya existe un equipo con el mismo código.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/*
 * sp_ModificarEquipo
 * Sirve para modificar un equipo existente validando que no exista otro con el mismo código.
 */
CREATE PROCEDURE SP_MODIFICAR_EQUIPO (
    @IdEquipo INT,
    @Codigo VARCHAR(20),
    @Nombre VARCHAR(30),
    @Descripcion VARCHAR(30),
    @IdMarca INT,
    @IdEstado INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM EQUIPO 
            WHERE UPPER(RTRIM(LTRIM(Codigo))) = UPPER(RTRIM(LTRIM(@Codigo)))
              AND IdEquipo != @IdEquipo
        )
        BEGIN
            UPDATE EQUIPO
            SET 
                Codigo = @Codigo,
                Nombre = @Nombre,
                Descripcion = @Descripcion,
                IdMarca = @IdMarca,
                IdEstado = @IdEstado
            WHERE IdEquipo = @IdEquipo;

            SET @Resultado = 1;
        END
        ELSE
        BEGIN
            SET @Mensaje = 'Ya existe un equipo con el mismo código.';
        END
    END TRY
    BEGIN CATCH
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO


/****************** PROCEDIMIENTOS ALMACENADOS PARA REPORTES ******************/
/*----------------------------------------------------------------------------------*/

/*
 * sp_ReporteRegistrar
 * Sirve para obtener un reporte de los registros de entradas filtrados por fecha.
 */
CREATE PROCEDURE SP_REPORTE_REGISTRAR(
 @fechainicio VARCHAR(10),
 @fechafin VARCHAR(10)
 )
AS
BEGIN
    SET DATEFORMAT dmy;
    SELECT 
        CONVERT(CHAR(10),c.FechaRegistro,103)[FechaRegistro],
        c.TipoDocumento,
        c.NumeroDocumento,
        u.NombreCompleto[UsuarioRegistro],
        p.Codigo[CodigoEquipo],
        p.Nombre[Equipo],
        ca.Descripcion[Marca],
        dc.Cantidad
    FROM REGISTRAR c
    INNER JOIN USUARIO u ON u.IdUsuario = c.IdUsuario
    INNER JOIN DETALLE_REGISTRAR dc ON dc.IdRegistrar = c.IdRegistrar
    INNER JOIN EQUIPO p ON p.IdEquipo = dc.IdEquipo
    INNER JOIN MARCA ca ON ca.IdMarca = p.IdMarca
    WHERE CONVERT(DATE,c.FechaRegistro) BETWEEN @fechainicio AND @fechafin
END
GO

/*
 * usp_Obtenerreporteacta
 * Sirve para obtener un reporte de actas autorizadas entre fechas.
 */
CREATE PROCEDURE SP_OBTENER_REPORTE_ACTA
(
    @FechaInicio DATE,
    @FechaFin DATE
)
AS
BEGIN
    SELECT 
        A.NumeroDocumento,
        A.FechaRegistro,
        F.Nombre AS NombreFarmacia,
        E.Codigo AS CodigoEquipo,
        E.Nombre AS NombreEquipo,
        ES.Descripcion AS Estado,
        DA.Cantidad,
        DA.NumeroSerial,
        DA.Caja,
        A.EstadoAutorizacion
    FROM ACTA A
    INNER JOIN FARMACIA F ON A.IdFarmacia = F.IdFarmacia
    INNER JOIN DETALLE_ACTA DA ON A.IdActa = DA.IdActa
    INNER JOIN EQUIPO E ON E.IdEquipo = DA.IdEquipo
    INNER JOIN ESTADO ES ON ES.IdEstado = E.IdEstado
    WHERE A.EstadoAutorizacion = 'AUTORIZADO'
      AND A.FechaRegistro >= @FechaInicio 
      AND A.FechaRegistro < DATEADD(DAY, 1, @FechaFin)
END
GO


/****************** PROCEDIMIENTOS ALMACENADOS PARA FARMACIA ******************/
/*----------------------------------------------------------------------------------*/

/*
 * sp_RegistrarFarmacia
 * Sirve para registrar una nueva farmacia validando que no exista otra con el mismo código.
 */
CREATE PROCEDURE SP_REGISTRAR_FARMACIA
(
    @Codigo VARCHAR(50),
    @Nombre VARCHAR(50),
    @Telefono VARCHAR(50),
    @Estado BIT,
    @Resultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

        SET @Resultado = 0;
        SET @Mensaje = '';

        IF EXISTS (SELECT 1 FROM FARMACIA WHERE Codigo = @Codigo)
        BEGIN
            SET @Mensaje = 'El código de farmacia ya existe.';
            RETURN;
        END

        INSERT INTO FARMACIA (Codigo, Nombre, Telefono, Estado)
        VALUES (@Codigo, @Nombre, @Telefono, @Estado);

        SET @Resultado = SCOPE_IDENTITY();

    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/*
 * sp_ModificarFarmacia
 * Sirve para modificar una farmacia existente validando que no exista otra con el mismo código.
 */
CREATE PROCEDURE SP_MODIFICAR_FARMACIA
(
    @IdFarmacia INT,
    @Codigo VARCHAR(50),
    @Nombre VARCHAR(50),
    @Telefono VARCHAR(50),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

        SET @Resultado = 1;
        SET @Mensaje = '';

        IF EXISTS (SELECT 1 FROM FARMACIA WHERE Codigo = @Codigo AND IdFarmacia != @IdFarmacia)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'El código ingresado ya pertenece a otra farmacia.';
            RETURN;
        END

        UPDATE FARMACIA
        SET Codigo = @Codigo,
            Nombre = @Nombre,
            Telefono = @Telefono,
            Estado = @Estado
        WHERE IdFarmacia = @IdFarmacia;

    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

/****************** PROCEDIMIENTOS ALMACENADOS PARA REGISTRAR ENTRADA ******************/
/*----------------------------------------------------------------------------------*/

/*
 * sp_Registrar
 * Sirve para registrar una entrada de equipos con detalle y actualizar stock en EQUIPO.
 */
CREATE PROCEDURE SP_REGISTRAR
(
    @IdUsuario INT,
    @TipoDocumento VARCHAR(500),
    @NumeroDocumento VARCHAR(500),
    @DetalleRegistrar EDetalle_Registrar READONLY,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

        DECLARE @IdRegistrar INT;

        SET @Resultado = 1;
        SET @Mensaje = '';

        BEGIN TRANSACTION registro;

        INSERT INTO REGISTRAR(IdUsuario, TipoDocumento, NumeroDocumento)
        VALUES(@IdUsuario, @TipoDocumento, @NumeroDocumento);

        SET @IdRegistrar = SCOPE_IDENTITY();

        INSERT INTO DETALLE_REGISTRAR(IdRegistrar, IdEquipo, Cantidad)
        SELECT @IdRegistrar, IdEquipo, Cantidad FROM @DetalleRegistrar;

        UPDATE E
        SET E.Stock = E.Stock + D.Cantidad
        FROM EQUIPO E
        INNER JOIN @DetalleRegistrar D ON D.IdEquipo = E.IdEquipo;

        COMMIT TRANSACTION registro;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION registro;
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO
