CREATE DATABASE InventarioDB;
Go
USE InventarioDB;

CREATE TABLE Inventario(
ID INT PRIMARY KEY IDENTITY(1,1),
Producto NVARCHAR(100) NOT NULL,
Categoria NVARCHAR(100) NOT NULL,
Cantidad INT NOT NULL,
PrecioCompra DECIMAL(10,2) NOT NULL
);
                    