CREATE TABLE [dbo].[Event]
(
    [Id] NVARCHAR(40) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL,
    [Performer] NVARCHAR(50) ,
    [Status] NVARCHAR(10) NOT NULL CHECK ([Status] IN ('Happening', 'Going to happen', 'Happened')),
    [Description] NVARCHAR(MAX),
    [MainImage] NVARCHAR(100) NULL,
    [CoverImage] NVARCHAR(100) NULL,
    [Time] DATETIME,
    CONSTRAINT [Pk_event] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

alter table [dbo].[Event]
    add constraint check_status
        check ([Status] in ('Live', 'Upcoming', 'Past'))
go

INSERT INTO [dbo].[Event] 
    ([Id], [Name], [Performer], [Status], [Description], [MainImage], [CoverImage], [Time]) 
    (SELECT '1', 'Event 1', 'Performer 1', 'Live', 'Description 1', 'MainImage 1', 'CoverImage 1', '2019-01-01 00:00:00')

UPDATE [dbo].[Event] SET [Name] = 'Event 1 Updated' 
                     WHERE [Id] = '1'
INSERT INTO [dbo].[Stream]
([Id], [Name], [Url], [File])
    (SELECT '1', 'Stream 1', 'Url 1', 'File 1')

CREATE TABLE [dbo].[EventStream]
(
    [StreamId] NVARCHAR(40) NOT NULL,
    [EventId] NVARCHAR(40) NOT NULL,
    CONSTRAINT [Pk_event_stream] PRIMARY KEY CLUSTERED ([StreamId] ASC, [EventId] ASC),
    CONSTRAINT [Fk_event_stream_event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]) ON DELETE  CASCADE,
    CONSTRAINT [Fk_event_stream_stream] FOREIGN KEY ([StreamId]) REFERENCES [dbo].[Stream] ([Id]) ON DELETE CASCADE
)
DROP TABLE [dbo].[EventStream]

INSERT INTO [dbo].[EventStream]
([StreamId], [EventId])
    (SELECT '1', '1')

DELETE FROM [dbo].[EventStream]
WHERE [StreamId] = '1' AND [EventId] = '1'

SELECT * FROM [dbo].[Stream]
WHERE [Id] = ANY(SELECT [StreamId] FROM [dbo].[EventStream] WHERE [EventId] = '1')

CREATE TABLE [dbo].[Attendance] (
    [Id] NVARCHAR(40) NOT NULL,
    [EventId] NVARCHAR(40) NOT NULL,
    [UserId] NVARCHAR(40) NOT NULL,
    [Status] NVARCHAR(10) NOT NULL CHECK ([Status] IN ('Initialize', 'Verified')),
    CONSTRAINT [Pk_attendance] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Fk_attendance_event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]),
    CONSTRAINT [Fk_attendance_user] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Account] ([Id])
)

ALTER TABLE [dbo].[Attendance]
ADD [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE()

SELECT * FROM [dbo].[Attendance]
WHERE [EventId] = '1';

SELECT * FROM (SELECT * FROM [dbo].[Attendance] WHERE [UserId] = '1') AS A
INNER JOIN Account U on U.Id = A.UserId 
INNER JOIN dbo.Event E on E.Id = A.EventId