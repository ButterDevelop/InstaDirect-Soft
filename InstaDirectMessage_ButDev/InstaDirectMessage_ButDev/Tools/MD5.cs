using InstaDirectMessage_ButDev.Tools;
using System;
using System.Security.Cryptography;
using System.Text;

namespace InstaDirectMessage_ButDev.Tools
{
    class MD5MyHash
    {
        public static string Encode(string x)
        {
            while (true)
            {
                string source = B64X.Encrypt(x);
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = B64X.Encrypt(GetMd5Hash(md5Hash, B64X.Decrypt(source)));
                    if (VerifyMd5Hash(md5Hash, B64X.Decrypt(source), B64X.Decrypt(hash))) return B64X.Decrypt(hash).ToUpper();
                }
            }
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = B64X.Encrypt(GetMd5Hash(md5Hash, input));

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(B64X.Decrypt(hashOfInput), hash)) return true; else return false;
        }

    }
}
