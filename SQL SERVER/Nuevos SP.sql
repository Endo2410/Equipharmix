---Actualizar clave 
CREATE PROCEDURE SP_ACTUALIZAR_CLAVE
    @IdUsuario INT,
    @NuevaClave NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE USUARIO
    SET Clave = @NuevaClave
    WHERE IdUsuario = @IdUsuario;
END
GO


---OBTENER USUARIOS
CREATE PROCEDURE SP_LISTAR_USUARIOS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.IdUsuario,
        u.Documento,
        u.NombreCompleto,
        u.NombreUsuario,
        u.Correo,
        u.Clave,
        u.Estado,
        r.IdRol,
        r.Descripcion AS RolDescripcion
    FROM USUARIO u
    INNER JOIN ROL r ON r.IdRol = u.IdRol;
END
GO

---LISTAR ROL
CREATE PROCEDURE SP_LISTAR_ROL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdRol,
        Descripcion,
        Estado
    FROM ROL
END


----LISTAR SUB MENU
CREATE PROCEDURE SP_LISTAR_SUB_MENU
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdSubMenu,
        IdModulo,
        NombreSubMenu
    FROM SUBMENU
END

---OBTENER CORRELATIVO 
CREATE PROCEDURE SP_OBTENER_CORRELATIVO_REGISTRAR
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) + 1 AS Correlativo FROM REGISTRAR;
END
GO

---OBTENER REGISTRO 
CREATE PROCEDURE SP_OBTENER_REGISTRO_POR_NUMERO
(
    @NumeroDocumento VARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.IdRegistrar,
        u.NombreCompleto,
        c.TipoDocumento,
        c.NumeroDocumento,
        CONVERT(char(10), c.FechaRegistro, 103) AS FechaRegistro
    FROM REGISTRAR c
    INNER JOIN USUARIO u ON u.IdUsuario = c.IdUsuario
    WHERE c.NumeroDocumento = @NumeroDocumento;
END
GO

---OBTENER DETALLE
CREATE PROCEDURE SP_OBTENER_DETALLE_REGISTRAR
(
    @IdRegistrar INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Nombre,
        dc.Cantidad
    FROM DETALLE_REGISTRAR dc
    INNER JOIN EQUIPO p ON p.IdEquipo = dc.IdEquipo
    WHERE dc.IdRegistrar = @IdRegistrar;
END
GO


--LISTAR MODULO
CREATE PROCEDURE SP_LISTAR_MODULO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdModulo,
        NombreModulo
    FROM MODULO;
END
GO

--LISTAR MARCA
CREATE PROCEDURE SP_LISTAR_MARCA
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdMarca,
        Descripcion,
        Estado
    FROM Marca;
END
GO

---LISTAR FARMACIA
CREATE PROCEDURE SP_LISTAR_FARMACIA
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdFarmacia,
        Codigo,
        Nombre,
        Telefono,
        Correo,
        Estado
    FROM FARMACIA;
END
GO


--LISTAR ESTADO
CREATE PROCEDURE SP_LISTAR_ESTADO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdEstado,
        Descripcion
    FROM ESTADO;
END
GO

--LISTAR EQUIPO
CREATE PROCEDURE SP_LISTAR_EQUIPO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.IdEquipo,
        p.Codigo,
        p.Nombre,
        p.Descripcion,
        c.IdMarca,
        c.Descripcion AS DescripcionMarca,
        p.Stock,
        e.IdEstado,
        e.Descripcion AS DescripcionEstado
    FROM Equipo p
    INNER JOIN Marca c ON c.IdMarca = p.IdMarca
    INNER JOIN Estado e ON e.IdEstado = p.IdEstado;
END
GO


CREATE PROCEDURE SP_LISTAR_ACCION
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdAccion,
        IdSubMenu,
        IdModulo,
        NombreAccion
    FROM ACCION;
END
GO


---OBTENER DETALLE DE ACTA
CREATE PROCEDURE SP_OBTENER_DETALLE_ACTA
    @IdActa INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Nombre,
        dv.Cantidad,
        dv.NumeroSerial,
        dv.Caja
    FROM DETALLE_ACTA dv
    INNER JOIN EQUIPO p ON p.IdEquipo = dv.IdEquipo
    WHERE dv.IdActa = @IdActa;
END
GO


CREATE PROCEDURE SP_OBTENER_ID_FARMACIA
    @Codigo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFarmacia
    FROM FARMACIA
    WHERE Codigo = @Codigo;
END
GO


CREATE PROCEDURE SP_SUMAR_STOCK
    @IdEquipo INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE EQUIPO
    SET Stock = Stock + @Cantidad
    WHERE IdEquipo = @IdEquipo;
END
GO


CREATE PROCEDURE SP_RESTAR_STOCK
    @IdEquipo INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE EQUIPO
    SET Stock = Stock - @Cantidad
    WHERE IdEquipo = @IdEquipo;
END
GO


CREATE PROCEDURE SP_OBTENER_NEGOCIO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdNegocio, Nombre, RUC, Direccion
    FROM NEGOCIO
    WHERE IdNegocio = 1;
END
GO

CREATE PROCEDURE SP_GUARDAR_NEGOCIO
    @Nombre NVARCHAR(100),
    @RUC NVARCHAR(20),
    @Direccion NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE NEGOCIO
    SET 
        Nombre = @Nombre,
        RUC = @RUC,
        Direccion = @Direccion
    WHERE IdNegocio = 1;
END
GO

CREATE PROCEDURE SP_OBTENER_LOGO_NEGOCIO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Logo
    FROM NEGOCIO
    WHERE IdNegocio = 1;
END
GO


CREATE PROCEDURE SP_ACTUALIZAR_LOGO_NEGOCIO
    @Logo VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE NEGOCIO
    SET Logo = @Logo
    WHERE IdNegocio = 1;
END
GO


CREATE PROCEDURE SP_LISTAR_PERMISOS_POR_USUARIO
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.IdPermiso, 
        p.IdRol, 
        r.Descripcion AS Rol,
        p.IdModulo, 
        m.NombreModulo,
        p.IdSubMenu, 
        s.NombreSubMenu,
        p.IdAccion, 
        a.NombreAccion
    FROM PERMISO p
    INNER JOIN ROL r ON r.IdRol = p.IdRol
    LEFT JOIN MODULO m ON m.IdModulo = p.IdModulo
    LEFT JOIN SUBMENU s ON s.IdSubMenu = p.IdSubMenu
    LEFT JOIN ACCION a ON a.IdAccion = p.IdAccion
    INNER JOIN USUARIO u ON u.IdRol = r.IdRol
    WHERE u.IdUsuario = @IdUsuario;
END
GO


CREATE PROCEDURE SP_LISTAR_PERMISOS_POR_ROL
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.IdPermiso,
        p.IdRol,
        p.IdModulo,
        m.NombreModulo,
        p.IdSubMenu,
        s.NombreSubMenu,
        p.IdAccion,
        a.NombreAccion
    FROM PERMISO p
    LEFT JOIN MODULO m ON m.IdModulo = p.IdModulo
    LEFT JOIN SUBMENU s ON s.IdSubMenu = p.IdSubMenu
    LEFT JOIN ACCION a ON a.IdAccion = p.IdAccion
    WHERE p.IdRol = @IdRol;
END
GO




CREATE TYPE TipoPermiso AS TABLE
(
    IdModulo INT,
    IdSubMenu INT NULL,
    IdAccion INT NULL
);
GO

CREATE PROCEDURE SP_GUARDAR_PERMISOS_COMPLETO
    @IdRol INT,
    @Permisos TipoPermiso READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -------------------------------------------------------------
        -- 1?? Crear tabla temporal con los permisos actuales
        -------------------------------------------------------------
        DECLARE @PermisosExistentes TABLE (
            IdPermiso INT,
            IdModulo INT,
            IdSubMenu INT NULL,
            IdAccion INT NULL
        );

        INSERT INTO @PermisosExistentes (IdPermiso, IdModulo, IdSubMenu, IdAccion)
        SELECT IdPermiso, IdModulo, IdSubMenu, IdAccion
        FROM PERMISO
        WHERE IdRol = @IdRol;

        -------------------------------------------------------------
        -- 2?? Eliminar los permisos que ya no existen en la lista nueva
        -------------------------------------------------------------
        DELETE FROM PERMISO
        WHERE IdRol = @IdRol
          AND NOT EXISTS (
              SELECT 1
              FROM @Permisos P
              WHERE ISNULL(P.IdModulo, -1) = ISNULL(PERMISO.IdModulo, -1)
                AND ISNULL(P.IdSubMenu, -1) = ISNULL(PERMISO.IdSubMenu, -1)
                AND ISNULL(P.IdAccion, -1) = ISNULL(PERMISO.IdAccion, -1)
          );

        -------------------------------------------------------------
        -- 3?? Insertar permisos nuevos que no estaban antes
        -------------------------------------------------------------
        INSERT INTO PERMISO (IdRol, IdModulo, IdSubMenu, IdAccion)
        SELECT
            @IdRol,
            P.IdModulo,
            P.IdSubMenu,
            P.IdAccion
        FROM @Permisos P
        WHERE NOT EXISTS (
            SELECT 1
            FROM @PermisosExistentes E
            WHERE ISNULL(E.IdModulo, -1) = ISNULL(P.IdModulo, -1)
              AND ISNULL(E.IdSubMenu, -1) = ISNULL(P.IdSubMenu, -1)
              AND ISNULL(E.IdAccion, -1) = ISNULL(P.IdAccion, -1)
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO




CREATE PROCEDURE SP_OBTENER_REPORTE_FARMACIA
(
    @FechaInicio DATE,
    @FechaFin DATE
)
AS
BEGIN
    SELECT 
        IdFarmacia,
        Codigo,
        Nombre,
        Telefono,
        Correo,
        Estado,
        FechaRegistro
    FROM FARMACIA
    WHERE FechaRegistro >= @FechaInicio
      AND FechaRegistro < DATEADD(DAY, 1, @FechaFin)  -- Incluye toda la fecha final
END
GO
