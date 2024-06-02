/*Crear Cliente*/
CREATE TABLE Clientes (
    ClienteId INT IDENTITY(1,1) PRIMARY KEY, -- Primary key with auto-increment
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    DNI NVARCHAR(20) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0 -- Boolean field to mark as deleted
);

/*
ALTER TABLE Clientes
ADD IsDeleted BIT NOT NULL DEFAULT 0;
*/

CREATE TABLE Turnos (
    TurnoId INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(ClienteId),
    CONSTRAINT UC_Turno UNIQUE (Fecha, Hora) -- Asegurar que no se dupliquen turnos en la misma fecha y hora
);

