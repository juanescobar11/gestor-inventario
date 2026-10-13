CREATE DATABASE GestorInventarioDB;
GO
USE GestorInventarioDB;
GO
CREATE TABLE Categorias (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Activo BIT NOT NULL DEFAULT (1),
    CONSTRAINT PK_Categorias PRIMARY KEY (Id)
);
GO
CREATE TABLE Productos (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT (0),
    StockMinimo INT NOT NULL DEFAULT (5),
    CategoriaId INT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT PK_Productos PRIMARY KEY (Id),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId)
        REFERENCES Categorias (Id),
    CONSTRAINT CK_Productos_Stock CHECK (Stock >= 0)
);
GO
-- Datos iniciales (seed)
INSERT INTO Categorias (Nombre) VALUES
    (N'Electrónica'), (N'Oficina'), (N'Aseo');
GO