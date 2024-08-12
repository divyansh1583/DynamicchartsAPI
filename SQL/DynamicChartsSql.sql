    -- Drop existing DCP_ tables to avoid conflicts
    DROP TABLE IF EXISTS DCP_Refund;
    DROP TABLE IF EXISTS DCP_Order;
    DROP TABLE IF EXISTS DCP_Session;
    DROP TABLE IF EXISTS DCP_Product;
    DROP TABLE IF EXISTS DCP_Source;
    DROP TABLE IF EXISTS DCP_Country;

    -- Create DCP_Country table with metadata columns
    CREATE TABLE DCP_Country (
        CountryID INT PRIMARY KEY,
        CountryName NVARCHAR(100),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Source table with metadata columns
    CREATE TABLE DCP_Source (
        SourceID INT PRIMARY KEY,
        SourceType NVARCHAR(100),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Product table with metadata columns
    CREATE TABLE DCP_Product (
        ProductID INT PRIMARY KEY,
        ProductName NVARCHAR(100),
        CostPrice DECIMAL(18, 2),
        SellingPrice DECIMAL(18, 2),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Order table with metadata columns and foreign keys
    CREATE TABLE DCP_Order (
        OrderID INT PRIMARY KEY,
        OrderDate DATETIME,
        ProductID INT FOREIGN KEY REFERENCES DCP_Product(ProductID),
        Quantity INT,
        SourceID INT FOREIGN KEY REFERENCES DCP_Source(SourceID),
        CountryID INT FOREIGN KEY REFERENCES DCP_Country(CountryID),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Refund table with metadata columns and foreign keys
    CREATE TABLE DCP_Refund (
        RefundID INT PRIMARY KEY,
        OrderID INT FOREIGN KEY REFERENCES DCP_Order(OrderID),
        RefundDate DATETIME,
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Session table with metadata columns and foreign keys
    CREATE TABLE DCP_Session (
        SessionID INT PRIMARY KEY,
        CountryID INT FOREIGN KEY REFERENCES DCP_Country(CountryID),
        StartDate DATETIME,
        EndDate DATETIME,
        SessionCreateDate DATETIME,
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Insert Data into DCP_Country
    INSERT INTO DCP_Country (CountryID, CountryName) VALUES 
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

    -- Insert Data into DCP_Source
    INSERT INTO DCP_Source (SourceID, SourceType) VALUES 
    (1, 'Direct'),
    (2, 'Social'),
    (3, 'Email'),
    (4, 'Referral'),
    (5, 'Other');

    -- Insert Data into DCP_Product
    INSERT INTO DCP_Product (ProductID, ProductName, CostPrice, SellingPrice) VALUES 
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

    -- Insert Data into DCP_Order
    DECLARE @i INT = 1;

    WHILE @i <= 500
    BEGIN
        INSERT INTO DCP_Order (OrderID, OrderDate, ProductID, Quantity, SourceID, CountryID, CreateDate, IsDeleted)
        VALUES (
            @i,
            DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()),  -- Random past date within the last year
            (SELECT TOP 1 ProductID FROM DCP_Product ORDER BY NEWID()),  -- Random ProductID
            ABS(CHECKSUM(NEWID()) % 10) + 1,  -- Random Quantity between 1 and 10
            (SELECT TOP 1 SourceID FROM DCP_Source ORDER BY NEWID()),  -- Random SourceID
            (SELECT TOP 1 CountryID FROM DCP_Country ORDER BY NEWID()),  -- Random CountryID
            GETDATE(),  -- Creation date
            0  -- IsDeleted
        );
        SET @i = @i + 1;
    END;

    -- Insert Data into DCP_Session
    DECLARE @j INT = 1;

    WHILE @j <= 300
    BEGIN
        INSERT INTO DCP_Session (SessionID, CountryID, StartDate, EndDate, SessionCreateDate, CreateDate, IsDeleted)
        VALUES (
            @j,
            (SELECT TOP 1 CountryID FROM DCP_Country ORDER BY NEWID()),  -- Random CountryID
            DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()),  -- Random past start date within the last year
            DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 30), DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE())),  -- Random past end date within 30 days of the start date
            GETDATE(),  -- Session creation date
            GETDATE(),  -- Creation date
            0  -- IsDeleted
        );
        SET @j = @j + 1;
    END;

    -- Insert Data into DCP_Refund
    DECLARE @k INT = 1;

    WHILE @k <= 30
    BEGIN
        INSERT INTO DCP_Refund (RefundID, OrderID, RefundDate, CreateDate, IsDeleted)
        VALUES (
            @k,
            (SELECT TOP 1 OrderID FROM DCP_Order ORDER BY NEWID()),  -- Random OrderID
            DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 14) + 1,  -- Random future date within 1 to 14 days after OrderDate
                (SELECT OrderDate FROM DCP_Order WHERE OrderID = (SELECT TOP 1 OrderID FROM DCP_Order ORDER BY NEWID()))),
            GETDATE(),  -- Creation date
            0  -- IsDeleted
        );
        SET @k = @k + 1;
    END;


    -- Select all records from DCP_Country
    SELECT * FROM DCP_Country;

    -- Select all records from DCP_Source
    SELECT * FROM DCP_Source;

    -- Select all records from DCP_Product
    SELECT * FROM DCP_Product;

    -- Select all records from DCP_Order
    SELECT * FROM DCP_Order;

    -- Select all records from DCP_Refund
    SELECT * FROM DCP_Refund;

    -- Select all records from DCP_Session
    SELECT * FROM DCP_Session;

    ------------------------------------------------------------------------------------------------------------------------------------------------
    CREATE PROCEDURE usp_GetTotalOrders
        @Filter NVARCHAR(10)  -- 'Week', 'Month', 'Year'
    AS
    BEGIN
        DECLARE @StartDate DATETIME;
        DECLARE @EndDate DATETIME = GETDATE();

        IF @Filter = 'Week'
            SET @StartDate = DATEADD(WEEK, -1, @EndDate);
        ELSE IF @Filter = 'Month'
            SET @StartDate = DATEADD(MONTH, -1, @EndDate);
        ELSE IF @Filter = 'Year'
            SET @StartDate = DATEADD(YEAR, -1, @EndDate);
        ELSE
            SET @StartDate = '1900-01-01';  -- Default: no filter (all time)

        -- Calculate total orders
        SELECT COUNT(*) AS TotalOrders
        FROM DCP_Order
        WHERE OrderDate BETWEEN @StartDate AND @EndDate;
    END;

    EXEC Usp_GetTotalOrders 'week';

    ------------------------------------------------------------------------------------------------------------------------------------------------

    CREATE PROCEDURE Usp_GetTotalEarnings
        @Filter NVARCHAR(10)  -- 'Week', 'Month', 'Year'
    AS
    BEGIN
        DECLARE @StartDate DATETIME;
        DECLARE @EndDate DATETIME = GETDATE();

        IF @Filter = 'Week'
            SET @StartDate = DATEADD(WEEK, -1, @EndDate);
        ELSE IF @Filter = 'Month'
            SET @StartDate = DATEADD(MONTH, -1, @EndDate);
        ELSE IF @Filter = 'Year'
            SET @StartDate = DATEADD(YEAR, -1, @EndDate);
        ELSE
            SET @StartDate = '1900-01-01';  -- Default: no filter (all time)

        -- Calculate total earnings
        SELECT SUM(p.SellingPrice * o.Quantity) AS TotalEarnings
        FROM DCP_Order o
        JOIN DCP_Product p ON o.ProductID = p.ProductID
        WHERE o.OrderDate BETWEEN @StartDate AND @EndDate;
    END;


    EXEC Usp_GetTotalEarnings 'week';


    ------------------------------------------------------------------------------------------------------------------------------------------------

    CREATE PROCEDURE GetConversionRatio
        @Filter NVARCHAR(10)  -- 'Week', 'Month', 'Year'
    AS
    BEGIN
        DECLARE @StartDate DATETIME;
        DECLARE @EndDate DATETIME = GETDATE();

        IF @Filter = 'Week'
            SET @StartDate = DATEADD(WEEK, -1, @EndDate);
        ELSE IF @Filter = 'Month'
            SET @StartDate = DATEADD(MONTH, -1, @EndDate);
        ELSE IF @Filter = 'Year'
            SET @StartDate = DATEADD(YEAR, -1, @EndDate);
        ELSE
            SET @StartDate = '1900-01-01';  -- Default: no filter (all time)

        -- Calculate total orders
        DECLARE @TotalOrders INT;
        SELECT @TotalOrders = COUNT(*)
        FROM DCP_Order
        WHERE OrderDate BETWEEN @StartDate AND @EndDate;

        -- Calculate total refunds
        DECLARE @TotalRefunds INT;
        SELECT @TotalRefunds = COUNT(*)
        FROM DCP_Refund r
        JOIN DCP_Order o ON r.OrderID = o.OrderID
        WHERE r.RefundDate BETWEEN @StartDate AND @EndDate;

        -- Calculate conversion ratio
        DECLARE @ConversionRatio DECIMAL(5, 2);
        IF @TotalOrders > 0
            SET @ConversionRatio = (@TotalOrders - @TotalRefunds) * 100.0 / @TotalOrders;
        ELSE
            SET @ConversionRatio = 0;

        -- Output the conversion ratio
        SELECT @ConversionRatio AS ConversionRatio;
    END;

    EXEC GetConversionRatio 'year';

    ------------------------------------------------------------------------------------------------------------------------------------------------

    CREATE PROCEDURE usp_GetTotalRefund
        @Filter NVARCHAR(10)  -- 'Week', 'Month', 'Year'
    AS
    BEGIN
        -- Declare variables
        DECLARE @StartDate DATETIME;
        DECLARE @EndDate DATETIME = GETDATE();

        -- Determine start date based on filter
        IF @Filter = 'Week'
            SET @StartDate = DATEADD(WEEK, -1, @EndDate);
        ELSE IF @Filter = 'Month'
            SET @StartDate = DATEADD(MONTH, -1, @EndDate);
        ELSE IF @Filter = 'Year'
            SET @StartDate = DATEADD(YEAR, -1, @EndDate);
        ELSE
            SET @StartDate = '1900-01-01';  -- Default to all time

        -- Calculate total refunds
        SELECT COUNT(*) AS TotalRefunds
        FROM DCP_Refund r
        JOIN DCP_Order o ON r.OrderID = o.OrderID
        WHERE r.RefundDate BETWEEN @StartDate AND @EndDate;
    END;


    EXEC usp_GetTotalRefund 'Year';
    ------------------------------------------------------------------------------------------------------------------------------------------------
	------------------------------------------------------------------------
	----------------------------
	use sDirect
	-- Audiences Metrics
-- Audiences Metrics
CREATE OR ALTER PROCEDURE usp_GetAudienceMetrics
    @Filter NVARCHAR(10)
AS
BEGIN
    DECLARE @StartDate DATE = CASE 
        WHEN @Filter = '1M' THEN DATEADD(MONTH, -1, GETDATE())
        WHEN @Filter = '6M' THEN DATEADD(MONTH, -6, GETDATE())
        WHEN @Filter = '1Y' THEN DATEADD(YEAR, -1, GETDATE())
        ELSE DATEADD(YEAR, -10, GETDATE()) -- For 'ALL'
    END

    ---- Placeholder values, replace with actual calculations from your data
    --SELECT 
    --    854 AS Avg_Session,
    --    1278 AS Conversion_Rate,
    --    200 AS Avg_Session_Duration_Seconds,
    --    40 AS Avg_Session_Increase_Percentage,
    --    60 AS Conversion_Rate_Increase_Percentage,
    --    37 AS Avg_Session_Duration_Increase_Percentage

    -- Monthly data for chart	
    SELECT 
        MONTH(s.StartDate) AS Month,
        AVG(DATEDIFF(SECOND, s.StartDate, s.EndDate)) AS Sessions,
        YEAR(s.StartDate) AS Year
    FROM DCP_Session s
    WHERE s.StartDate >= @StartDate
    GROUP BY YEAR(s.StartDate), MONTH(s.StartDate)
    ORDER BY YEAR(s.StartDate), MONTH(s.StartDate)
END

EXEC usp_GetAudienceMetrics '6M'

-- Sessions by Countries
CREATE OR ALTER PROCEDURE usp_GetSessionsByCountries
    @Filter NVARCHAR(10)
AS
BEGIN
    DECLARE @StartDate DATE = CASE 
        WHEN @Filter = '1M' THEN DATEADD(MONTH, -1, GETDATE())
        WHEN @Filter = '6M' THEN DATEADD(MONTH, -6, GETDATE())
        ELSE DATEADD(YEAR, -10, GETDATE()) -- For 'ALL'
    END

    SELECT TOP 10
        c.CountryName,
        COUNT(s.SessionID) AS Sessions
    FROM DCP_Session s
    JOIN DCP_Country c ON s.CountryID = c.CountryID
    WHERE s.StartDate >= @StartDate
    GROUP BY c.CountryName
    ORDER BY Sessions DESC
END

EXEC usp_GetSessionsByCountries '6M'

-- Balance Overview
CREATE OR ALTER PROCEDURE usp_GetBalanceOverview
    @Year INT
AS
BEGIN
    DECLARE @TotalRevenue DECIMAL(18,2)
    DECLARE @TotalExpenses DECIMAL(18,2)

    SELECT 
        @TotalRevenue = SUM(o.Quantity * p.SellingPrice),
        @TotalExpenses = SUM(o.Quantity * p.CostPrice)
    FROM DCP_Order o
    JOIN DCP_Product p ON o.ProductID = p.ProductID
    WHERE YEAR(o.OrderDate) = @Year

    SELECT 
        @TotalRevenue AS Revenue,
        @TotalExpenses AS Expenses,
        (@TotalRevenue - @TotalExpenses) / @TotalRevenue * 100 AS ProfitRatio

    -- Monthly data for chart
    SELECT 
        MONTH(o.OrderDate) AS Month,
        SUM(o.Quantity * p.SellingPrice) AS Revenue,
        SUM(o.Quantity * p.CostPrice) AS Expenses
    FROM DCP_Order o
    JOIN DCP_Product p ON o.ProductID = p.ProductID
    WHERE YEAR(o.OrderDate) = @Year
    GROUP BY MONTH(o.OrderDate)
    ORDER BY MONTH(o.OrderDate)
END

EXEC usp_GetBalanceOverview 2024

-- Sales by Locations
CREATE OR ALTER PROCEDURE usp_GetSalesByLocations
AS
BEGIN
    SELECT TOP 5
        c.CountryName,
        SUM(o.Quantity * p.SellingPrice) * 100.0 / (SELECT SUM(Quantity * SellingPrice) FROM DCP_Order o JOIN DCP_Product p ON o.ProductID = p.ProductID) AS SalesPercentage
    FROM DCP_Order o
    JOIN DCP_Product p ON o.ProductID = p.ProductID
    JOIN DCP_Country c ON o.CountryID = c.CountryID
    GROUP BY c.CountryName
    ORDER BY SalesPercentage DESC
END

EXEC usp_GetSalesByLocations

-- Store Visits by Source
CREATE OR ALTER PROCEDURE usp_GetStoreVisitsBySource
AS
BEGIN
    SELECT 
        s.SourceType,
        COUNT(o.OrderID) * 100.0 / (SELECT COUNT(*) FROM DCP_Order) AS Percentage
    FROM DCP_Order o
    JOIN DCP_Source s ON o.SourceID = s.SourceID
    GROUP BY s.SourceType
END

EXEC usp_GetStoreVisitsBySource