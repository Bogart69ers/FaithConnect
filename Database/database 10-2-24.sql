USE [FaithConnect]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/2/2024 5:19:43 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccount]    Script Date: 10/2/2024 5:19:43 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Role]    Script Date: 10/2/2024 5:19:43 AM ******/
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
/****** Object:  Table [dbo].[Content]    Script Date: 10/2/2024 5:19:43 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[eventId] [nvarchar](max) NULL,
	[groupId] [int] NULL,
	[event_date] [datetime] NULL,
	[date_created] [datetime] NULL,
	[status] [int] NULL,
	[title] [nvarchar](255) NULL,
	[description] [nvarchar](max) NULL,
	[createdBy] [int] NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 10/2/2024 5:19:43 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Forum]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forum](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[forumId] [nvarchar](max) NULL,
	[title] [nvarchar](255) NULL,
	[content] [nvarchar](max) NULL,
	[groupId] [int] NULL,
	[createdBy] [int] NULL,
	[date_created] [datetime] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupMembership]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMembership](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[groupId] [int] NULL,
	[status] [int] NULL,
	[dateJoined] [datetime] NULL,
 CONSTRAINT [PK_GroupMembership] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[groupId] [nvarchar](max) NULL,
	[groupName] [nvarchar](100) NULL,
	[description] [nvarchar](max) NULL,
	[groupAdmin] [int] NULL,
	[date_created] [datetime] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 10/2/2024 5:19:43 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Media]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[mediaId] [nvarchar](max) NULL,
	[postId] [int] NULL,
	[eventId] [int] NULL,
	[forumId] [int] NULL,
	[fileName] [nvarchar](max) NULL,
	[filePath] [nvarchar](max) NULL,
	[mediaType] [nvarchar](100) NULL,
	[date_created] [datetime] NULL,
 CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[postId] [nvarchar](max) NULL,
	[title] [nvarchar](255) NULL,
	[content] [nvarchar](max) NULL,
	[groupId] [int] NULL,
	[createdBy] [int] NULL,
	[date_created] [datetime] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey]    Script Date: 10/2/2024 5:19:43 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInformation]    Script Date: 10/2/2024 5:19:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[GroupMembership] ON 

INSERT [dbo].[GroupMembership] ([id], [userId], [groupId], [status], [dateJoined]) VALUES (25, 2, 6, 1, CAST(N'2024-07-30T03:48:02.997' AS DateTime))
INSERT [dbo].[GroupMembership] ([id], [userId], [groupId], [status], [dateJoined]) VALUES (28, 8, 6, 0, CAST(N'2024-10-01T21:37:27.697' AS DateTime))
INSERT [dbo].[GroupMembership] ([id], [userId], [groupId], [status], [dateJoined]) VALUES (30, 18, 6, 1, CAST(N'2024-10-02T03:25:46.590' AS DateTime))
SET IDENTITY_INSERT [dbo].[GroupMembership] OFF
GO
SET IDENTITY_INSERT [dbo].[Groups] ON 

INSERT [dbo].[Groups] ([id], [groupId], [groupName], [description], [groupAdmin], [date_created], [status]) VALUES (6, N'c9e4e3b0-b143-48c8-8947-79733ac95373', N'joseph and friends', N'Joseph and friendships', 1033, CAST(N'2024-07-27T22:51:44.390' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Groups] OFF
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

INSERT [dbo].[Image] ([id], [userId], [imageFile], [coverPhoto]) VALUES (1011, 18, N'Image_2.jpg', N'Brown Vintage Watercolor Creative Portfolio Presentation.png')
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([roleId], [roleName]) VALUES (1, N'User')
INSERT [dbo].[Role] ([roleId], [roleName]) VALUES (2, N'Admin')
INSERT [dbo].[Role] ([roleId], [roleName]) VALUES (3, N'Spiritual Leader')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[UserAccount] ON 

INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (1033, N'6ebbf32a-98c6-4193-9836-f22a852c917e', N'Joseph', N'123', N'iantubol@gmail.com', 1, 3, N'654484', CAST(N'2024-07-27T22:18:08.493' AS DateTime), NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (1034, N'c81839f5-c45b-46b9-aac8-fc3f972a0552', N'Jino', N'123', N'jino@gmail.com', 1, 2, N'809274', CAST(N'2024-07-27T23:02:13.687' AS DateTime), NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (1035, N'9cd469ad-8ef5-4280-a663-31cb9b79d14a', N'chan', N'123', N'chan@gmail.com', 1, 1, N'449687', CAST(N'2024-07-30T05:00:17.883' AS DateTime), NULL)
INSERT [dbo].[UserAccount] ([id], [userId], [username], [password], [email], [status], [role], [vcode], [date_created], [date_modified]) VALUES (1045, N'bb9d20fe-462c-439a-a9af-8eefd016d933', N'Bogartz123', N'123', N'Patrickmarvs@gmail.com', 1, 1, N'343375', CAST(N'2024-10-02T03:21:59.170' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[UserAccount] OFF
GO
SET IDENTITY_INSERT [dbo].[UserInformation] ON 

INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (5, 1033, N'Dagondon', N'Joseph', N'891273812784', N'iantubol@gmail.com', N'Yati, Liloan, Cebu', 1, NULL, CAST(N'2024-07-27T22:18:08.537' AS DateTime))
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (6, 1034, N'Lupango', N'Jino', N'09950358463', N'jino@gmail.com', N'Lapu-Lapu City', 1, NULL, CAST(N'2024-07-27T23:02:13.733' AS DateTime))
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (7, 1035, N'Cacayan', N'Christian', N'12345678910', N'chan@gmail.com', N'Lapu-Lapu City, Pusok, 6015', 1, NULL, CAST(N'2024-07-30T05:00:17.883' AS DateTime))
INSERT [dbo].[UserInformation] ([id], [userId], [last_name], [first_name], [phone], [email], [address], [status], [bio], [date_created]) VALUES (18, 1045, N'Tatoy', N'Marvin', N'09924818641', N'Patrickmarvs@gmail.com', N'Bantayan, Kampingganon, 1234', 1, N'Biot', CAST(N'2024-10-02T03:21:59.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[UserInformation] OFF
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Groups] FOREIGN KEY([groupId])
REFERENCES [dbo].[Groups] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Groups]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_UserAccount] FOREIGN KEY([createdBy])
REFERENCES [dbo].[UserAccount] ([id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_UserAccount]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Event1] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([id])
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
ALTER TABLE [dbo].[Forum]  WITH CHECK ADD  CONSTRAINT [FK_Forum_Groups] FOREIGN KEY([groupId])
REFERENCES [dbo].[Groups] ([id])
GO
ALTER TABLE [dbo].[Forum] CHECK CONSTRAINT [FK_Forum_Groups]
GO
ALTER TABLE [dbo].[Forum]  WITH CHECK ADD  CONSTRAINT [FK_Forum_UserAccount] FOREIGN KEY([createdBy])
REFERENCES [dbo].[UserAccount] ([id])
GO
ALTER TABLE [dbo].[Forum] CHECK CONSTRAINT [FK_Forum_UserAccount]
GO
ALTER TABLE [dbo].[GroupMembership]  WITH CHECK ADD  CONSTRAINT [FK_GroupMembership_Groups] FOREIGN KEY([groupId])
REFERENCES [dbo].[Groups] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMembership] CHECK CONSTRAINT [FK_GroupMembership_Groups]
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_UserAccount] FOREIGN KEY([groupAdmin])
REFERENCES [dbo].[UserAccount] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_UserAccount]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_UserInformation] FOREIGN KEY([userId])
REFERENCES [dbo].[UserInformation] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_UserInformation]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_Event] FOREIGN KEY([eventId])
REFERENCES [dbo].[Event] ([id])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_Event]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_Forum] FOREIGN KEY([forumId])
REFERENCES [dbo].[Forum] ([id])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_Forum]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_Post] FOREIGN KEY([postId])
REFERENCES [dbo].[Post] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_Post]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Groups] FOREIGN KEY([groupId])
REFERENCES [dbo].[Groups] ([id])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_Groups]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_UserAccount] FOREIGN KEY([createdBy])
REFERENCES [dbo].[UserAccount] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_UserAccount]
GO
ALTER TABLE [dbo].[UserAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserAccount_Role] FOREIGN KEY([role])
REFERENCES [dbo].[Role] ([roleId])
GO
ALTER TABLE [dbo].[UserAccount] CHECK CONSTRAINT [FK_UserAccount_Role]
GO
ALTER TABLE [dbo].[UserInformation]  WITH CHECK ADD  CONSTRAINT [FK_UserInformation_UserAccount] FOREIGN KEY([userId])
REFERENCES [dbo].[UserAccount] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserInformation] CHECK CONSTRAINT [FK_UserInformation_UserAccount]
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
