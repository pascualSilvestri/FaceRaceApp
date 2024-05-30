/*Crear Cliente*/
CREATE TABLE Clientes (
    ClienteId INT IDENTITY(1,1) PRIMARY KEY, -- Primary key with auto-increment
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    DNI NVARCHAR(20) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Correo NVARCHAR(100) NOT NULL
);



