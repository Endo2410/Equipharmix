
-- ============================================
-- Trigger de ROL
-- ============================================
CREATE TRIGGER TR_ROL_AUDITORIA
ON ROL
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);

    -- Obtener el usuario desde SESSION_CONTEXT
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    -----------------------
    -- 1. Auditoría INSERT
    -----------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT 
        'ROL',
        'INSERT',
        @Usuario,
        'Se creó el rol "' + i.Descripcion + '" con estado ' +
        CASE WHEN i.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END
    FROM inserted i
    LEFT JOIN deleted d ON i.IdRol = d.IdRol
    WHERE d.IdRol IS NULL;


    -----------------------
    -- 2. Auditoría UPDATE
    -----------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'ROL',
        'UPDATE',
        @Usuario,
        CASE
            WHEN i.Descripcion <> d.Descripcion AND i.Estado <> d.Estado THEN
                'Se modificó el rol "' + d.Descripcion + 
                '" ? Nuevo nombre "' + i.Descripcion +
                '" y estado cambiado de ' +
                CASE WHEN d.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END +
                ' a ' +
                CASE WHEN i.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END

            WHEN i.Descripcion <> d.Descripcion THEN
                'Se cambió nombre del rol "' + d.Descripcion +
                '" a "' + i.Descripcion + '"'

            WHEN i.Estado <> d.Estado THEN
                'Se cambió estado del rol "' + i.Descripcion +
                '" de ' +
                CASE WHEN d.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END +
                ' a ' +
                CASE WHEN i.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END

            ELSE
                'Se actualizó el rol "' + i.Descripcion + '" sin cambios relevantes'
        END
    FROM inserted i
    INNER JOIN deleted d ON i.IdRol = d.IdRol;
END
GO

-- ============================================
-- Trigger de MARCA
-- ============================================
CREATE TRIGGER TR_MARCA_AUDITORIA
ON MARCA
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    -----------------------------------
    -- 1. AUDITORÍA DE INSERT
    -----------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'MARCA',
        'INSERT',
        @Usuario,
        'Se creó la marca "' + i.Descripcion + '" con estado ' +
        CASE WHEN i.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END
    FROM inserted i
    LEFT JOIN deleted d ON i.IdMarca = d.IdMarca
    WHERE d.IdMarca IS NULL;


    -----------------------------------
    -- 2. AUDITORÍA DE UPDATE
    -----------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'MARCA',
        'UPDATE',
        @Usuario,
        CASE
            -- Cambió nombre y estado
            WHEN i.Descripcion <> d.Descripcion AND i.Estado <> d.Estado THEN
                'Se modificó la marca "' + d.Descripcion +
                '" ? nuevo nombre "' + i.Descripcion +
                '" y estado cambiado de ' +
                CASE WHEN d.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END +
                ' a ' +
                CASE WHEN i.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END

            -- Solo cambió el nombre
            WHEN i.Descripcion <> d.Descripcion THEN
                'Se cambió el nombre de la marca "' + d.Descripcion +
                '" a "' + i.Descripcion + '"'

            -- Solo cambió el estado
            WHEN i.Estado <> d.Estado THEN
                'Se cambió el estado de la marca "' + i.Descripcion +
                '" de ' +
                CASE WHEN d.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END +
                ' a ' +
                CASE WHEN i.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END

            -- Sin cambios relevantes (por si ocurre)
            ELSE
                'Se actualizó la marca "' + i.Descripcion + '" sin cambios relevantes'
        END
    FROM inserted i
    INNER JOIN deleted d ON i.IdMarca = d.IdMarca;
END;
GO


-- ============================================
-- Trigger de NEGOCIO
-- ============================================
CREATE TRIGGER TR_NEGOCIO_AUDITORIA
ON NEGOCIO
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    --------------------------------
    -- 1. INSERT
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'NEGOCIO',
        'INSERT',
        @Usuario,
        'Se registró el negocio "' + i.Nombre +
        '" con RUC "' + i.RUC +
        '" y dirección "' + i.Direccion + '"'
    FROM inserted i
    LEFT JOIN deleted d ON i.IdNegocio = d.IdNegocio
    WHERE d.IdNegocio IS NULL;

    --------------------------------
    -- 2. UPDATE
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'NEGOCIO',
        'UPDATE',
        @Usuario,
        CASE
            WHEN i.Nombre <> d.Nombre AND i.RUC <> d.RUC AND i.Direccion <> d.Direccion THEN
                'Se actualizó el negocio "' + d.Nombre +
                '" ? Nuevo nombre "' + i.Nombre +
                '", nuevo RUC "' + i.RUC +
                '" y nueva dirección "' + i.Direccion + '"'

            WHEN i.Nombre <> d.Nombre AND i.RUC <> d.RUC THEN
                'Se actualizó el nombre del negocio "' + d.Nombre +
                '" a "' + i.Nombre +
                '" y RUC cambiado a "' + i.RUC + '"'

            WHEN i.Nombre <> d.Nombre AND i.Direccion <> d.Direccion THEN
                'Se cambió el nombre del negocio "' + d.Nombre +
                '" a "' + i.Nombre +
                '" y su dirección a "' + i.Direccion + '"'

            WHEN i.RUC <> d.RUC AND i.Direccion <> d.Direccion THEN
                'Se actualizó el RUC del negocio "' + i.Nombre +
                '" a "' + i.RUC +
                '" y su dirección a "' + i.Direccion + '"'

            WHEN i.Nombre <> d.Nombre THEN
                'Se cambió el nombre del negocio de "' + d.Nombre +
                '" a "' + i.Nombre + '"'

            WHEN i.RUC <> d.RUC THEN
                'Se cambió el RUC del negocio "' + i.Nombre +
                '" de "' + d.RUC +
                '" a "' + i.RUC + '"'

            WHEN i.Direccion <> d.Direccion THEN
                'Se cambió la dirección del negocio "' + i.Nombre +
                '" de "' + d.Direccion +
                '" a "' + i.Direccion + '"'

            WHEN (i.Logo IS NOT NULL AND d.Logo IS NOT NULL) 
                 AND HASHBYTES('SHA2_256', i.Logo) <> HASHBYTES('SHA2_256', d.Logo) THEN
                'Se actualizó el logo del negocio: tamaño anterior = ' +
                CAST(DATALENGTH(d.Logo) AS NVARCHAR(20)) + ' bytes, ' +
                'nuevo tamaño = ' + CAST(DATALENGTH(i.Logo) AS NVARCHAR(20)) + ' bytes'

            ELSE
                'Se actualizó el negocio "' + i.Nombre + '" sin cambios relevantes'
        END
    FROM inserted i
    INNER JOIN deleted d ON i.IdNegocio = d.IdNegocio;

END;
GO

-- ============================================
-- Trigger de USUARIO
-- ============================================
CREATE TRIGGER TR_USUARIO_AUDITORIA
ON USUARIO
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    --------------------------------
    -- 1. INSERT
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
	SELECT 
		'USUARIO',
		'INSERT',
		@Usuario,
		'Se creó el usuario "' + ISNULL(i.NombreCompleto,'N/A') + '" con rol "' + 
		ISNULL(r.Descripcion,'N/A') + '"'
	FROM inserted i
	LEFT JOIN deleted d ON i.IdUsuario = d.IdUsuario
	LEFT JOIN ROL r ON i.IdRol = r.IdRol
	WHERE d.IdUsuario IS NULL;

    --------------------------------
    -- 2. UPDATE (campo por campo)
    --------------------------------

	INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
	SELECT
		'USUARIO',
		'UPDATE',
		@Usuario,
		CASE
			WHEN ISNULL(i.NombreCompleto,'') <> ISNULL(d.NombreCompleto,'') THEN
				'Se cambió el nombre del usuario de "' + ISNULL(d.NombreCompleto,'N/A') +
				'" a "' + ISNULL(i.NombreCompleto,'N/A') + '"'

			WHEN ISNULL(i.IdRol,0) <> ISNULL(d.IdRol,0) THEN
				'Se cambió el rol del usuario "' + ISNULL(i.NombreCompleto,'N/A') +
				'" de ' + CAST(ISNULL(d.IdRol,0) AS NVARCHAR) +
				' a ' + CAST(ISNULL(i.IdRol,0) AS NVARCHAR)

			WHEN ISNULL(i.Correo,'') <> ISNULL(d.Correo,'') THEN
				'Se cambió el correo del usuario "' + ISNULL(i.NombreCompleto,'N/A') +
				'" de "' + ISNULL(d.Correo,'N/A') + '" a "' + ISNULL(i.Correo,'N/A') + '"'

			WHEN ISNULL(i.Clave,'') <> ISNULL(d.Clave,'') THEN
				'Se cambió la contraseña del usuario "' + ISNULL(i.NombreCompleto,'N/A') + '"'

			WHEN ISNULL(i.Estado,0) <> ISNULL(d.Estado,0) THEN
				'Se cambió el estado del usuario "' + ISNULL(i.NombreCompleto,'N/A') + 
				'" de ' + CASE WHEN ISNULL(d.Estado,0)=1 THEN 'Activo' ELSE 'No Activo' END +
				' a ' + CASE WHEN ISNULL(i.Estado,0)=1 THEN 'Activo' ELSE 'No Activo' END

			ELSE
				'Se actualizó el usuario "' + ISNULL(i.NombreCompleto,'N/A') + '" sin cambios relevantes'
		END
	FROM inserted i
	INNER JOIN deleted d ON i.IdUsuario = d.IdUsuario;

END;
GO

-- ============================================
-- Trigger de FARMACIA
-- ============================================
CREATE TRIGGER TR_FARMACIA_AUDITORIA
ON FARMACIA
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    --------------------------------
    -- 1. INSERT
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'FARMACIA',
        'INSERT',
        @Usuario,
        'Se registró la farmacia "' + i.Nombre +
        '" con código "' + i.Codigo +
        '" y teléfono "' + ISNULL(i.Telefono,'N/A') + '"'
    FROM inserted i
    LEFT JOIN deleted d ON i.IdFarmacia = d.IdFarmacia
    WHERE d.IdFarmacia IS NULL;


    --------------------------------
    -- 2. UPDATE
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'FARMACIA',
        'UPDATE',
        @Usuario,
        CASE
            WHEN i.Nombre <> d.Nombre AND i.Codigo <> d.Codigo AND i.Telefono <> d.Telefono
                 AND i.Estado <> d.Estado AND ISNULL(i.Correo,'') <> ISNULL(d.Correo,'') THEN
                'Se actualizaron todos los datos de la farmacia "' + d.Nombre + '"'

            WHEN i.Nombre <> d.Nombre THEN
                'Se cambió el nombre de la farmacia "' + d.Nombre +
                '" a "' + i.Nombre + '"'

            WHEN i.Codigo <> d.Codigo THEN
                'Se cambió el código de la farmacia "' + i.Nombre +
                '" de "' + d.Codigo + '" a "' + i.Codigo + '"'

            WHEN i.Telefono <> d.Telefono THEN
                'Se actualizó el teléfono de la farmacia "' + i.Nombre +
                '" de "' + ISNULL(d.Telefono,'N/A') +
                '" a "' + ISNULL(i.Telefono,'N/A') + '"'

            WHEN i.Estado <> d.Estado THEN
                'Se cambió el estado de la farmacia "' + i.Nombre +
                '" de ' + CASE WHEN d.Estado=1 THEN 'Activa' ELSE 'Inactiva' END +
                ' a ' + CASE WHEN i.Estado=1 THEN 'Activa' ELSE 'Inactiva' END

            WHEN ISNULL(i.Correo,'') <> ISNULL(d.Correo,'') THEN
                'Se cambió el correo de la farmacia "' + i.Nombre +
                '" de "' + ISNULL(d.Correo,'N/A') +
                '" a "' + ISNULL(i.Correo,'N/A') + '"'

            ELSE
                'Se actualizó la farmacia "' + i.Nombre + '" sin cambios relevantes'
        END
    FROM inserted i
    INNER JOIN deleted d ON i.IdFarmacia = d.IdFarmacia;
END;
GO

-- ============================================
-- Trigger de EQUIPO
-- ============================================
CREATE TRIGGER TR_EQUIPO_AUDITORIA 
ON EQUIPO 
AFTER INSERT, UPDATE 
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    --------------------------------
    -- 1. INSERT
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'EQUIPO',
        'INSERT',
        @Usuario,
        'Se registró el equipo "' + ISNULL(i.Nombre,'N/A') + 
        '" con código "' + ISNULL(i.Codigo,'N/A') + 
        '", descripción "' + ISNULL(i.Descripcion,'N/A') + 
        '", marca "' + ISNULL(m.Descripcion,'N/A') + 
        '", estado "' + ISNULL(e.Descripcion,'N/A') + 
        '" y stock ' + CAST(i.Stock AS VARCHAR(10))
    FROM inserted i
    LEFT JOIN MARCA m ON i.IdMarca = m.IdMarca
    LEFT JOIN ESTADO e ON i.IdEstado = e.IdEstado
    LEFT JOIN deleted d ON i.IdEquipo = d.IdEquipo
    WHERE d.IdEquipo IS NULL;

    --------------------------------
    -- 2. UPDATE (campo por campo)
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'EQUIPO',
        'UPDATE',
        @Usuario,
        CASE
            WHEN i.Nombre <> d.Nombre THEN
                'Se cambió el nombre del equipo "' + ISNULL(d.Nombre,'N/A') +
                '" a "' + ISNULL(i.Nombre,'N/A') + '"'

            WHEN ISNULL(i.Codigo,'') <> ISNULL(d.Codigo,'') THEN
                'Se cambió el código del equipo "' + ISNULL(i.Nombre,'N/A') +
                '" de "' + ISNULL(d.Codigo,'N/A') + '" a "' + ISNULL(i.Codigo,'N/A') + '"'

            WHEN ISNULL(i.Descripcion,'') <> ISNULL(d.Descripcion,'') THEN
                'Se cambió la descripción del equipo "' + ISNULL(i.Nombre,'N/A') +
                '" de "' + ISNULL(d.Descripcion,'N/A') + '" a "' + ISNULL(i.Descripcion,'N/A') + '"'

            WHEN ISNULL(i.IdMarca,0) <> ISNULL(d.IdMarca,0) THEN
                'Se cambió la marca del equipo "' + ISNULL(i.Nombre,'N/A') +
                '" de "' + ISNULL(dm.Descripcion,'N/A') + '" a "' + ISNULL(m.Descripcion,'N/A') + '"'

            WHEN i.Stock <> d.Stock THEN
                'Se cambió el stock del equipo "' + ISNULL(i.Nombre,'N/A') +
                '" de ' + CAST(d.Stock AS VARCHAR(10)) + ' a ' + CAST(i.Stock AS VARCHAR(10))

            WHEN ISNULL(i.IdEstado,0) <> ISNULL(d.IdEstado,0) THEN
                'Se cambió el estado del equipo "' + ISNULL(i.Nombre,'N/A') +
                '" de "' + ISNULL(de.Descripcion,'N/A') + '" a "' + ISNULL(e.Descripcion,'N/A') + '"'

            ELSE
                'Se actualizó el equipo "' + ISNULL(i.Nombre,'N/A') + '" sin cambios relevantes'
        END
    FROM inserted i
    INNER JOIN deleted d ON i.IdEquipo = d.IdEquipo
    LEFT JOIN MARCA m  ON i.IdMarca = m.IdMarca
    LEFT JOIN MARCA dm ON d.IdMarca = dm.IdMarca
    LEFT JOIN ESTADO e  ON i.IdEstado = e.IdEstado
    LEFT JOIN ESTADO de ON d.IdEstado = de.IdEstado;

END;
GO


-- ============================================
-- Trigger de PERMISO
CREATE TRIGGER TR_PERMISO_AUDITORIA 
ON PERMISO
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Usuario NVARCHAR(100);
    SELECT @Usuario = CAST(SESSION_CONTEXT(N'Usuario') AS NVARCHAR(100));

    --------------------------------
    -- 1. INSERT
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'PERMISO',
        'INSERT',
        @Usuario,
        'Se asignó un nuevo permiso: Rol="' + ISNULL(r.Descripcion,'N/A') + '"' +
        ISNULL(', Módulo="' + m.NombreModulo + '"','') +
        ISNULL(', SubMenú="' + s.NombreSubMenu + '"','') +
        ISNULL(', Acción="' + a.NombreAccion + '"','')
    FROM inserted i
    LEFT JOIN ROL r ON i.IdRol = r.IdRol
    LEFT JOIN MODULO m ON i.IdModulo = m.IdModulo
    LEFT JOIN SUBMENU s ON i.IdSubMenu = s.IdSubMenu
    LEFT JOIN ACCION a ON i.IdAccion = a.IdAccion;

    --------------------------------
    -- 2. DELETE
    --------------------------------
    INSERT INTO AUDITORIA (Tabla, Operacion, Usuario, Datos)
    SELECT
        'PERMISO',
        'DELETE',
        @Usuario,
        'Se eliminó el permiso: Rol="' + ISNULL(r.Descripcion,'N/A') + '"' +
        ISNULL(', Módulo="' + m.NombreModulo + '"','') +
        ISNULL(', SubMenú="' + s.NombreSubMenu + '"','') +
        ISNULL(', Acción="' + a.NombreAccion + '"','')
    FROM deleted d
    LEFT JOIN ROL r ON d.IdRol = r.IdRol
    LEFT JOIN MODULO m ON d.IdModulo = m.IdModulo
    LEFT JOIN SUBMENU s ON d.IdSubMenu = s.IdSubMenu
    LEFT JOIN ACCION a ON d.IdAccion = a.IdAccion;

END;
GO




delete from AUDITORIA


CREATE PROCEDURE SP_Reporte_Auditoria
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdAuditoria,
        Tabla,
        Operacion,
        Usuario,
        Fecha,
        Datos
    FROM AUDITORIA
     WHERE Fecha >= @FechaInicio
      AND Fecha < DATEADD(DAY, 1, @FechaFin)  -- Incluye toda la fecha final
END
GO

