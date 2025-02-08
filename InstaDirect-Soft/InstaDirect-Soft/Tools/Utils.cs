using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.Tools
{
    public class Utils
    {
        public static string RandPattern(string text)
        {
            string[] Привет = { "Привет!", "Здравствуйте!", "Конишива!", "Вiтаю!", "Доброго времени суток!", "Здоровеньки були!", "Хай!", "Хелло!" };
            string[] Hello = {  "Hello!", "Hi!", "Good day!", "Good afternoon!" };
            string[] Пока = { "До свидания!", "Пока!", "Да пабачэння!", "Гудбай!" };
            string[] Bye = { "Goodbye!", "Bye!", "See you!", "Adiue!" };
            string[] ЕстьМинутка = { "Спешу уведомить.", "Я надолго не задержу.", "Есть минутка?", "Есть немного времени?" };
            string[] Спасибо = { "Спасибо!", "Благодарю!", "Спасибки!", "Моя благодарность!" };
            string[] Thanks = { "Thank you!", "Thanks!", "Thanks a lot!", "Thankee!" };
            Random rnd = new Random();
            text = text.Replace("{$Привет$}", Привет[rnd.Next(Hello.Length)]);
            text = text.Replace("{$Hello$}", Hello[rnd.Next(Hello.Length)]);

            text = text.Replace("{$Пока$}", Пока[rnd.Next(Bye.Length)]);
            text = text.Replace("{$Bye$}", Bye[rnd.Next(Bye.Length)]);

            text = text.Replace("{$ЕстьМинутка?$}", ЕстьМинутка[rnd.Next(ЕстьМинутка.Length)]);

            text = text.Replace("{$Спасибо$}", Спасибо[rnd.Next(Спасибо.Length)]);
            text = text.Replace("{$Thanks$}", Thanks[rnd.Next(Спасибо.Length)]);

            Regex regex = new Regex("\\{(.*)\\}");
            foreach (Match match in regex.Matches(text))
            {
                string m = match.Value;
                m = m.Remove(m.Length - 1);
                m = m.Remove(0, 1);
                string[] variants = m.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                text = text.Replace(match.Value, variants[rnd.Next(variants.Length)]);
            }

            return text;
        }
    }
}
