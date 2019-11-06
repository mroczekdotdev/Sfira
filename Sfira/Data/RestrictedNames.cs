using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MroczekDotDev.Sfira.Data
{
    public static class RestrictedNames
    {
        private static readonly string resourcesFolder = "Resources";
        private static readonly string reservedKeywords = "ReservedKeywords.txt";
        public static readonly HashSet<string> HashSet;

        static RestrictedNames()
        {
            HashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Areas.About),
                nameof(Areas.Account),

                Controllers.ChatController.Name,
                Controllers.CommentController.Name,
                Controllers.ExploreController.Name,
                Controllers.HomeController.Name,
                Controllers.MessagesController.Name,
                Controllers.PostController.Name,
                Controllers.TagController.Name,
                Controllers.UserController.Name,

                nameof(Controllers.HomeController.PostsFeed),

                nameof(Models.ApplicationUser),
                nameof(Models.Attachment),
                nameof(Models.Chat),
                nameof(Models.Comment),
                nameof(Models.DirectChat),
                nameof(Models.Entry),
                nameof(Models.ImageAttachment),
                nameof(Models.Message),
                nameof(Models.Post),
                nameof(Models.UserBlock),
                nameof(Models.UserChat),
                nameof(Models.UserFollow),
                nameof(Models.UserPost),

                nameof(Services.CachedStorage),
                nameof(Services.EmailSender),
                nameof(Services.FileUploading),
                nameof(Services.FileUploading.FileUploader),
                nameof(Services.Scheduling)
            };

            TypeInfo typeInfo = typeof(Program).GetTypeInfo();
            Assembly assembly = typeInfo.Assembly;
            string assemblyNamespace = typeInfo.Namespace;
            string resource = assemblyNamespace + "." + resourcesFolder + "." + reservedKeywords;

            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    HashSet.UnionWith(ReadLines(streamReader));
                }
            }

            IEnumerable<string> ReadLines(StreamReader streamReader)
            {
                while (!streamReader.EndOfStream)
                {
                    yield return streamReader.ReadLine();
                }
            }
        }
    }
}
