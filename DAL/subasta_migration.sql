USE [1erP]
GO

IF COL_LENGTH('dbo.Articulo', 'Estado') IS NULL
BEGIN
    ALTER TABLE [dbo].[Articulo]
    ADD [Estado] [varchar](20) NOT NULL
        CONSTRAINT [DF_ARTICULO_ESTADO] DEFAULT ('Disponible')
END
GO

IF OBJECT_ID('dbo.Subasta', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Subasta](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [IdArticulo] [int] NOT NULL,
        [FechaInicio] [datetime] NOT NULL,
        [FechaFin] [datetime] NOT NULL,
        [PrecioInicial] [decimal](18, 2) NOT NULL,
        [PrecioFinal] [decimal](18, 2) NOT NULL,
        [IdGanador] [int] NULL,
        [Estado] [varchar](20) NOT NULL,

        CONSTRAINT [PK_SUBASTA] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_SUBASTA_ARTICULO] FOREIGN KEY([IdArticulo])
            REFERENCES [dbo].[Articulo] ([Id]),
        CONSTRAINT [FK_SUBASTA_GANADOR] FOREIGN KEY([IdGanador])
            REFERENCES [dbo].[Usuario] ([Id]),
        CONSTRAINT [CK_SUBASTA_ESTADO] CHECK ([Estado] IN ('Activa', 'Finalizada', 'Cancelada')),
        CONSTRAINT [CK_SUBASTA_FECHAS] CHECK ([FechaFin] > [FechaInicio]),
        CONSTRAINT [CK_SUBASTA_PRECIOS] CHECK ([PrecioInicial] > 0 AND [PrecioFinal] >= [PrecioInicial])
    )
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UX_SUBASTA_ARTICULO_ACTIVA'
      AND object_id = OBJECT_ID('dbo.Subasta')
)
BEGIN
    CREATE UNIQUE INDEX [UX_SUBASTA_ARTICULO_ACTIVA]
    ON [dbo].[Subasta]([IdArticulo])
    WHERE [Estado] = 'Activa'
END
GO
