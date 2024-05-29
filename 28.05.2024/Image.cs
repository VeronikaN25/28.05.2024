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
        public readonly static ImageSource Persik = LoadImage("persik.png");
        public readonly static ImageSource Something = LoadImage("something.png");
        public readonly static ImageSource Avocado = LoadImage("avocado.png");
       
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");
        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
