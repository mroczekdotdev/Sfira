CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE EXTENSION IF NOT EXISTS citext;

CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "UserName" citext NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    "RegisterTime" timestamp without time zone NOT NULL,
    "Name" text NULL,
    "Description" text NULL,
    "Location" text NULL,
    "Website" text NULL,
    "CountryRegion" text NULL,
    "Language" text NULL,
    "TimeZone" text NULL,
    "AvatarImage" text NULL,
    "CoverImage" text NULL,
    "FollowingCount" integer NOT NULL,
    "FollowersCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" serial NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" serial NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Posts" (
    "Id" serial NOT NULL,
    "AuthorId" text NULL,
    "PublicationTime" timestamp without time zone NOT NULL,
    "Body" text NULL,
    "Tags" citext NULL,
    "LikesCount" integer NOT NULL,
    "FavoritesCount" integer NOT NULL,
    "CommentsCount" integer NOT NULL,
    CONSTRAINT "PK_Posts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Posts_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "UserBlocks" (
    "BlockingUserId" text NOT NULL,
    "BlockedUserId" text NOT NULL,
    CONSTRAINT "PK_UserBlocks" PRIMARY KEY ("BlockingUserId", "BlockedUserId"),
    CONSTRAINT "FK_UserBlocks_AspNetUsers_BlockedUserId" FOREIGN KEY ("BlockedUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserBlocks_AspNetUsers_BlockingUserId" FOREIGN KEY ("BlockingUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserFollows" (
    "FollowingUserId" text NOT NULL,
    "FollowedUserId" text NOT NULL,
    CONSTRAINT "PK_UserFollows" PRIMARY KEY ("FollowingUserId", "FollowedUserId"),
    CONSTRAINT "FK_UserFollows_AspNetUsers_FollowedUserId" FOREIGN KEY ("FollowedUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserFollows_AspNetUsers_FollowingUserId" FOREIGN KEY ("FollowingUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Attachments" (
    "Name" uuid NOT NULL,
    "ParentId" integer NOT NULL,
    "OwnerId" text NULL,
    "Discriminator" text NOT NULL,
    "Extension" text NULL,
    CONSTRAINT "PK_Attachments" PRIMARY KEY ("Name"),
    CONSTRAINT "FK_Attachments_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Attachments_Posts_ParentId" FOREIGN KEY ("ParentId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Comments" (
    "Id" serial NOT NULL,
    "AuthorId" text NULL,
    "PublicationTime" timestamp without time zone NOT NULL,
    "Body" text NULL,
    "ParentId" integer NOT NULL,
    CONSTRAINT "PK_Comments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Comments_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Comments_Posts_ParentId" FOREIGN KEY ("ParentId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserPosts" (
    "UserId" text NOT NULL,
    "PostId" integer NOT NULL,
    "Relation" text NOT NULL,
    CONSTRAINT "PK_UserPosts" PRIMARY KEY ("UserId", "PostId"),
    CONSTRAINT "FK_UserPosts_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserPosts_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Messages" (
    "Id" serial NOT NULL,
    "AuthorId" text NULL,
    "PublicationTime" timestamp without time zone NOT NULL,
    "Body" text NULL,
    "ChatId" integer NOT NULL,
    CONSTRAINT "PK_Messages" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Messages_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Chats" (
    "Id" serial NOT NULL,
    "LastMessageId" integer NULL,
    "Discriminator" text NOT NULL,
    CONSTRAINT "PK_Chats" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Chats_Messages_LastMessageId" FOREIGN KEY ("LastMessageId") REFERENCES "Messages" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "UserChats" (
    "UserId" text NOT NULL,
    "ChatId" integer NOT NULL,
    "LastReadTime" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_UserChats" PRIMARY KEY ("UserId", "ChatId"),
    CONSTRAINT "FK_UserChats_Chats_ChatId" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserChats_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE UNIQUE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_Attachments_OwnerId" ON "Attachments" ("OwnerId");

CREATE UNIQUE INDEX "IX_Attachments_ParentId" ON "Attachments" ("ParentId");

CREATE UNIQUE INDEX "IX_Chats_LastMessageId" ON "Chats" ("LastMessageId");

CREATE INDEX "IX_Comments_AuthorId" ON "Comments" ("AuthorId");

CREATE INDEX "IX_Comments_ParentId" ON "Comments" ("ParentId");

CREATE INDEX "IX_Messages_AuthorId" ON "Messages" ("AuthorId");

CREATE INDEX "IX_Messages_ChatId" ON "Messages" ("ChatId");

CREATE INDEX "IX_Posts_AuthorId" ON "Posts" ("AuthorId");

CREATE INDEX "IX_UserBlocks_BlockedUserId" ON "UserBlocks" ("BlockedUserId");

CREATE INDEX "IX_UserChats_ChatId" ON "UserChats" ("ChatId");

CREATE INDEX "IX_UserFollows_FollowedUserId" ON "UserFollows" ("FollowedUserId");

CREATE INDEX "IX_UserPosts_PostId" ON "UserPosts" ("PostId");

ALTER TABLE "Messages" ADD CONSTRAINT "FK_Messages_Chats_ChatId" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20191115002357_Initial', '2.2.6-servicing-10079');

