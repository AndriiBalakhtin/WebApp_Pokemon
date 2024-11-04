CREATE TABLE [dbo].[Pokemons] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR(100)     NOT NULL,
    [Type]  VARCHAR(50)      NOT NULL,
    [Level] INT              NOT NULL,
    [Image] VARCHAR(255)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
