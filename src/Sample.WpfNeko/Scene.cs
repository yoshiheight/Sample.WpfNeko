using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sample.WpfNeko
{
    public sealed class Scene
    {
        private readonly Canvas _canvas;

        public Scene(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Update(double elapsed)
        {
            foreach (var gameObject in _canvas.Children.Cast<GameObject>())
            {
                gameObject.Update(elapsed);
            }
        }

        public bool TryGetMousePos(out Point? mousePos)
        {
            if (_canvas.RenderSize.Width > 0 && _canvas.RenderSize.Height > 0)
            {
                mousePos = Mouse.GetPosition(_canvas);
                if (new Rect(_canvas.RenderSize).Contains(mousePos.Value))
                {
                    return true;
                }
            }

            mousePos = null;
            return false;
        }

        public IEnumerable<GameObject> GameObjects => _canvas.Children.Cast<GameObject>();

        public GameObject AddGameObject(GameObject gameObject)
        {
            gameObject.Scene = this;
            _canvas.Children.Add(gameObject);
            return gameObject;
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            _canvas.Children.Remove(gameObject);
            gameObject.Scene = null;
        }
    }
}
