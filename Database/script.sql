USE [master]
GO
/****** Object:  Database [FaithConnect]    Script Date: 7/13/2024 1:56:12 AM ******/
CREATE DATABASE [FaithConnect]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FaithConnect', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\FaithConnect.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FaithConnect_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\FaithConnect_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FaithConnect].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FaithConnect] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FaithConnect] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FaithConnect] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FaithConnect] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FaithConnect] SET ARITHABORT OFF 
GO
ALTER DATABASE [FaithConnect] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FaithConnect] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FaithConnect] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FaithConnect] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FaithConnect] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FaithConnect] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FaithConnect] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FaithConnect] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FaithConnect] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FaithConnect] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FaithConnect] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FaithConnect] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FaithConnect] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FaithConnect] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FaithConnect] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FaithConnect] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FaithConnect] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FaithConnect] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FaithConnect] SET  MULTI_USER 
GO
ALTER DATABASE [FaithConnect] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FaithConnect] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FaithConnect] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FaithConnect] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FaithConnect] SET DELAYED_DURABILITY = DISABLED 
GO
USE [FaithConnect]
GO
/****** Object:  Table [dbo].[Content]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[MediaType] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [nvarchar](max) NULL,
	[Title] [nvarchar](255) NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Organizer] [nvarchar](max) NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FeedbackId] [nvarchar](max) NULL,
	[UserId] [int] NULL,
	[EventId] [int] NULL,
	[Comments] [nvarchar](max) NULL,
	[Rating] [int] NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[imageFile] [nvarchar](255) NULL,
	[coverPhoto] [nvarchar](255) NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[roleId] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [nvarchar](100) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[roleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SurveyId] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_Survey] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccount]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccount](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [nvarchar](max) NULL,
	[username] [nvarchar](100) NULL,
	[password] [nvarchar](100) NULL,
	[email] [nvarchar](100) NULL,
	[status] [int] NULL,
	[role] [int] NULL,
	[vcode] [nvarchar](100) NULL,
	[date_created] [datetime] NULL,
	[date_modified] [datetime] NULL,
 CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInformation]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [nvarchar](max) NULL,
	[last_name] [nvarchar](255) NULL,
	[first_name] [nvarchar](255) NULL,
	[phone] [nvarchar](255) NULL,
	[email] [nvarchar](255) NULL,
	[address] [nvarchar](max) NULL,
	[status] [int] NULL,
	[bio] [nvarchar](max) NULL,
	[date_created] [datetime] NULL,
 CONSTRAINT [PK_UserInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Role]    Script Date: 7/13/2024 1:56:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Role]
AS
SELECT        dbo.UserAccount.id, dbo.UserAccount.username, dbo.Role.roleName
FROM            dbo.Role INNER JOIN
                         dbo.UserAccount ON dbo.Role.roleId = dbo.UserAccount.role
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

INSERT [dbo].[Image] ([id], [userId], [imageFile], [coverPhoto]) VALUES (7, 5, N'Image_2.jpg', N'received_3096757050464994.jpeg')
INSERT [dbo].[Image] ([id], [userId], [imageFile], [coverPhoto]) VALUES (10, 7, N'234.bmp', N'heehee.png')
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([roleId], [roleName]) VALUES (1, N'User')
INSERT [dbo].[Role] ([roleId], [roleName]) VALUES (2, N'Admin')
INSERT [dbo].[Role] ([roleId], [roleName]) VALUES (3, N'Spiritual Leader')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[UserAccount] ON 

INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (1, N'123123123123123', N'marvin', N'123', N'patrickmarv09@gmail.com', 0, 1, N'123456', NULL, NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (21, N'4de3f003-44aa-4df6-8920-9b05e4b3054d', N'marben', N'123', N'Patrickmarvs@gmail.com', 1, 1, N'980089', CAST(N'2024-07-03T23:46:04.023' AS DateTime), NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (22, N'04ec81bb-06fe-4fc0-b0f1-492f5f49565c', N'Joseph', N'123', N'dagondonjoseph19@gmail.com', 1, 1, N'547263', CAST(N'2024-07-03T23:48:36.973' AS DateTime), NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (23, N'e2baeabb-0cf5-4ab2-b50e-389ca7bef7b7', N'Dagondons', N'123', N'usertest@gmail.com', 1, 1, N'144299', CAST(N'2024-07-09T15:35:13.697' AS DateTime), NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (1023, N'ff380bf2-192f-49cd-b889-42b60556c41b', N'dagodo', N'XiS9Lpn6pKLX9WZ', N'josep@gmail.com', 1, 1, N'407567', CAST(N'2024-07-13T01:37:55.807' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[UserAccount] OFF
GO
SET IDENTITY_INSERT [dbo].[UserInformation] ON 

INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (1, N'123123123123123', N'marvin', N'123', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (5, N'4de3f003-44aa-4df6-8920-9b05e4b3054d', N'Tatoy', N'Marvin', N'0950358463', N'Patrickmarvs@gmail.com', N'Liloan, Yati, 6014', 1, N'Hatdog', CAST(N'2024-07-03T23:46:04.000' AS DateTime))
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (6, N'04ec81bb-06fe-4fc0-b0f1-492f5f49565c', N'Dagondon', N'Josep', N'09934738748', N'dagondonjoseph19@gmail.com', N'Liloan, Yati, 6002', 1, N'asad', CAST(N'2024-07-03T23:48:36.973' AS DateTime))
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (7, N'e2baeabb-0cf5-4ab2-b50e-389ca7bef7b7', N'Dagondon', N'Joseph', N'09999999999', N'usertest@gmail.com', N'Liloan, Yati, 6002', 1, N'Biot', CAST(N'2024-07-09T15:35:13.000' AS DateTime))
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (1007, N'ff380bf2-192f-49cd-b889-42b60556c41b', N'Dagodo', N'Josep', N'0123456789', N'josep@gmail.com', N'Liloan, Yati, 6002', 1, NULL, CAST(N'2024-07-13T01:37:55.807' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserInformation] OFF
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Event1] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_Event1]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_UserAccount1] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserAccount] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_UserAccount1]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_UserInformation] FOREIGN KEY([userId])
REFERENCES [dbo].[UserInformation] ([id])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_UserInformation]
GO
ALTER TABLE [dbo].[UserAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserAccount_Role] FOREIGN KEY([role])
REFERENCES [dbo].[Role] ([roleId])
GO
ALTER TABLE [dbo].[UserAccount] CHECK CONSTRAINT [FK_UserAccount_Role]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Role"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "UserAccount"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Role'
GO
USE [master]
GO
ALTER DATABASE [FaithConnect] SET  READ_WRITE 
GO
