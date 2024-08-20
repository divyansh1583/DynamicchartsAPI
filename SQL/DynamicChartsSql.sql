    -- Drop existing DCP_ tables to avoid conflicts
    DROP TABLE IF EXISTS DCP_Refund;
    DROP TABLE IF EXISTS DCP_Order;
    DROP TABLE IF EXISTS DCP_Session;
    DROP TABLE IF EXISTS DCP_Product;
    DROP TABLE IF EXISTS DCP_Source;
    DROP TABLE IF EXISTS DCP_Country;

    -- Create DCP_Country table with metadata columns
    CREATE TABLE DCP_Country (
        CountryID INT IDENTITY(1,1) PRIMARY KEY ,
        CountryName NVARCHAR(100),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Source table with metadata columns
    CREATE TABLE DCP_Source (
        SourceID INT IDENTITY(1,1) PRIMARY KEY,
        SourceType NVARCHAR(100),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Product table with metadata columns
    CREATE TABLE DCP_Product (
        ProductID INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100),
        CostPrice DECIMAL(18, 2),
        SellingPrice DECIMAL(18, 2),
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Order table with metadata columns and foreign keys
    CREATE TABLE DCP_Order (
        OrderID INT IDENTITY(1,1) PRIMARY KEY,
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
        RefundID INT IDENTITY(1,1) PRIMARY KEY,
        OrderID INT FOREIGN KEY REFERENCES DCP_Order(OrderID),
        RefundDate DATETIME,
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Create DCP_Session table with metadata columns and foreign keys
    CREATE TABLE DCP_Session (
        SessionID INT IDENTITY(1,1) PRIMARY KEY         ,
        CountryID INT FOREIGN KEY REFERENCES DCP_Country(CountryID),
        StartDate DATETIME,
        EndDate DATETIME,
        SessionCreateDate DATETIME,
        CreateDate DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0
    );

    -- Insert Data into DCP_Country
    INSERT INTO DCP_Country (CountryName) VALUES 
    ('United States'),
    ('Canada'),
    ('United Kingdom'),
    ('Germany'),
    ('France'),
    ('Australia'),
    ('Italy'),
    ('Spain'),
    ('Netherlands'),
    ('Sweden');

    -- Insert Data into DCP_Source
    INSERT INTO DCP_Source (SourceType) VALUES 
    ('Direct'),
    ('Social'),
    ('Email'),
    ('Referral'),
    ('Other');

    -- Insert Data into DCP_Product
    INSERT INTO DCP_Product (ProductName, CostPrice, SellingPrice) VALUES 
    ('Smartphone', 200.00, 299.99),
    ('Laptop', 800.00, 1199.99),
    ('Headphones', 50.00, 79.99),
    ('Smartwatch', 100.00, 149.99),
    ('Tablet', 300.00, 449.99),
    ('Camera', 400.00, 599.99),
    ('Monitor', 150.00, 229.99),
    ('Keyboard', 30.00, 49.99),
    ('Mouse', 20.00, 29.99),
    ('Printer', 120.00, 179.99);

    -- Insert Data into DCP_Order
    DECLARE @i INT = 1;

    WHILE @i <= 500
    BEGIN
        INSERT INTO DCP_Order (OrderDate, ProductID, Quantity, SourceID, CountryID, CreateDate, IsDeleted)
        VALUES (
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
	    DECLARE @StartDate DATETIME = DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE());
	    INSERT INTO DCP_Session (CountryID, StartDate, EndDate, SessionCreateDate, CreateDate, IsDeleted)
	    VALUES (
	        (SELECT TOP 1 CountryID FROM DCP_Country ORDER BY NEWID()),  -- Random CountryID
	        @StartDate,  -- Random past start date within the last year
	        DATEADD(MINUTE, ABS(CHECKSUM(NEWID()) % 120), @StartDate),  -- Random end date within 2 hours of start date
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
        INSERT INTO DCP_Refund (OrderID, RefundDate, CreateDate, IsDeleted)
        VALUES (
            (SELECT TOP 1 OrderID FROM DCP_Order ORDER BY NEWID()),  -- Random OrderID
            DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 14) + 1,  -- Random future date within 1 to 14 days after OrderDate
                (SELECT OrderDate FROM DCP_Order WHERE OrderID = (SELECT TOP 1 OrderID FROM DCP_Order ORDER BY NEWID()))),
            GETDATE(),  -- Creation date
            0  -- IsDeleted
        );
        SET @k = @k + 1;
    END;

	USE sDirect
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
    ---------------
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
	    END;
	
	    DECLARE @PreviousStartDate DATE = DATEADD(DAY, DATEDIFF(DAY, @StartDate, GETDATE()), DATEADD(DAY, -1, @StartDate));
	
	    -- Calculate metrics
	    DECLARE @AvgSession DECIMAL(10,2),
	            @ConversionRate DECIMAL(5,2),
	            @AvgSessionDuration INT,
	            @PrevAvgSession DECIMAL(10,2),
	            @PrevConversionRate DECIMAL(5,2),
	            @PrevAvgSessionDuration INT;
	
	    -- Current period metrics
	    SELECT @AvgSession = CAST(COUNT(*) AS DECIMAL(10,2)) / NULLIF(COUNT(DISTINCT CAST(StartDate AS DATE)), 0),
	           @AvgSessionDuration = AVG(DATEDIFF(SECOND, StartDate, EndDate))
	    FROM DCP_Session
	    WHERE StartDate >= @StartDate;
	
	    SELECT @ConversionRate = (COUNT(*) * 100.0) / NULLIF((SELECT COUNT(*) FROM DCP_Session WHERE StartDate >= @StartDate), 0)
	    FROM DCP_Order
	    WHERE OrderDate >= @StartDate;
	
	    -- Previous period metrics
	    SELECT @PrevAvgSession = CAST(COUNT(*) AS DECIMAL(10,2)) / NULLIF(COUNT(DISTINCT CAST(StartDate AS DATE)), 0),
	           @PrevAvgSessionDuration = AVG(DATEDIFF(SECOND, StartDate, EndDate))
	    FROM DCP_Session
	    WHERE StartDate >= @PreviousStartDate AND StartDate < @StartDate;
	
	    SELECT @PrevConversionRate = (COUNT(*) * 100.0) / NULLIF((SELECT COUNT(*) FROM DCP_Session WHERE StartDate >= @PreviousStartDate AND StartDate < @StartDate), 0)
	    FROM DCP_Order
	    WHERE OrderDate >= @PreviousStartDate AND OrderDate < @StartDate;
	
	    -- Calculate percentage increases
	    DECLARE @AvgSessionIncrease DECIMAL(5,2),
	            @ConversionRateIncrease DECIMAL(5,2),
	            @AvgSessionDurationIncrease DECIMAL(5,2);
	
	    SET @AvgSessionIncrease = CASE WHEN @PrevAvgSession = 0 THEN 100 ELSE ((@AvgSession - @PrevAvgSession) * 100.0) / @PrevAvgSession END;
	    SET @ConversionRateIncrease = CASE WHEN @PrevConversionRate = 0 THEN 100 ELSE ((@ConversionRate - @PrevConversionRate) * 100.0) / @PrevConversionRate END;
	    SET @AvgSessionDurationIncrease = CASE WHEN @PrevAvgSessionDuration = 0 THEN 100 ELSE ((@AvgSessionDuration - @PrevAvgSessionDuration) * 100.0) / @PrevAvgSessionDuration END;
	
	    -- Return the results
	    SELECT 
	        ISNULL(@AvgSession, 0) AS Avg_Session,
	        ISNULL(@ConversionRate, 0) AS Conversion_Rate,
	        ISNULL(@AvgSessionDuration, 0) AS Avg_Session_Duration_Seconds,
	        ISNULL(@AvgSessionIncrease, 0) AS Avg_Session_Increase_Percentage,
	        ISNULL(@ConversionRateIncrease, 0) AS Conversion_Rate_Increase_Percentage,
	        ISNULL(@AvgSessionDurationIncrease, 0) AS Avg_Session_Duration_Increase_Percentage;
	
	    -- Monthly data for chart    
	    SELECT 
	        MONTH(s.StartDate) AS Month,
	        COUNT(*) AS Sessions,
	        YEAR(s.StartDate) AS Year
	    FROM DCP_Session s
	    WHERE s.StartDate >= @StartDate
	    GROUP BY YEAR(s.StartDate), MONTH(s.StartDate)
	    ORDER BY YEAR(s.StartDate), MONTH(s.StartDate);
	END;
	
	EXEC usp_GetAudienceMetrics '1M'
	
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
	
	EXEC usp_GetSessionsByCountries '1M'
	
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
	        CASE 
	            WHEN @TotalRevenue = 0 THEN 0
	            ELSE (@TotalRevenue - @TotalExpenses) / @TotalRevenue * 100 
	        END AS ProfitRatio
	
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

-----------------------------------------------------------------------------------------
use sDirect

CREATE OR ALTER PROCEDURE AddProduct
	@ProductID INT,
    @OrderDate DATETIME,
    @Quantity INT,
    @SourceID INT,
    @CountryID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @OrderID INT;

    BEGIN TRANSACTION;

    BEGIN TRY

        -- Insert the order
        INSERT INTO DCP_Order (OrderDate, ProductID, Quantity, SourceID, CountryID)
        VALUES (@OrderDate, @ProductID, @Quantity, @SourceID, @CountryID);

		 -- Get the new OrderID
        SET @OrderID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;

        -- Return the new IDs
        SELECT @ProductID AS NewProductID, @OrderID AS NewOrderID;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END

EXEC AddProduct
    @ProductID = 1,
    @OrderDate = '2024-08-20 14:30:00',
    @Quantity = 2,
    @SourceID = 2,
    @CountryID = 1;

CREATE TABLE ErrorLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    StackTrace NVARCHAR(MAX) NULL,
    RequestPath NVARCHAR(255) NOT NULL,
    RequestMethod NVARCHAR(10) NOT NULL
)

select * from ErrorLogs