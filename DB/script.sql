USE [master]
GO
/****** Object:  Database [GP]    Script Date: 1/13/2016 4:45:07 PM ******/
CREATE DATABASE [GP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GP', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\GP.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GP_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\GP_log.ldf' , SIZE = 199296KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [GP] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GP] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GP] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GP] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GP] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GP] SET ARITHABORT OFF 
GO
ALTER DATABASE [GP] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GP] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GP] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GP] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GP] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GP] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GP] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GP] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GP] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GP] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GP] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GP] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GP] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GP] SET  MULTI_USER 
GO
ALTER DATABASE [GP] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GP] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GP] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [GP] SET DELAYED_DURABILITY = DISABLED 
GO
USE [GP]
GO
/****** Object:  Table [dbo].[confirmIPD]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[confirmIPD](
	[reqID] [nchar](20) NOT NULL,
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[expiryDate] [date] NOT NULL,
	[quantity] [int] NULL,
 CONSTRAINT [PK_confirmIPD] PRIMARY KEY CLUSTERED 
(
	[reqID] ASC,
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[confirmStore]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[confirmStore](
	[reqID] [nchar](20) NOT NULL,
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[expiryDate] [date] NOT NULL,
	[quantity] [int] NULL,
 CONSTRAINT [PK_confirmStore] PRIMARY KEY CLUSTERED 
(
	[reqID] ASC,
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ipd]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ipd](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL CONSTRAINT [DF_ipd_quantity]  DEFAULT ((0)),
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL CONSTRAINT [DF_ipd_reduce]  DEFAULT ((0)),
 CONSTRAINT [PK_ipd] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[medicine]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[medicine](
	[phID] [int] NOT NULL,
	[pid] [int] NOT NULL,
	[wdNo] [int] NOT NULL,
	[genName] [nchar](30) NULL,
	[enabled] [bit] NULL,
	[dosage] [int] NOT NULL,
	[frequency] [int] NULL,
	[period] [int] NULL,
	[date] [date] NULL,
 CONSTRAINT [PK_medicine] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[pid] ASC,
	[wdNo] ASC,
	[dosage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[opd]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[opd](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_opd] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[patients]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patients](
	[pid] [int] NOT NULL,
	[pNicNo] [nchar](10) NOT NULL,
	[pBhtNo] [int] NOT NULL,
	[year] [int] NOT NULL,
	[wdNo] [int] NOT NULL,
	[pEnableCheck] [bit] NOT NULL,
	[pRegDate] [date] NOT NULL,
	[pFirstName] [nchar](20) NOT NULL,
	[pMiddleName] [nchar](20) NOT NULL,
	[pLastName] [nchar](20) NOT NULL,
	[pDob] [date] NOT NULL,
	[pContactNo] [nchar](10) NOT NULL,
	[pStatus] [nchar](10) NOT NULL,
	[pAddr] [nchar](50) NULL,
	[gName] [nchar](40) NOT NULL,
	[gRelation] [nchar](10) NOT NULL,
	[gContactNo] [nchar](10) NOT NULL,
	[pHistory] [text] NULL,
	[pPrescription] [text] NULL,
 CONSTRAINT [PK_patients] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[pharmaceuticals]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pharmaceuticals](
	[phID] [int] NOT NULL,
	[id] [int] NOT NULL,
	[type] [nvarchar](10) NOT NULL,
	[category] [nchar](18) NOT NULL,
	[countable] [bit] NOT NULL,
	[genName] [nchar](40) NOT NULL,
 CONSTRAINT [PK_pharmaceuticals] PRIMARY KEY CLUSTERED 
(
	[phID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reqFromIPD]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reqFromIPD](
	[reqID] [nvarchar](150) NOT NULL,
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[enable] [bit] NOT NULL,
 CONSTRAINT [PK_reqFromIPD_1] PRIMARY KEY CLUSTERED 
(
	[reqID] ASC,
	[phID] ASC,
	[dosage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reqFromStore]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reqFromStore](
	[reqID] [nvarchar](150) NOT NULL,
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[enable] [bit] NULL,
 CONSTRAINT [PK_reqFromStore] PRIMARY KEY CLUSTERED 
(
	[reqID] ASC,
	[phID] ASC,
	[dosage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[store]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[store](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NOT NULL CONSTRAINT [DF_store_quantity]  DEFAULT ((0)),
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NOT NULL CONSTRAINT [DF_store_reduce]  DEFAULT ((0)),
 CONSTRAINT [PK_store] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[uID] [int] NOT NULL,
	[userID] [int] NOT NULL,
	[username] [nchar](15) NOT NULL,
	[password] [nvarchar](30) NOT NULL,
	[uEnableCheck] [bit] NOT NULL,
	[uName] [nchar](50) NOT NULL,
	[uEmail] [nchar](30) NULL,
	[uNicNo] [nchar](10) NOT NULL,
	[uContactNo] [nchar](10) NOT NULL,
	[uDep] [int] NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[uID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd01]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd01](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL CONSTRAINT [DF_wd01_quantity]  DEFAULT ((0)),
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL CONSTRAINT [DF_wd01_reduce]  DEFAULT ((0)),
 CONSTRAINT [PK_wd01] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd02]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd02](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd02] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd03]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd03](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd03] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd04]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd04](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd04] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd05]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd05](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd05] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd06]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd06](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd06] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd07]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd07](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd07] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd08]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd08](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd08] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd09]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd09](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd09] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd10]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd10](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[expiryDate] [date] NOT NULL,
	[quantity] [int] NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd10] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd11]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd11](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd11] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wd12]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wd12](
	[phID] [int] NOT NULL,
	[dosage] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiryDate] [date] NOT NULL,
	[reduce] [int] NULL,
 CONSTRAINT [PK_wd12] PRIMARY KEY CLUSTERED 
(
	[phID] ASC,
	[dosage] ASC,
	[expiryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ipdView]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ipdView]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, ipd.dosage, ipd.quantity, ipd.expiryDate, ipd.reduce
FROM pharmaceuticals, ipd  
WHERE pharmaceuticals.phID=ipd.phID;

GO
/****** Object:  View [dbo].[opdView]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[opdView]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, opd.dosage, opd.quantity, opd.expiryDate
FROM pharmaceuticals, opd  
WHERE pharmaceuticals.phID=opd.phID;

GO
/****** Object:  View [dbo].[storeView]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[storeView]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, store.dosage, store.quantity, store.expiryDate, store.reduce
FROM pharmaceuticals, store  
WHERE pharmaceuticals.phID=store.phID;

GO
/****** Object:  View [dbo].[wd01View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd01View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd01.dosage, wd01.quantity, wd01.expiryDate
FROM pharmaceuticals, wd01  
WHERE pharmaceuticals.phID=wd01.phID;

GO
/****** Object:  View [dbo].[wd02View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd02View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd02.dosage, wd02.quantity, wd02.expiryDate
FROM pharmaceuticals, wd02  
WHERE pharmaceuticals.phID=wd02.phID;

GO
/****** Object:  View [dbo].[wd03View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd03View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd03.dosage, wd03.quantity, wd03.expiryDate
FROM pharmaceuticals, wd03  
WHERE pharmaceuticals.phID=wd03.phID;


GO
/****** Object:  View [dbo].[wd04View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd04View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd04.dosage, wd04.quantity, wd04.expiryDate
FROM pharmaceuticals, wd04  
WHERE pharmaceuticals.phID=wd04.phID;

GO
/****** Object:  View [dbo].[wd05View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd05View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd05.dosage, wd05.quantity, wd05.expiryDate
FROM pharmaceuticals, wd05  
WHERE pharmaceuticals.phID=wd05.phID;


GO
/****** Object:  View [dbo].[wd06View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd06View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd06.dosage, wd06.quantity, wd06.expiryDate
FROM pharmaceuticals, wd06  
WHERE pharmaceuticals.phID=wd06.phID;


GO
/****** Object:  View [dbo].[wd07View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd07View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd07.dosage, wd07.quantity, wd07.expiryDate
FROM pharmaceuticals, wd07  
WHERE pharmaceuticals.phID=wd07.phID;


GO
/****** Object:  View [dbo].[wd08View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd08View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd08.dosage, wd08.quantity, wd08.expiryDate
FROM pharmaceuticals, wd08  
WHERE pharmaceuticals.phID=wd08.phID;


GO
/****** Object:  View [dbo].[wd09View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd09View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd09.dosage, wd09.quantity, wd09.expiryDate
FROM pharmaceuticals, wd09  
WHERE pharmaceuticals.phID=wd09.phID;


GO
/****** Object:  View [dbo].[wd10View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd10View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd10.dosage, wd10.quantity, wd10.expiryDate
FROM pharmaceuticals, wd10  
WHERE pharmaceuticals.phID=wd10.phID;


GO
/****** Object:  View [dbo].[wd11View]    Script Date: 1/13/2016 4:45:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd11View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd11.dosage, wd11.quantity, wd11.expiryDate
FROM pharmaceuticals, wd11  
WHERE pharmaceuticals.phID=wd11.phID;


GO
/****** Object:  View [dbo].[wd12View]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wd12View]
AS
SELECT pharmaceuticals.genName, pharmaceuticals.type, wd12.dosage, wd12.quantity, wd12.expiryDate
FROM pharmaceuticals, wd12  
WHERE pharmaceuticals.phID=wd12.phID;


GO
ALTER TABLE [dbo].[opd] ADD  CONSTRAINT [DF_opd_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[opd] ADD  CONSTRAINT [DF_opd_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd02] ADD  CONSTRAINT [DF_wd02_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd02] ADD  CONSTRAINT [DF_wd02_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd03] ADD  CONSTRAINT [DF_wd03_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd03] ADD  CONSTRAINT [DF_wd03_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd04] ADD  CONSTRAINT [DF_wd04_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd04] ADD  CONSTRAINT [DF_wd04_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd05] ADD  CONSTRAINT [DF_wd05_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd05] ADD  CONSTRAINT [DF_wd05_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd06] ADD  CONSTRAINT [DF_wd06_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd06] ADD  CONSTRAINT [DF_wd06_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd07] ADD  CONSTRAINT [DF_wd07_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd07] ADD  CONSTRAINT [DF_wd07_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd08] ADD  CONSTRAINT [DF_wd08_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd08] ADD  CONSTRAINT [DF_wd08_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd09] ADD  CONSTRAINT [DF_wd09_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd09] ADD  CONSTRAINT [DF_wd09_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd10] ADD  CONSTRAINT [DF_wd10_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd10] ADD  CONSTRAINT [DF_wd10_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd11] ADD  CONSTRAINT [DF_wd11_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd11] ADD  CONSTRAINT [DF_wd11_reduce]  DEFAULT ((0)) FOR [reduce]
GO
ALTER TABLE [dbo].[wd12] ADD  CONSTRAINT [DF_wd12_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[wd12] ADD  CONSTRAINT [DF_wd12_reduce]  DEFAULT ((0)) FOR [reduce]
GO
/****** Object:  StoredProcedure [dbo].[addToIPD]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToIPD]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmStore WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmStore WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM ipd WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE ipd SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE store SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO ipd (phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q)
				UPDATE store SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmStore WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmStore WHERE reqID=@reqID;
	DELETE FROM reqFromStore WHERE reqID=@res;
END

GO
/****** Object:  StoredProcedure [dbo].[addToWD01]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD01]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd01 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd01 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd01(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD02]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD02]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd02 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd02 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd02 (phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q)
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD03]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addToWD03]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd03 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd03 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd03 (phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD04]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD04]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd04 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd04 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd04(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD05]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD05]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd05 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd05 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd05(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END

GO
/****** Object:  StoredProcedure [dbo].[addToWD06]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD06]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd06 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd06 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd06(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD07]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD07]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd07 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd07 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd07(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD08]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD08]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd08 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd08 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd08(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END

GO
/****** Object:  StoredProcedure [dbo].[addToWD09]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD09]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd09 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd09 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd09(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END

GO
/****** Object:  StoredProcedure [dbo].[addToWD10]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD10]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd10 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd10 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd10(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD11]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD11]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd11 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd11 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd11(phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END
GO
/****** Object:  StoredProcedure [dbo].[addToWD12]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addToWD12]
	@reqID varchar(20),
	@res varchar(20)
AS

DECLARE 
	@p int= 0,
	@d int = 0,
	@e date,
	@q int = 0

BEGIN
	WHILE EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM confirmIPD WHERE reqID=@reqID)
	BEGIN
		SELECT TOP 1 @p=phID, @d=dosage, @e=expiryDate, @q=quantity FROM confirmIPD WHERE reqID=@reqID;
		IF EXISTS (SELECT TOP 1 phID, dosage, expiryDate, quantity FROM wd12 WHERE phID=@p AND dosage=@d AND expiryDate=@e)
			BEGIN
				UPDATE wd12 SET quantity=quantity+@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		ELSE
			BEGIN
				INSERT INTO wd12 (phID, dosage, expiryDate, quantity) VALUES (@p, @d, @e, @q);
				UPDATE ipd SET quantity=quantity-@q, reduce=reduce-@q WHERE phID=@p AND dosage=@d AND expiryDate=@e;
			END
		DELETE FROM confirmIPD WHERE reqID=@reqID AND  phID=@p AND dosage=@d AND expiryDate=@e;
	END 
	DELETE FROM confirmIPD WHERE reqID=@res;
	DELETE FROM reqFromIPD WHERE reqID=@res;
END

GO
/****** Object:  StoredProcedure [dbo].[confirmProcIPD]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[confirmProcIPD]
	@reqID varchar(20),
	@phID int,
	@dosage int,
	@quantity int
AS

DECLARE @r int=0,
		@expDate date,
		@req varchar(20) = @reqID + ' CON'

WHILE (@quantity > 0)
BEGIN
	IF EXISTS (SELECT TOP 1 (ipd.quantity- ISNULL (ipd.reduce, 0)), ipd.expiryDate FROM ipd WHERE ipd.phID=@phID AND ipd.dosage=@dosage AND ((ipd.quantity - ISNULL(ipd.reduce, 0)) > 0) ORDER BY ipd.expiryDate)
	BEGIN
		SELECT TOP 1 @r=(ipd.quantity - ISNULL(ipd.reduce, 0)), @expDate=ipd.expiryDate FROM ipd WHERE ipd.phID=@phID AND ipd.dosage=@dosage AND ((ipd.quantity - ISNULL(ipd.reduce, 0)) > 0) ORDER BY ipd.expiryDate
		IF (@r >= @quantity)
			BEGIN
				UPDATE ipd SET reduce=reduce + @quantity WHERE ipd.phID=@phID AND ipd.dosage=@dosage AND ipd.expiryDate=@expDate
				INSERT INTO confirmIPD (reqID, phID, dosage, expiryDate, quantity) VALUES(@req, @phID, @dosage, @expDate, @quantity)
				SET @quantity = 0
			END

		ELSE
			BEGIN
				UPDATE ipd SET reduce=reduce + @r WHERE ipd.phID=@phID AND ipd.dosage=@dosage AND ipd.expiryDate=@expDate
				INSERT INTO confirmIPD (reqID, phID, dosage, expiryDate, quantity) VALUES (@req, @phID, @dosage, @expDate, @r)
				SET @quantity=@quantity-@r
			END
	END

	ELSE
		BEGIN
			RETURN @quantity
		END 
END
GO
/****** Object:  StoredProcedure [dbo].[confirmProcStore]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[confirmProcStore]
	@reqID varchar(20),
	@phID int,
	@dosage int,
	@quantity int
AS

DECLARE @r int=0,
		@expDate date,
		@req varchar(20) = @reqID + ' CON'
WHILE (@quantity > 0)
BEGIN
	IF EXISTS (SELECT TOP 1 (store.quantity- ISNULL (store.reduce, 0)), store.expiryDate FROM store WHERE store.phID=@phID AND store.dosage=@dosage AND ((store.quantity - ISNULL(store.reduce, 0)) > 0) ORDER BY store.expiryDate)
	BEGIN
		SELECT TOP 1 @r=(store.quantity- ISNULL(store.reduce, 0)), @expDate=store.expiryDate FROM store WHERE store.phID=@phID AND store.dosage=@dosage AND ((store.quantity - ISNULL(store.reduce, 0)) > 0) ORDER BY store.expiryDate
		IF (@r >= @quantity)
			BEGIN
				UPDATE store SET reduce=reduce + @quantity WHERE store.phID=@phID AND store.dosage=@dosage AND store.expiryDate=@expDate
				INSERT INTO confirmStore (reqID, phID, dosage, expiryDate, quantity) VALUES(@req, @phID, @dosage, @expDate, @quantity)
				SET @quantity = 0
			END

		ELSE
			BEGIN
				UPDATE store SET reduce=reduce + @r WHERE store.phID=@phID AND store.dosage=@dosage AND store.expiryDate=@expDate
				INSERT INTO confirmStore (reqID, phID, dosage, expiryDate, quantity) VALUES (@req, @phID, @dosage, @expDate, @r)
				SET @quantity=@quantity-@r
			END
	END

	ELSE
		BEGIN
			RETURN @quantity
		END 
END
GO
/****** Object:  StoredProcedure [dbo].[SqlQueryNotificationStoredProcedure-6bbd86df-9e92-478a-9fe7-4b97283e76db]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SqlQueryNotificationStoredProcedure-6bbd86df-9e92-478a-9fe7-4b97283e76db] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-6bbd86df-9e92-478a-9fe7-4b97283e76db]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-6bbd86df-9e92-478a-9fe7-4b97283e76db] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-6bbd86df-9e92-478a-9fe7-4b97283e76db') > 0)   DROP SERVICE [SqlQueryNotificationService-6bbd86df-9e92-478a-9fe7-4b97283e76db]; if (OBJECT_ID('SqlQueryNotificationService-6bbd86df-9e92-478a-9fe7-4b97283e76db', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-6bbd86df-9e92-478a-9fe7-4b97283e76db]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-6bbd86df-9e92-478a-9fe7-4b97283e76db]; END COMMIT TRANSACTION; END

GO
/****** Object:  StoredProcedure [dbo].[SqlQueryNotificationStoredProcedure-7ba3fb46-297d-4e56-bf35-f657e6d0381b]    Script Date: 1/13/2016 4:45:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SqlQueryNotificationStoredProcedure-7ba3fb46-297d-4e56-bf35-f657e6d0381b] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-7ba3fb46-297d-4e56-bf35-f657e6d0381b]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-7ba3fb46-297d-4e56-bf35-f657e6d0381b] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-7ba3fb46-297d-4e56-bf35-f657e6d0381b') > 0)   DROP SERVICE [SqlQueryNotificationService-7ba3fb46-297d-4e56-bf35-f657e6d0381b]; if (OBJECT_ID('SqlQueryNotificationService-7ba3fb46-297d-4e56-bf35-f657e6d0381b', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-7ba3fb46-297d-4e56-bf35-f657e6d0381b]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-7ba3fb46-297d-4e56-bf35-f657e6d0381b]; END COMMIT TRANSACTION; END
GO
USE [master]
GO
ALTER DATABASE [GP] SET  READ_WRITE 
GO
