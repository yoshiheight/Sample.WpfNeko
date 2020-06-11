using System;
using System.Windows;
using System.Windows.Media;

namespace Sample.WpfNeko
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Scene _scene;

        public MainWindow()
        {
            InitializeComponent();

            _scene = new Scene(LayoutRoot);

            _scene.AddGameObject(new Neko()).SetPos(new Point(20.0, 20.0));

            TimeSpan? old = null;
            CompositionTarget.Rendering += (s, e) =>
            {
                var re = (RenderingEventArgs)e;
                var elapsed = (old != null) ? re.RenderingTime.TotalMilliseconds - old.Value.TotalMilliseconds : 0.0;
                _scene.Update(elapsed / 1000.0);

                old = re.RenderingTime;
            };
        }
    }
}
