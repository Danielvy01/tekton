USE [master]
GO

/****** Object:  Database [EXAMDEMO]    Script Date: 19/06/2024 13:35:54 ******/
CREATE DATABASE [EXAMDEMO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EXAMDEMO', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EXAMDEMO.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EXAMDEMO_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EXAMDEMO_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO


USE [EXAMDEMO]
GO
CREATE SCHEMA [Config]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Config].[Producto](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Status] [bit] NULL,
	[Stock] [int] NULL,
	[Description] [varchar](5000) NULL,
	[Price] [decimal](18, 2) NULL,
	[Discount] [int] NULL,
	[FinalPrice]  AS (([Price]*((100)-[Discount]))/(100)),
	[EstadoRegistro] [bit] NULL,
	[UsuarioCreacion] [varchar](50) NULL,
	[FechaCreacion] [date] NULL,
	[UsuarioModificacion] [varchar](50) NULL,
	[FechaModificacion] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [Config].[Producto] ON 
GO
INSERT [Config].[Producto] ([ProductId], [Name], [Status], [Stock], [Description], [Price], [Discount], [EstadoRegistro], [UsuarioCreacion], [FechaCreacion], [UsuarioModificacion], [FechaModificacion]) VALUES (1, N'Producto 01', 1, 100, N'Producto 01 ...', CAST(100.00 AS Decimal(18, 2)), 10, 1, N'danielvy', CAST(N'2024-06-19' AS Date), N'danielvy02', CAST(N'2024-06-19' AS Date))
GO
INSERT [Config].[Producto] ([ProductId], [Name], [Status], [Stock], [Description], [Price], [Discount], [EstadoRegistro], [UsuarioCreacion], [FechaCreacion], [UsuarioModificacion], [FechaModificacion]) VALUES (2, N'Producto 02', 0, 100, N'Producto 02 ...', CAST(100.00 AS Decimal(18, 2)), 10, 1, N'danielvy', CAST(N'2024-06-19' AS Date), NULL, NULL)
GO
SET IDENTITY_INSERT [Config].[Producto] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Config].[SP_ACTUALIZAR_PRODUCTO]
(
	@ProductId int,
	@Name varchar(100),
	@Status bit,
	@Stock int,
	@Description varchar(5000),
	@Price decimal(18,2),
	@Discount int,
	@UsuarioModificacion varchar(50),
	@Success			BIT OUTPUT,
	@Message			VARCHAR(200) OUTPUT
)
AS
BEGIN 
		UPDATE Config.Producto
		SET	
			Name=@Name,
			Status=@Status,
			Stock=@Stock,
			Description=@Description,
			Price=@Price,
			Discount=@Discount,
			FechaModificacion= GETDATE(),
			UsuarioModificacion=@UsuarioModificacion
		WHERE 
		ProductId=@ProductId;
		
		SET @Success=1;
		SET @Message='Se actualizo correctamente';
		
		SELECT @Success,@Message
		
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [Config].[SP_ELIMINAR_PRODUCTO]
(
	@ProductId int,
	@UsuarioEliminacion varchar(50),
	@Success			BIT OUTPUT,
	@Message			VARCHAR(MAX) OUTPUT
)
AS
BEGIN 
	
	UPDATE Config.Producto
	SET EstadoRegistro=0,
	FechaModificacion=getdate(),
	UsuarioModificacion=@UsuarioEliminacion
	WHERE 
	ProductId=@ProductId;

	SET @Success=1;
	SET	@Message='Se elimino correctamente'
	
	select @Message, @Success
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Config].[SP_INSERT_PRODUCTO]
(
@Name varchar(100),
@Status bit,
@Stock int,
@Description varchar(5000),
@Price decimal(18,2),
@Discount int,
@UsuarioCreacion varchar(50),
@Success		BIT OUTPUT,
@Message		VARCHAR(200) OUTPUT
)
AS
BEGIN
	DECLARE @EXISTE INT=0;

	-- Verificar si el día feriado ya existe
	SELECT @EXISTE = ISNULL(COUNT(*),0) FROM Config.Producto WHERE EstadoRegistro=1 and Name=@Name;

	
	IF @EXISTE = 0
	BEGIN
		INSERT INTO Config.Producto(
				Name,
				Status,
				Stock,
				Description,
				Price,
				Discount,
				--FinalPrice,
				EstadoRegistro,
				UsuarioCreacion,
				FechaCreacion,
				UsuarioModificacion,
				FechaModificacion)
		VALUES (
			@Name,
			@Status,
			@Stock,
			@Description,
			@Price,
			@Discount,
			1,
			@UsuarioCreacion,		
			getdate(),
			null,
			null
		);
		SET @Success=1;
		SET @Message ='';	
	END
	ELSE
	BEGIN
		SET @Success=0;
		SET @Message ='El nombre del produco ya fue registrado';
	END
	SELECT @Success, @Message	
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Config].[SP_OBTENER_PRODUCTO]
@ProductId int=null
as
begin
	select 
		ProductId,
		Name,
		Status,
		Stock,
		Description,
		Price,
		Discount,
		FinalPrice,
		EstadoRegistro,
		UsuarioCreacion,
		FechaCreacion,
		UsuarioModificacion,
		FechaModificacion
	from Config.Producto
	WHERE EstadoRegistro=1 
	and (ProductId=@ProductId OR isnull(@ProductId,0)=0)
	and (isnull(@ProductId,0)>0 OR EstadoRegistro=1)
	order by ProductId asc
end
GO
