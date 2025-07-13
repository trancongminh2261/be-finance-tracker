﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MonaDotNetTemplate.Utilities
{
    public class FileUtilities
    {
        public static void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["Path is null"]);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void SaveToPath(string path, byte[] fileContent)
        {
            if (fileContent == null)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest, ["FileContent is null"]);
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path is empty");
            }

            File.WriteAllBytes(path, fileContent);
        }

        public static string GetContentType(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path is empty");
            }
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        public static bool ValidExtension(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path is empty");
            }
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.Any(s => s.Key == ext);
        }

        public static byte[] StreamToByte(Stream input)
        {
            if (input == null)
            {
                throw new AppException(CoreContant.ResponseMessageType.BadRequest,["Stream invalid"]);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
