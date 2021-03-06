USE [master]
GO
/****** Object:  Database [Portal]    Script Date: 10/6/2020 8:14:25 AM ******/
CREATE DATABASE [Portal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Portal', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Portal.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Portal_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Portal_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Portal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Portal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Portal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Portal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Portal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Portal] SET ARITHABORT OFF 
GO
ALTER DATABASE [Portal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Portal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Portal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Portal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Portal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Portal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Portal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Portal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Portal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Portal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Portal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Portal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Portal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Portal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Portal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Portal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Portal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Portal] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Portal] SET  MULTI_USER 
GO
ALTER DATABASE [Portal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Portal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Portal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Portal] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Portal] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Portal] SET QUERY_STORE = OFF
GO
USE [Portal]
GO
/****** Object:  User [portal]    Script Date: 10/6/2020 8:14:25 AM ******/
CREATE USER [portal] FOR LOGIN [portal] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[ArticleFiles]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleFiles](
	[ID] [uniqueidentifier] NOT NULL,
	[ArticleID] [uniqueidentifier] NOT NULL,
	[FileID] [uniqueidentifier] NOT NULL,
	[Caption] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[ID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[AuthorID] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Published] [datetime] NULL,
	[Title] [varchar](255) NOT NULL,
	[Subtitle] [varchar](255) NULL,
	[Body] [text] NOT NULL,
	[Excerpt] [text] NULL,
	[FeaturedImage] [varchar](255) NULL,
	[Slug] [varchar](255) NULL,
	[IsFeatured] [bit] NOT NULL,
	[IsArchived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Articles_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArticleTags]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleTags](
	[ID] [uniqueidentifier] NOT NULL,
	[ArticleID] [uniqueidentifier] NOT NULL,
	[TagID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ArticleTags_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[DisplayName] [varchar](255) NOT NULL,
	[IsVisible] [tinyint] NOT NULL,
 CONSTRAINT [PK_Authors_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [uniqueidentifier] NOT NULL,
	[DisplayName] [varchar](100) NOT NULL,
	[Slug] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Categories_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[ID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[UploadName] [varchar](200) NOT NULL,
	[UploadType] [varchar](50) NOT NULL,
	[Uploaded] [datetime] NOT NULL,
	[UploadContents] [varbinary](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 10/6/2020 8:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[ID] [uniqueidentifier] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[DisplayName] [varchar](150) NOT NULL,
 CONSTRAINT [PK_Tags_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT (NULL) FOR [Published]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT (NULL) FOR [Subtitle]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT ((0)) FOR [IsFeatured]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT ((0)) FOR [IsArchived]
GO
ALTER TABLE [dbo].[Articles] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ArticleTags] ADD  CONSTRAINT [DF_ArticleTags_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Authors] ADD  CONSTRAINT [DF_Authors_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Files] ADD  DEFAULT (getdate()) FOR [Uploaded]
GO
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [DF_Tags_ID]  DEFAULT (newid()) FOR [ID]
GO
USE [master]
GO
ALTER DATABASE [Portal] SET  READ_WRITE 
GO
