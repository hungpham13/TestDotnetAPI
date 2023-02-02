-- Write your own SQL object definition here, and it'll be included in your package.
CREATE TABLE [dbo].[Account]
(
    [Id] NVARCHAR(40) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [UserName] NVARCHAR(50) NOT NULL,
    [Role] NVARCHAR(100) NOT NULL CHECK ([Role] IN ('Admin', 'Manager', 'User')),
    [PhoneNumber] NVARCHAR(15) NULL,
    [Password] NVARCHAR(50) NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1,
    [ActiveTimeStart] DATETIME,
    [ActiveTimeEnd] DATETIME,
    CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED ([Id] ASC)
);


INSERT INTO [dbo].[Account]
    ([Id], [UserName], [Name], [Role], [PhoneNumber], [Password], [Active], [ActiveTimeStart], [ActiveTimeEnd])
VALUES('2', 'hung', 'Phạm Thành Hưng', 'User', '0971169255', '12345678', 1, '1/17/2023 12:02:14 PM', '1/17/2024 12:02:14 PM');

SELECT TOP (1000) * FROM [DbThucTap].[dbo].[Account]

SELECT *
FROM [dbo].[Account]
WHERE [Id] = '2'


DELETE FROM [dbo].[Account]

UPDATE [dbo].[Account] SET [UserName] = 'hung3', [Name] = 'Phạm Thành Hưng', [Role] = 'User', [PhoneNumber] = '0971169255', [Password] = '12345678', [Active] = 0, [ActiveTimeStart] = '1/1/0001 12:00:00 AM', [ActiveTimeEnd] = '1/1/0001 12:00:00 AM' WHERE Id = '9851dbb2-0953-48d3-87a4-2876eea76c42';

SELECT *
FROM [dbo].[Account]
WHERE [Id] != '2' AND [UserName] = 'hung';

SELECT *
FROM (
  SELECT ROW_NUMBER() OVER (ORDER BY [ActiveTimeStart] DESC) AS rownumber, *
    FROM [dbo].[Account]
) AS foo
WHERE rownumber <= 10 and rownumber >= 5

SELECT COUNT(*) FROM [dbo].[Account];

DROP TABLE [dbo].[Account];
ALTER DATABASE [DbThucTap] COLLATE Vietnamese_CI_AS;

ALTER TABLE [dbo].[Account] ALTER COLUMN  Salt NVARCHAR(128)
ALTER TABLE [dbo].[Account] ALTER COLUMN  Password NVARCHAR(128)