using Microsoft.EntityFrameworkCore;
using Rhetos.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rhetos.AspNetFormsAuth
{
    internal static class AccessControlDbMigrationScript
    {
        #region AspNetUsers
        public static string AspNetUsers =
            @"
            IF NOT EXISTS (SELECT * 
                           FROM INFORMATION_SCHEMA.TABLES 
                           WHERE TABLE_SCHEMA = 'dbo' 
                             AND TABLE_NAME = 'AspNetUsers') 
            BEGIN
                CREATE TABLE [dbo].[AspNetUsers](
	                [Id] [nvarchar](450) NOT NULL,
	                [AccessFailedCount] [int] NOT NULL,
	                [ConcurrencyStamp] [nvarchar](max) NULL,
	                [Email] [nvarchar](256) NULL,
	                [EmailConfirmed] [bit] NOT NULL,
	                [LockoutEnabled] [bit] NOT NULL,
	                [LockoutEnd] [datetimeoffset](7) NULL,
	                [NormalizedEmail] [nvarchar](256) NULL,
	                [NormalizedUserName] [nvarchar](256) NULL,
	                [PasswordHash] [nvarchar](max) NULL,
	                [PhoneNumber] [nvarchar](max) NULL,
	                [PhoneNumberConfirmed] [bit] NOT NULL,
	                [SecurityStamp] [nvarchar](max) NULL,
	                [TwoFactorEnabled] [bit] NOT NULL,
	                [UserName] [nvarchar](256) NULL,
                 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            END
            ";
        #endregion
        #region AspNetUserLogins
        public static string AspNetUserLogins =
            @"
            IF NOT EXISTS (SELECT * 
                           FROM INFORMATION_SCHEMA.TABLES 
                           WHERE TABLE_SCHEMA = 'dbo' 
                             AND TABLE_NAME = 'AspNetUserLogins') 
            BEGIN

            CREATE TABLE [dbo].[AspNetUserLogins](
	            [LoginProvider] [nvarchar](450) NOT NULL,
	            [ProviderKey] [nvarchar](450) NOT NULL,
	            [ProviderDisplayName] [nvarchar](max) NULL,
	            [UserId] [nvarchar](450) NOT NULL,
                CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
            (
	            [LoginProvider] ASC,
	            [ProviderKey] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

            ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
            REFERENCES [dbo].[AspNetUsers] ([Id])
            ON DELETE CASCADE

            ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]

            END
            ";
        #endregion
        #region AspNetUserTokens
        public static string AspNetUserTokens =
            @"            
            IF NOT EXISTS (SELECT * 
                           FROM INFORMATION_SCHEMA.TABLES 
                           WHERE TABLE_SCHEMA = 'dbo' 
                             AND TABLE_NAME = 'AspNetUserTokens') 
            BEGIN

                CREATE TABLE [dbo].[AspNetUserTokens](
	                [UserId] [nvarchar](450) NOT NULL,
	                [LoginProvider] [nvarchar](450) NOT NULL,
	                [Name] [nvarchar](450) NOT NULL,
	                [Value] [nvarchar](max) NULL,
                 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
                (
	                [UserId] ASC,
	                [LoginProvider] ASC,
	                [Name] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
                REFERENCES [dbo].[AspNetUsers] ([Id])
                ON DELETE CASCADE

                ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
            
            END
            ";
        #endregion
    }
    public class AccessControlDbContextInitializer : IDatabaseInitializer
    {
        private readonly AccessControlDbContext _accessControlDbContext;

        public AccessControlDbContextInitializer(AccessControlDbContext accessControlDbContext)
        {
            _accessControlDbContext = accessControlDbContext;
        }

        public void Initialize()
        {
            using (var transaction = _accessControlDbContext.Database.BeginTransaction())
            {
                _accessControlDbContext.Database.ExecuteSqlCommand(new RawSqlString(AccessControlDbMigrationScript.AspNetUsers));
                _accessControlDbContext.Database.ExecuteSqlCommand(new RawSqlString(AccessControlDbMigrationScript.AspNetUserTokens));
                _accessControlDbContext.Database.ExecuteSqlCommand(new RawSqlString(AccessControlDbMigrationScript.AspNetUserLogins));

                transaction.Commit();
            }
        }
    }
}
