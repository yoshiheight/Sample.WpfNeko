using System.Windows;

namespace Sample.WpfNeko
{
    /// <summary>
    /// ネコクラス
    /// </summary>
    public sealed class Neko : GameObject
    {
        private NekoStateContext _stateContext;
        public Point? TrackingTargetPos { get; private set; }

        public static readonly int Speed = 60;

        public Neko()
            : base(new Size(32, 32))
        {
            _stateContext = new NekoStateContext(this);
        }

        public Rect GetCollisionRect()
        {
            var x = Pos.X - Width / 2.0;
            var y = Pos.Y - Width / 2.0;

            return new Rect(x, y, Width, Height);
        }

        public override void Update(double elapsed)
        {
            if (Scene.TryGetMousePos(out var mousePos))
            {
                TrackingTargetPos = mousePos;
            }

            _stateContext.Update(elapsed);
        }
    }
}
