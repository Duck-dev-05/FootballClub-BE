USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FootballClubDB')
BEGIN
    CREATE DATABASE FootballClubDB;
END
GO

USE FootballClubDB;
GO

-- Create Users table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Password NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) NOT NULL DEFAULT 'user',
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END

-- Create Players table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Players' and xtype='U')
BEGIN
    CREATE TABLE Players (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Position NVARCHAR(50) NOT NULL,
        ImageUrl NVARCHAR(255),
        Description NVARCHAR(MAX),
        Price DECIMAL(10,2) NOT NULL,
        IsAvailable BIT NOT NULL DEFAULT 1
    );
END

-- Create Matches table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Matches' and xtype='U')
BEGIN
    CREATE TABLE Matches (
        Id INT PRIMARY KEY IDENTITY(1,1),
        HomeTeam NVARCHAR(100) NOT NULL,
        AwayTeam NVARCHAR(100) NOT NULL,
        MatchDate DATETIME NOT NULL,
        Venue NVARCHAR(100) NOT NULL,
        Competition NVARCHAR(100),
        TicketPrice DECIMAL(10,2) NOT NULL,
        AvailableTickets INT NOT NULL,
        IsSoldOut BIT NOT NULL DEFAULT 0
    );
END

-- Create Tickets table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tickets' and xtype='U')
BEGIN
    CREATE TABLE Tickets (
        Id INT PRIMARY KEY IDENTITY(1,1),
        MatchId INT NOT NULL,
        UserId INT NOT NULL,
        SeatNumber NVARCHAR(20) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        PurchaseDate DATETIME NOT NULL DEFAULT GETDATE(),
        Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
        PaymentStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',
        PaymentId NVARCHAR(100),
        FOREIGN KEY (MatchId) REFERENCES Matches(Id),
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END

-- Create Gallery table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Gallery' and xtype='U')
BEGIN
    CREATE TABLE Gallery (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(100) NOT NULL,
        ImageUrl NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX),
        UploadDate DATETIME NOT NULL DEFAULT GETDATE(),
        Category NVARCHAR(50) NOT NULL DEFAULT 'General'
    );
END

-- Create News table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='News' and xtype='U')
BEGIN
    CREATE TABLE News (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(200) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        ImageUrl NVARCHAR(255),
        PublishedDate DATETIME NOT NULL DEFAULT GETDATE(),
        Author NVARCHAR(100) NOT NULL,
        IsPublished BIT NOT NULL DEFAULT 1
    );
END

-- Create UserAuditLogs table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserAuditLogs' and xtype='U')
BEGIN
    CREATE TABLE UserAuditLogs (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        Action NVARCHAR(50) NOT NULL,
        Details NVARCHAR(MAX),
        Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END

-- Add Calendar Events table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CalendarEvents' and xtype='U')
BEGIN
    CREATE TABLE CalendarEvents (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(200) NOT NULL,
        Start DATETIME NOT NULL,
        [End] DATETIME NOT NULL,
        [Type] NVARCHAR(50) NOT NULL DEFAULT 'match',
        Description NVARCHAR(MAX),
        Location NVARCHAR(200),
        CreatedBy INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
    );
END 