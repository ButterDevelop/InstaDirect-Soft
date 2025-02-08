using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.Tools
{
    public class Translate
    {
        public static string local = "Русский";
        public static List<string> ru = new List<string>();
        public static List<string> en = new List<string>();

        public static void Init()
        {
            local = Properties.Settings.Default.Language;
            ru = Properties.Resources.ru.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            en = Properties.Resources.en.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string Tr(string x)
        {
            if (local == "English") return en[ru.FindIndex(s => s == x)]; else return x;
        }
    }
}
