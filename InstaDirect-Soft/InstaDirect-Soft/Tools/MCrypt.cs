using InstaDirectMessage_ButDev.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.Tools
{
    public static class MCrypt
    {
        private static string[] list = new string[10] { "A0DHY", "1ZLOP", "QEWX2", "TF3G", "U4CB", "5IMN", "R6", "7S", "J8", "K9V" };
        private static string database = B64X.Encrypt("A0DHY1ZLOPQEWX2TF3GU4CB5IMNR67SJ8K9V");
        private static int[] listS = new int[36] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 9 };

        public static string Encrypt(string x)
        {
            string res = "";
            Random rng = new Random(DateTime.Now.Second * DateTime.Now.Year * DateTime.Now.DayOfYear);
            for (int i = 0; i < x.Length; i++)
            {
                int ch = x[i];
                int ed = ch % 10; ch /= 10;
                int des = ch % 10; ch /= 10;
                int sot = ch % 10; ch /= 10;
                int tys = ch % 10;

                res = B64X.Encrypt(B64X.Decrypt(res) + list[ed][rng.Next(list[ed].Length)]);
                res = B64X.Encrypt(B64X.Decrypt(res) + list[des][rng.Next(list[des].Length)]);
                res = B64X.Encrypt(B64X.Decrypt(res) + list[sot][rng.Next(list[sot].Length)]);
                res = B64X.Encrypt(B64X.Decrypt(res) + list[tys][rng.Next(list[tys].Length)]);
            }
            return B64X.Decrypt(res);
        }

        public static string Decrypt(string x)
        {
            string res = "";
            for (int i = 0; i < x.Length; i += 4)
            {
                int ed = listS[B64X.Decrypt(database).IndexOf(x[i])];
                int des = listS[B64X.Decrypt(database).IndexOf(x[i + 1])];
                int sot = listS[B64X.Decrypt(database).IndexOf(x[i + 2])];
                int tys = listS[B64X.Decrypt(database).IndexOf(x[i + 3])];
                int ch = tys * 1000 + sot * 100 + des * 10 + ed;

                res = B64X.Encrypt(B64X.Decrypt(res) + (char)ch);
            }
            return B64X.Decrypt(res);
        }
    }
}
