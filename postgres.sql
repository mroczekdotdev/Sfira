CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE EXTENSION IF NOT EXISTS citext;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "AspNetRoles" (
        "Id" text NOT NULL,
        "Name" character varying(256) NULL,
        "NormalizedName" character varying(256) NULL,
        "ConcurrencyStamp" text NULL,
        CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
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
        "ProfileImage" boolean NOT NULL,
        "HeaderImage" boolean NOT NULL,
        "CountryRegion" text NULL,
        "Language" text NULL,
        "TimeZone" text NULL,
        CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "AspNetRoleClaims" (
        "Id" serial NOT NULL,
        "RoleId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "AspNetUserClaims" (
        "Id" serial NOT NULL,
        "UserId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "AspNetUserLogins" (
        "LoginProvider" character varying(128) NOT NULL,
        "ProviderKey" character varying(128) NOT NULL,
        "ProviderDisplayName" text NULL,
        "UserId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "AspNetUserRoles" (
        "UserId" text NOT NULL,
        "RoleId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "AspNetUserTokens" (
        "UserId" text NOT NULL,
        "LoginProvider" character varying(128) NOT NULL,
        "Name" character varying(128) NOT NULL,
        "Value" text NULL,
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "Posts" (
        "Id" serial NOT NULL,
        "AuthorId" text NULL,
        "PublicationTime" timestamp without time zone NOT NULL,
        "Message" text NULL,
        "Tags" text NULL,
        "LikesCount" integer NOT NULL,
        "FavoritesCount" integer NOT NULL,
        "CommentsCount" integer NOT NULL,
        CONSTRAINT "PK_Posts" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Posts_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "Attachments" (
        "Name" uuid NOT NULL,
        "PostId" integer NOT NULL,
        "OwnerId" text NULL,
        "Discriminator" text NOT NULL,
        "Extension" text NULL,
        CONSTRAINT "PK_Attachments" PRIMARY KEY ("Name"),
        CONSTRAINT "FK_Attachments_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Attachments_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "Comments" (
        "Id" serial NOT NULL,
        "AuthorId" text NULL,
        "PublicationTime" timestamp without time zone NOT NULL,
        "Message" text NULL,
        "ParentId" integer NULL,
        CONSTRAINT "PK_Comments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Comments_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Comments_Posts_ParentId" FOREIGN KEY ("ParentId") REFERENCES "Posts" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE TABLE "UserPosts" (
        "UserId" text NOT NULL,
        "PostId" integer NOT NULL,
        "Relation" text NOT NULL,
        CONSTRAINT "PK_UserPosts" PRIMARY KEY ("UserId", "PostId"),
        CONSTRAINT "FK_UserPosts_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_UserPosts_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE UNIQUE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_Attachments_OwnerId" ON "Attachments" ("OwnerId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE UNIQUE INDEX "IX_Attachments_PostId" ON "Attachments" ("PostId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_Comments_AuthorId" ON "Comments" ("AuthorId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_Comments_ParentId" ON "Comments" ("ParentId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_Posts_AuthorId" ON "Posts" ("AuthorId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    CREATE INDEX "IX_UserPosts_PostId" ON "UserPosts" ("PostId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190915202801_Initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190915202801_Initial', '2.2.6-servicing-10079');
    END IF;
END $$;
