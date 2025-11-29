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
('menuasiganados'),
('menuautorizacion'),
('menureportes'),
('menuacercade');



-- SUBMENUS
INSERT INTO SUBMENU (IdModulo, NombreSubMenu) VALUES
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
(4, 'submenuserie'),
(4, 'submenubuscarserie'),
(5, 'submenuprestamo'),
(5, 'submenudetalleprestamo'),
(5, 'submenuprestamo1'),
(8, 'submenuauctorizacionacta'),
(8, 'submenuautorizacionbaja'),
(8, 'submenuequiposbaja'),
(9, 'submenureportefarmacia'),
(9, 'submenureporteauditoria'),
(9, 'submenureporteusuario'),
(9, 'submenureporteacta'),
(9, 'submenureporteregistrar');

select * from SUBMENU
select * from MODULO


-- ACCIONES (ejemplo botones: Agregar, Editar, Eliminar, Ver)
INSERT INTO ACCION (IdSubMenu, NombreAccion) VALUES
(1, 'btnguardar'), (1, 'btnlimpiar'),(2, 'btnguardar'), (3, 'btnguardar'), (3, 'btnlimpiar'), (4, 'btnguardar'),(4, 'btnlimpiar'), (5, 'btnexportar'), 
(5, 'btnguardar'),(5, 'btnlimpiar'), (6, 'btnsubir'), (6, 'btnguardarcambios'), (7, 'btnagregarequipo'), (7, 'btnregistrar'), (8, 'btndescargar'),
(9, 'btncrearventa'), (10, 'btndescargar'),(16, 'btnguardar'), (16, 'btnlimpiar'), (16, 'btneliminar'), (17, 'btneliminar')
(14, 'btnexportar'), (15, 'btnexportar');

INSERT INTO ACCION (IdModulo, NombreAccion) VALUES
(6,'btnguardar'), (6, 'btnlimpiar'), (7, 'btnlimpiar'), (7, 'btneliminar'), (7, 'btnguardarmotivo'), (7, 'btnexportar');


DECLARE @idRol INT = 1;

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

select * from USUARIO
SELECT * from ROL
SELECT * from MODULO
SELECT * from SUBMENU
SELECT * from ACCION
