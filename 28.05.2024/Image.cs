using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _28._05._2024
{
    public static  class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");
        public readonly static ImageSource Food1 = LoadImage("Food1.png");
        public readonly static ImageSource Food2 = LoadImage("Food2.png");
        public readonly static ImageSource Food3 = LoadImage("Food3.png");
       
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");
        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
