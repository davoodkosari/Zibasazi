/****** Object:  Table [Security].[RadynConfig]    Script Date: 6/13/2016 3:48:01 PM ******/

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Security].[RadynConfig]') AND type in (N'U'))

BEGIN

CREATE TABLE [Security].[RadynConfig](
	[Name] [varchar](100) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Type] [varchar](100) NULL,
 CONSTRAINT [PK_RadynConfig] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

GO

/*************************************************************************************/
/** Set Db Version **/

INSERT INTO [Security].[RadynConfig] (Name, Value) VALUES ('DbVersion', '3.0.0')

Go
