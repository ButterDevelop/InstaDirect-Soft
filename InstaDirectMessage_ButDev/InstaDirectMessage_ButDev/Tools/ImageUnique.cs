using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.Tools
{
    public class ImageUnique
    {
        public static Image ChangeImage(Image a)
        {
            Bitmap imageBit = new Bitmap(a);
            Random random = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < (imageBit.Width * imageBit.Height) / 100; i++) imageBit.SetPixel(random.Next(imageBit.Width), random.Next(imageBit.Height), Color.Aqua);
            Image image = imageBit;

            for (int i = 0; i < (image.Width * image.Height) / 200000; i++)
            {
                int x = random.Next(image.Width);
                using (Graphics g = Graphics.FromImage(image)) g.DrawLine(Pens.Aquamarine, x, 0, x, image.Height - 1);
            }
            for (int i = 0; i < (image.Width * image.Height) / 200000; i++)
            {
                int x = random.Next(image.Height);
                using (Graphics g = Graphics.FromImage(image)) g.DrawLine(Pens.Beige, 0, x, image.Width - 1, x);
            }

            string[] messages = new string[] { "Avatar", "Cool photo", "It's me", "No way", "Nice!", "Great", "COOLEST", "Easy :)", "Heeeeeey", "Broooooo" };
            using (Graphics g = Graphics.FromImage(image)) g.DrawString(messages[random.Next(messages.Length)], new Font("Arial", random.Next(55, 70)), new SolidBrush(Color.DarkBlue), image.Width / 2, image.Height / 2);

            return image;
        }
    }
}
