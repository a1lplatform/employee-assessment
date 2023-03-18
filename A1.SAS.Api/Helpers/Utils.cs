﻿using A1.SAS.Api.Extension;
using System.Security.Cryptography;

namespace A1.SAS.Api.Helpers
{
    public static class Utils
    {
        private static string EncryptionKey = "A1platform@2022";

        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string inputPwd, string storedPwd)
        {
            byte[] hashBytes = Convert.FromBase64String(storedPwd);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(inputPwd, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }

        public static string GenerateA1PCode(string codeType)
        {
            return $"{codeType}-{DateTime.Now.ToUnixTime()}";
        }
    }
}
