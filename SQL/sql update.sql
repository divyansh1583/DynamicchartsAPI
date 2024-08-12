use sDirect
--done
select * from DgOrder;
CREATE TABLE DgOrder (
    OrderID INT PRIMARY KEY,
    OrderDate DATETIME,
    ProductID INT FOREIGN KEY REFERENCES DgProduct(ProductID),
    Quantity INT,
    SourceID INT FOREIGN KEY REFERENCES DgSource(SourceID),
	CountryID INT FOREIGN KEY REFERENCE DgCountry(CountryID)
  
);
--ALTER TABLE DgOrder
--ADD CountryID INT FOREIGN KEY REFERENCES DgCountry(CountryID);
--done
select * from DgSource ;
CREATE TABLE DgSource (
    SourceID INT PRIMARY KEY,
    SourceType NVARCHAR(100)
);
--done
select * from DgOrder
CREATE TABLE DgProduct (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    CostPrice DECIMAL(18, 2),
    SellingPrice DECIMAL(18, 2)
);

--done

CREATE TABLE DgRefund (
    RefundID INT PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES DgOrder(OrderID),
	RefundDate DATETIME
);

--done

CREATE TABLE DgSession (
    SessionID INT PRIMARY KEY,
    CountryID INT FOREIGN KEY REFERENCES DgCountry(CountryID),
    StartDate DATETIME,
    EndDate DATETIME,
    SessionCreateDate DATETIME
);
--done
CREATE TABLE DgCountry (
    CountryID INT PRIMARY KEY,
    CountryName NVARCHAR(100)
);



-- Alter DgOrder table to add metadata columns
ALTER TABLE DgOrder
ADD CreateDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0;

-- Alter DgSource table to add metadata columns
ALTER TABLE DgSource
ADD CreateDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0;

-- Alter DgProduct table to add metadata columns
ALTER TABLE DgProduct
ADD CreateDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0;

-- Alter DgRefund table to add metadata columns
ALTER TABLE DgRefund
ADD CreateDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0;

-- Alter DgSession table to add metadata columns
ALTER TABLE DgSession
ADD CreateDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0;

-- Alter DgCountry table to add metadata columns
ALTER TABLE DgCountry
ADD CreateDate DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0;



--Insert Commonds

INSERT INTO DgCountry (CountryID, CountryName) VALUES 
(1, 'United States'),
(2, 'Canada'),
(3, 'United Kingdom'),
(4, 'Germany'),
(5, 'France'),
(6, 'Australia'),
(7, 'Italy'),
(8, 'Spain'),
(9, 'Netherlands'),
(10, 'Sweden');

INSERT INTO DgSource (SourceID, SourceType) VALUES 
(1, 'Direct'),
(2, 'Social'),
(3, 'Email'),
(4, 'Referral'),
(5, 'Other');


INSERT INTO DgProduct (ProductID, ProductName, CostPrice, SellingPrice) VALUES 
(1, 'Smartphone', 200.00, 299.99),
(2, 'Laptop', 800.00, 1199.99),
(3, 'Headphones', 50.00, 79.99),
(4, 'Smartwatch', 100.00, 149.99),
(5, 'Tablet', 300.00, 449.99),
(6, 'Camera', 400.00, 599.99),
(7, 'Monitor', 150.00, 229.99),
(8, 'Keyboard', 30.00, 49.99),
(9, 'Mouse', 20.00, 29.99),
(10, 'Printer', 120.00, 179.99);


DECLARE @i INT = 1;

WHILE @i <= 500
BEGIN
    INSERT INTO DgOrder (OrderID, OrderDate, ProductID, Quantity, SourceID, CountryID, CreateDate, IsDeleted)
    VALUES (
        @i,
        DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()),  -- Random past date within the last year
        (SELECT TOP 1 ProductID FROM DgProduct ORDER BY NEWID()),  -- Random ProductID
        ABS(CHECKSUM(NEWID()) % 10) + 1,  -- Random Quantity between 1 and 10
        (SELECT TOP 1 SourceID FROM DgSource ORDER BY NEWID()),  -- Random SourceID
        (SELECT TOP 1 CountryID FROM DgCountry ORDER BY NEWID()),  -- Random CountryID
        GETDATE(),  -- Creation date
        0  -- IsDeleted
    );
    SET @i = @i + 1;
END;

Select * from DgOrder


-- Add 300 entries into DgSession
DECLARE @j INT = 1;

WHILE @j <= 300
BEGIN
    INSERT INTO DgSession (SessionID, CountryID, StartDate, EndDate, SessionCreateDate, CreateDate, IsDeleted)
    VALUES (
        @j,
        (SELECT TOP 1 CountryID FROM DgCountry ORDER BY NEWID()),  -- Random CountryID
        DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()),  -- Random past start date within the last year
        DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 30), DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE())),  -- Random past end date within 30 days of the start date
        GETDATE(),  -- Session creation date
        GETDATE(),  -- Creation date
        0  -- IsDeleted
    );
    SET @j = @j + 1;
END;

DECLARE @k INT = 1;

WHILE @k <= 30
BEGIN
    INSERT INTO DgRefund (RefundID, OrderID, RefundDate, CreateDate, IsDeleted)
    VALUES (
        @k,
        (SELECT TOP 1 OrderID FROM DgOrder ORDER BY NEWID()),  -- Random OrderID
        DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()),  -- Random past date within the last year
        GETDATE(),  -- Creation date
        0  -- IsDeleted
    );
    SET @k = @k + 1;
END;

-- Drop tables in the correct order to avoid foreign key conflicts
DROP TABLE IF EXISTS DgRefund;
DROP TABLE IF EXISTS DgOrder;
DROP TABLE IF EXISTS DgSession;
DROP TABLE IF EXISTS DgProduct;
DROP TABLE IF EXISTS DgSource;
DROP TABLE IF EXISTS DC_Country;


