
/****************** INSERTAMOS REGISTROS A LAS TABLAS ******************/
/*---------------------------------------------------------------------*/

USE Inventario_IT
GO

-- INSETAR ESTADOS
INSERT INTO ESTADO (Descripcion) VALUES ('Activo');
INSERT INTO ESTADO (Descripcion) VALUES ('No Activo');
GO

-- INSERTAR ROL
 INSERT INTO ROL (Descripcion) VALUES('GERENTE');
 INSERT INTO ROL (Descripcion) VALUES('ADMINISTRADOR');
 INSERT INTO ROL (Descripcion) VALUES('TECNICO');
 GO

--INSERTAR USUARIO
INSERT INTO USUARIO(Documento,NombreCompleto,NombreUsuario,Correo,Clave,IdRol,Estado)
 VALUES 
 ('4213','José Luis López','jblanco','jlpezblanco170@gmail.com','bea95c9e3ffe90fd6dc4f1eee7f1e49d164f7458622ea2d69eae10c49ade6a99',1,1)
 GO

 -- INSERTAR PERMISOS PARA LOS 3 ROLES
 INSERT INTO PERMISO(IdRol,NombreMenu) VALUES
  (1,'menuusuarios'),
  (1,'menumantenedor'),
  (1,'menuregistrar'),
  (1,'menuacta'),
  (1,'menufarmacia'),
  (1,'menuautorizacion'),
  (1,'menuasiganados'),
  (1,'menureportes'),
  (1,'menuacercade')
  GO

INSERT INTO PERMISO(IdRol,NombreMenu) VALUES
  (2,'menuusuarios'),
  (2,'menumantenedor'),
  (2,'menuregistrar'),
  (2,'menuacta'),
  (2,'menufarmacia'),
  (2,'menuasiganados'),
  (2,'menureportes'),
  (2,'menuacercade')
  GO

  INSERT INTO PERMISO(IdRol,NombreMenu) VALUES
  (3,'menuasiganados')
  GO

   INSERT INTO NEGOCIO(IdNegocio,Nombre,RUC,Direccion,Logo) VALUES
  (1,'FARMACIA SABA','20202020','Managua',null),


  INSERT INTO FARMACIA(Codigo,Nombre,Telefono,Estado) VALUES
  ('FS01','Farmacia Saba Los Robles','8788878',1)
  GO

  INSERT INTO FARMACIA(Codigo,Nombre,Telefono,Estado) VALUES
  ('FS05','Farmacia Saba HOSPITAL BAUTISTA','78258875',1),
  ('FS06','Farmacia Saba PLAZA ESPAÑA # 2','78862911',1),
  ('FS07','Farmacia Saba C. SUR KM 7.5','78320127',1)
GO


INSERT INTO FARMACIA(Codigo,Nombre,Telefono,Estado) VALUES
  ('FS08','Farmacia Saba GANCHO DE CAMINO','81008074',1)
GO

INSERT INTO FARMACIA(Codigo,Nombre,Telefono,Estado) VALUES
  ('FS09','Farmacia Saba TICUANTEPE','81008075',1)
GO
