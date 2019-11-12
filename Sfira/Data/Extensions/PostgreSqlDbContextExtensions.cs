﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MroczekDotDev.Sfira.Data.Extensions
{
    public static class PostgreSqlDbContextExtensions
    {
        private static readonly char ds = Path.DirectorySeparatorChar;

        public static void EnsureSeeded(this PostgreSqlDbContext context, IOptions<SeedingOptions> optionsAccessor)
        {
            string userMediaDirectory = "wwwroot" + ds + "media" + ds + "user";
            var options = optionsAccessor.Value;

            if (options.SeedDummyData)
            {
                SeedDummyData();
            }

            void SeedDummyData()
            {
                string dummyDataDirectory = "Resources" + ds + "DummyData" + ds;

                if (!context.Users.Any())
                {
                    var Users = JsonConvert.DeserializeObject<List<ApplicationUser>>(File.ReadAllText(
                        dummyDataDirectory + "Users.json"));
                    context.AddRange(Users);

                    var UserFollows = JsonConvert.DeserializeObject<List<UserFollow>>(File.ReadAllText(
                        dummyDataDirectory + "UserFollows.json"));
                    context.AddRange(UserFollows);

                    var Posts = JsonConvert.DeserializeObject<List<Post>>(File.ReadAllText(
                        dummyDataDirectory + "Posts.json"));
                    context.AddRange(Posts);
                    context.Database.ExecuteSqlCommand("ALTER SEQUENCE \"Posts_Id_seq\" RESTART WITH 65");

                    var ImageAttachments = JsonConvert.DeserializeObject<List<ImageAttachment>>(
                        File.ReadAllText(dummyDataDirectory + "ImageAttachments.json"));
                    context.AddRange(ImageAttachments);

                    var Comments = JsonConvert.DeserializeObject<List<Comment>>(
                        File.ReadAllText(dummyDataDirectory + "Comments.json"));
                    context.AddRange(Comments);
                    context.Database.ExecuteSqlCommand("ALTER SEQUENCE \"Comments_Id_seq\" RESTART WITH 37");

                    context.SaveChanges();

                    CopyDirectoryRecursively(dummyDataDirectory + "media", userMediaDirectory);
                }
            }

            void CopyDirectoryRecursively(string sourceDirName, string destDirName)
            {
                var dir = new DirectoryInfo(sourceDirName);

                if (dir.Exists)
                {
                    var dirs = dir.GetDirectories();

                    if (!Directory.Exists(destDirName))
                    {
                        Directory.CreateDirectory(destDirName);
                    }

                    foreach (FileInfo file in dir.GetFiles())
                    {
                        string destPath = Path.Combine(destDirName, file.Name);

                        var destFile = new FileInfo(destPath);

                        if (!destFile.Exists)
                        {
                            file.CopyTo(destPath, true);
                        }
                    }

                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string destPath = Path.Combine(destDirName, subdir.Name);
                        CopyDirectoryRecursively(subdir.FullName, destPath);
                    }
                }
            }
        }
    }
}
