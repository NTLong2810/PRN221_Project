USE [master]
GO
/****** Object:  Database [SupermarketDB]    Script Date: 10/10/2023 8:55:06 PM ******/
CREATE DATABASE [SupermarketDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SupermarketDB', FILENAME = N'D:\SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SupermarketDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SupermarketDB_log', FILENAME = N'D:\SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SupermarketDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SupermarketDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SupermarketDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SupermarketDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SupermarketDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SupermarketDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SupermarketDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SupermarketDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [SupermarketDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SupermarketDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SupermarketDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SupermarketDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SupermarketDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SupermarketDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SupermarketDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SupermarketDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SupermarketDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SupermarketDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SupermarketDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SupermarketDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SupermarketDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SupermarketDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SupermarketDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SupermarketDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SupermarketDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SupermarketDB] SET RECOVERY FULL 
GO
ALTER DATABASE [SupermarketDB] SET  MULTI_USER 
GO
ALTER DATABASE [SupermarketDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SupermarketDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SupermarketDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SupermarketDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SupermarketDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SupermarketDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SupermarketDB', N'ON'
GO
ALTER DATABASE [SupermarketDB] SET QUERY_STORE = OFF
GO
USE [SupermarketDB]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[categoryId] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[QuantityInStock] [int] NOT NULL,
	[ExpirationDate] [date] NULL,
	[categoryId] [int] NULL,
 CONSTRAINT [PK__Products__B40CC6ED4E963F9A] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrderItems]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrderItems](
	[OrderItemID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrders]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[SupplierID] [int] NULL,
	[OrderDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesOrderItems]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesOrderItems](
	[OrderItemID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesOrders]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesOrders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 10/10/2023 8:55:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [nvarchar](255) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[Phone] [nvarchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([categoryId], [categoryName]) VALUES (1, N'An vat')
INSERT [dbo].[Category] ([categoryId], [categoryName]) VALUES (2, N'Sua')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [ProductName], [UnitPrice], [QuantityInStock], [ExpirationDate], [categoryId]) VALUES (1, N'Bim Bim Ostar', CAST(15000.00 AS Decimal(18, 2)), 50, CAST(N'2023-12-31' AS Date), 1)
INSERT [dbo].[Products] ([ProductID], [ProductName], [UnitPrice], [QuantityInStock], [ExpirationDate], [categoryId]) VALUES (2, N'Vinamilk Socola', CAST(10000.00 AS Decimal(18, 2)), 60, CAST(N'2023-11-28' AS Date), 2)
INSERT [dbo].[Products] ([ProductID], [ProductName], [UnitPrice], [QuantityInStock], [ExpirationDate], [categoryId]) VALUES (3, N'Sua Milo', CAST(12000.00 AS Decimal(18, 2)), 100, CAST(N'2024-01-20' AS Date), 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[PurchaseOrderItems] ON 

INSERT [dbo].[PurchaseOrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [UnitPrice]) VALUES (1, 1, 1, 50, CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[PurchaseOrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [UnitPrice]) VALUES (2, 2, 2, 60, CAST(7000.00 AS Decimal(18, 2)))
INSERT [dbo].[PurchaseOrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [UnitPrice]) VALUES (3, 3, 3, 100, CAST(9000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[PurchaseOrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON 

INSERT [dbo].[PurchaseOrders] ([OrderID], [SupplierID], [OrderDate]) VALUES (1, 1, CAST(N'2023-10-09' AS Date))
INSERT [dbo].[PurchaseOrders] ([OrderID], [SupplierID], [OrderDate]) VALUES (2, 2, CAST(N'2023-10-07' AS Date))
INSERT [dbo].[PurchaseOrders] ([OrderID], [SupplierID], [OrderDate]) VALUES (3, 3, CAST(N'2023-10-08' AS Date))
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([SupplierID], [SupplierName], [Address], [Phone]) VALUES (1, N'Ostar', N'Quang Ninh', N'0932313134')
INSERT [dbo].[Suppliers] ([SupplierID], [SupplierName], [Address], [Phone]) VALUES (2, N'Vinamilk', N'Binh Duong', N'0323131313')
INSERT [dbo].[Suppliers] ([SupplierID], [SupplierName], [Address], [Phone]) VALUES (3, N'Milo', N'Ha Noi', N'0923131231')
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Category] FOREIGN KEY([categoryId])
REFERENCES [dbo].[Category] ([categoryId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Category]
GO
ALTER TABLE [dbo].[PurchaseOrderItems]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[PurchaseOrders] ([OrderID])
GO
ALTER TABLE [dbo].[PurchaseOrderItems]  WITH CHECK ADD  CONSTRAINT [FK__PurchaseO__Produ__2C3393D0] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[PurchaseOrderItems] CHECK CONSTRAINT [FK__PurchaseO__Produ__2C3393D0]
GO
ALTER TABLE [dbo].[PurchaseOrders]  WITH CHECK ADD FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([SupplierID])
GO
ALTER TABLE [dbo].[SalesOrderItems]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[SalesOrders] ([OrderID])
GO
ALTER TABLE [dbo].[SalesOrderItems]  WITH CHECK ADD  CONSTRAINT [FK__SalesOrde__Produ__31EC6D26] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[SalesOrderItems] CHECK CONSTRAINT [FK__SalesOrde__Produ__31EC6D26]
GO
USE [master]
GO
ALTER DATABASE [SupermarketDB] SET  READ_WRITE 
GO
