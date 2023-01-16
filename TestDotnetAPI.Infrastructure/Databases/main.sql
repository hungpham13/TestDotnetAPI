-- Write your own SQL object definition here, and it'll be included in your package.
CREATE TABLE [dbo].[Account]
(
    [Id] NVARCHAR(40) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [Role] NVARCHAR(100) NOT NULL CHECK ([Role] IN ('Admin', 'Manager', 'User')),
    [PhoneNumber] NVARCHAR(15) NULL,
    [Password] NVARCHAR(50) NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1,
    [ActiveTimeStart] DATETIME,
    [ActiveTimeEnd] DATETIME,
    CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED ([Id] ASC)
);

INSERT INTO [dbo].[Account]
    ([Id], [Name], [Role], [PhoneNumber], [Password], [Active], [ActiveTimeStart], [ActiveTimeEnd])
VALUES
    ('2', 'Hung', 'Admin', '01923', '123', 1, GETDATE(), GETDATE());


SELECT TOP (1000)
    [Id] , [Name] , [Role] , [PhoneNumber] , [Password] , [Active] , [ActiveTimeStart] , [ActiveTimeEnd]
FROM [DbThucTap].[dbo].[Account]