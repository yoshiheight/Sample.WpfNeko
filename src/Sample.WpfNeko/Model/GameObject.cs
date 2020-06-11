using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sample.WpfNeko
{
    public abstract class GameObject : Image
    {
        public Scene Scene { get; set; }
        public Point Pos { get; private set; }

        protected GameObject(Size size)
        {
            Stretch = Stretch.None;

            Width = size.Width;
            Height = size.Height;
        }

        public void SetPos(Point pos)
        {
            Pos = pos;

            Canvas.SetLeft(this, Pos.X - Width / 2.0);
            Canvas.SetTop(this, Pos.Y - Height / 2.0);
        }

        public void SetImage(string image)
        {
            Source = ResourceRepository.Instance.GetImage(image);
        }

        public abstract void Update(double elapsed);
    }
}
