CREATE TABLE Users (
    UserId INT PRIMARY KEY,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);

CREATE TABLE SearchHistory (
    SearchId INT PRIMARY KEY IDENTITY,
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    TickerSymbol NVARCHAR(10) NOT NULL,
    PERatio DECIMAL(18, 2),
    MarketCap DECIMAL(18, 2),
    Sector NVARCHAR(255),
    Industry NVARCHAR(255),
    Name NVARCHAR(255),
    SearchDate DATETIME DEFAULT GETDATE(),
);

INSERT INTO Users (UserId, Email, PasswordHash) VALUES (1, 'admin@email.com', 'admin');