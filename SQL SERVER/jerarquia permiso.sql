-- 1. MODULO O MENU PRINCIPAL
CREATE TABLE MODULO (
    IdModulo INT IDENTITY(1,1) PRIMARY KEY,
    NombreModulo VARCHAR(100) NOT NULL,
	FechaRegistro DATETIME NULL CONSTRAINT DF_MODULO_FechaRegistro DEFAULT GETDATE()
);

-- 2. SUBMENU
CREATE TABLE SUBMENU (
    IdSubMenu INT IDENTITY(1,1) PRIMARY KEY,
    IdModulo INT NOT NULL,
    NombreSubMenu VARCHAR(100) NOT NULL,
	FechaRegistro DATETIME NULL CONSTRAINT DF_SUBMENU_FechaRegistro DEFAULT GETDATE(),
    FOREIGN KEY (IdModulo) REFERENCES MODULO(IdModulo)
);

-- 3. ACCION (botones o acciones dentro de un submenú)
CREATE TABLE ACCION (
    IdAccion INT IDENTITY(1,1) PRIMARY KEY,
	IdModulo INT NULL,
    IdSubMenu INT NULL,
    NombreAccion VARCHAR(100) NOT NULL,
	FechaRegistro DATETIME NULL CONSTRAINT DF_ACCION_FechaRegistro DEFAULT GETDATE(),
    FOREIGN KEY (IdModulo) REFERENCES MODULO(IdModulo),
	FOREIGN KEY (IdSubMenu) REFERENCES SUBMENU(IdSubMenu)
);

-- 4. PERMISO
CREATE TABLE PERMISO (
    IdPermiso INT IDENTITY(1,1) PRIMARY KEY,
    IdRol INT NOT NULL,
    IdModulo INT NULL,
    IdSubMenu INT NULL,
    IdAccion INT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdRol) REFERENCES ROL(IdRol),
    FOREIGN KEY (IdModulo) REFERENCES MODULO(IdModulo),
    FOREIGN KEY (IdSubMenu) REFERENCES SUBMENU(IdSubMenu),
    FOREIGN KEY (IdAccion) REFERENCES ACCION(IdAccion)
);


-- MODULOS
INSERT INTO MODULO (NombreModulo) VALUES 
('menuseguridad'),
('menumantenedor'), 
('menuregistrar'), 
('menuacta'), 
('menuprestamo'),
('menufarmacia'), 
('menuautorizacion'),
('menuasiganados'),
('menureportes'),
('menuacercade');
select * from MODULO

-- SUBMENUS
INSERT INTO SUBMENU (IdModulo, NombreSubMenu) VALUES
(10, 'submenuprestamo'),
(10, 'submenudetalleprestamo'),

(1, 'submenuroles'),
(1, 'submenupermisos'),
(1, 'submenuusuarios'),
(2, 'submenumarca'),
(2, 'submenuequipo'),
(2, 'submenunegocio'),
(3, 'submenuregistrarcompra'),
(3, 'submenuverdatallecompra'),
(4, 'submenuregistraracta'),
(4, 'submenuverdetalleacta'),
(6, 'submenuauctorizacionacta'),
(6, 'submenuautorizacionbaja'),
(6, 'submenuequiposbaja'),
(8, 'submenureporteacta'),
(8, 'submenureporteregistrar');


-- ACCIONES (ejemplo botones: Agregar, Editar, Eliminar, Ver)
INSERT INTO ACCION (IdSubMenu, NombreAccion) VALUES
(1, 'btnguardar'), (1, 'btnlimpiar'),(2, 'btnguardar'), (2, 'btncancelar'),(3, 'btnguardar'), (3, 'btnlimpiar'), (4, 'btnguardar'),(4, 'btnlimpiar'), (5, 'btnexportar'), 
(5, 'btnguardar'),(5, 'btnlimpiar'), (6, 'btnsubir'), (6, 'btnguardarcambios'), (7, 'btnagregarequipo'), (7, 'btnregistrar'), (8, 'btndescargar'),
(9, 'btncrearventa'), (10, 'btndescargar'), (12, 'btnguardar'), (12, 'btnlimpiar'), (12, 'btneliminar'), (13, 'btnlimpiar'), (13, 'btneliminar'),(13, 'btnexportar'),
(14, 'btnexportar'), (15, 'btnexportar');

INSERT INTO ACCION (IdModulo, NombreAccion) VALUES
(5,'btnguardar'), (5, 'btnlimpiar'), (7, 'btnlimpiar'), (7, 'btneliminar'), (7, 'btnguardarmotivo'), (7, 'btnexportar');


DECLARE @idRol INT = 2;

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
WHERE p.IdRol = @idRol;



INSERT INTO PERMISO (IdRol, IdModulo, IdSubMenu, IdAccion) VALUES
(1, 1, 2, 3) -- submenuroles sin acción

