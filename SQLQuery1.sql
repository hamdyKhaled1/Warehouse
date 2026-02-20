CREATE DATABASE WarehouseDB;
GO
USE WarehouseDB;

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE TABLE Warehouses (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL,
    Location NVARCHAR(300)
);

CREATE TABLE Stocks (
    Id INT PRIMARY KEY IDENTITY,
    ProductId INT NOT NULL,
    WarehouseId INT NOT NULL,
    Quantity INT NOT NULL,
    RowVersion ROWVERSION,

    CONSTRAINT FK_Stock_Product
        FOREIGN KEY (ProductId) REFERENCES Products(Id),

    CONSTRAINT FK_Stock_Warehouse
        FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id),

    CONSTRAINT UQ_Product_Warehouse
        UNIQUE(ProductId, WarehouseId)
);

CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    OrderNumber NVARCHAR(50) NOT NULL,
    TotalAmount DECIMAL(18,2),
    Status NVARCHAR(50) DEFAULT 'Pending',
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,

    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

ALTER TABLE Products
ADD IsDeleted BIT DEFAULT 0;

ALTER TABLE Orders
ADD IsDeleted BIT DEFAULT 0;

ALTER TABLE Products
ALTER COLUMN IsActive BIT NOT NULL;


USE WarehouseDB;
GO

-- ================================
-- Products
-- ================================
INSERT INTO Products (Name, Description, Price, IsActive)
VALUES
('Laptop Dell XPS 13', 'Ultrabook Laptop 13 inch', 1200, 1),
('iPhone 15', 'Latest Apple iPhone 15', 1500, 1),
('Logitech Mouse', 'Wireless Mouse', 50, 1),
('Gaming Chair', 'Ergonomic Chair', 300, 1),
('HD Monitor 27"', '27 inch Full HD Monitor', 200, 1);

-- ================================
--  Warehouses
-- ================================
INSERT INTO Warehouses (Name, Location)
VALUES
('Central Warehouse', 'Riyadh, Saudi Arabia'),
('Eastern Warehouse', 'Dammam, Saudi Arabia'),
('Western Warehouse', 'Jeddah, Saudi Arabia');

-- ================================
--  Stocks
-- ================================
INSERT INTO Stocks (ProductId, WarehouseId, Quantity)
VALUES
(1, 1, 10),
(1, 2, 5),
(2, 1, 20),
(2, 3, 10),
(3, 1, 50),
(3, 2, 30),
(4, 2, 15),
(5, 3, 25);

-- ================================
-- Orders
-- ================================
INSERT INTO Orders (OrderNumber, TotalAmount, Status)
VALUES
('ORD-1001', 1250, 'Completed'),
('ORD-1002', 2000, 'Pending');

-- ================================
--  OrderItems
-- ================================
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES
(1, 1, 1, 1200),  -- Laptop
(1, 3, 1, 50),    -- Mouse
(2, 2, 1, 1500),  -- iPhone
(2, 5, 2, 200);   -- HD Monitor
