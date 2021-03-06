CREATE TABLE [dbo].[Item](
	[Id] [int] NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Shop](
	[Id] [int] NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Shop] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Journal_Sales](
	[Id] [int] NOT NULL,
	[BuyerId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[ShopId] [int] NOT NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Journal_Sales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Buyer](
	[Id] [int] NOT NULL,
	[Sex] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Buyer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]