using System;
using System.Windows;

namespace Sample.WpfNeko
{
    public static class NekoStates
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract class StateBase
        {
            protected readonly NekoStateContext _context;
            protected int _motion;

            protected Neko Neko => _context.Neko;

            protected double TotalElapsed { get; set; } = 0.0;

            protected StateBase(NekoStateContext context)
            {
                _context = context;
            }

            public void Reset()
            {
                TotalElapsed = 0.0;
                _motion = 0;

                DoReset();
            }

            protected abstract void DoReset();

            public void Update(double elapsed)
            {
                TotalElapsed += elapsed;

                DoUpdate(elapsed);
            }

            protected abstract void DoUpdate(double elapsed);
        }

        /// <summary>
        /// デフォルト状態
        /// </summary>
        public sealed class DefaultState : StateBase
        {
            private double _changeInterval;
            private Func<StateBase>[] _states;

            public DefaultState(NekoStateContext context)
                : base(context)
            {
                _states = new Func<StateBase>[]
                {
                    () => _context.YawnState,
                    () => _context.FootState,
                    () => _context.ScratchState,
                    () => _context.SleepingState,
                };
            }

            protected override void DoReset()
            {
                _changeInterval = MathUtil.Rand(1, 4);

                Neko.SetImage("default.png");
            }

            protected override void DoUpdate(double elapsed)
            {
                if (_changeInterval <= TotalElapsed)
                {
                    _context.SetState(_states[MathUtil.Rand(_states.Length)]());
                }
            }
        }

        /// <summary>
        /// 気付き状態
        /// </summary>
        public sealed class AwareState : StateBase
        {
            public AwareState(NekoStateContext context)
                : base(context)
            {
            }

            protected override void DoReset()
            {
                Neko.SetImage("aware.png");
            }

            protected override void DoUpdate(double elapsed)
            {
                const double MotionInterval = 0.2;
                if (MotionInterval <= TotalElapsed)
                {
                    _context.SetState(_context.MovingState);
                }
            }
        }

        /// <summary>
        /// 移動状態
        /// </summary>
        public sealed class MovingState : StateBase
        {
            public MovingState(NekoStateContext context)
                : base(context)
            {
            }

            protected override void DoReset()
            {
                Neko.SetImage("default.png");
            }

            protected override void DoUpdate(double elapsed)
            {
                var motionInterval = 18.0 / Neko.Speed;
                if (motionInterval <= TotalElapsed)
                {
                    if (Neko.GetCollisionRect().Contains(Neko.TrackingTargetPos.Value))
                    {
                        _context.SetState(_context.DefaultState);
                        return;
                    }

                    var angle = MathUtil.RadianToDegree(Math.Atan2(-(Neko.TrackingTargetPos.Value.Y - Neko.Pos.Y), Neko.TrackingTargetPos.Value.X - Neko.Pos.X));
                    angle = (angle + 360) % 360;

                    var no = (int)((angle + 22.5) % 360 / 45.0);
                    Neko.SetImage($"move{no}_{_motion}.png");
                    _motion = (_motion + 1) % 2;

                    // 移動
                    Neko.SetPos(
                        new Point(
                            Neko.Pos.X + Math.Cos(MathUtil.DegreeToRadian(angle)) * TotalElapsed * Neko.Speed,
                            Neko.Pos.Y - Math.Sin(MathUtil.DegreeToRadian(angle)) * TotalElapsed * Neko.Speed));

                    TotalElapsed -= motionInterval;
                }
            }
        }

        /// <summary>
        /// ひっかき状態
        /// </summary>
        public sealed class ScratchState : StateBase
        {
            private int _no;
            private int _count;

            public ScratchState(NekoStateContext context)
                : base(context)
            {
            }

            protected override void DoReset()
            {
                _no = MathUtil.Rand(4);
                _count = 0;
            }

            protected override void DoUpdate(double elapsed)
            {
                const double MotionInterval = 0.5;
                if (MotionInterval <= TotalElapsed)
                {
                    Neko.SetImage($"scratch{_no}_{_motion}.png");
                    _motion = (_motion + 1) % 2;

                    TotalElapsed -= MotionInterval;
                    _count++;
                }

                if (8 < _count)
                {
                    _context.SetState(_context.DefaultState);
                }
            }
        }

        /// <summary>
        /// 首かき状態
        /// </summary>
        public sealed class FootState : StateBase
        {
            private int _count;

            public FootState(NekoStateContext context)
                : base(context)
            {
            }

            protected override void DoReset()
            {
                _count = 0;
            }

            protected override void DoUpdate(double elapsed)
            {
                const double MotionInterval = 0.12;
                if (MotionInterval <= TotalElapsed)
                {
                    Neko.SetImage($"foot_{_motion}.png");
                    _motion = (_motion + 1) % 2;

                    TotalElapsed -= MotionInterval;
                    _count++;
                }

                if (12 < _count)
                {
                    _context.SetState(_context.DefaultState);
                }
            }
        }

        /// <summary>
        /// あくび状態
        /// </summary>
        public sealed class YawnState : StateBase
        {
            public YawnState(NekoStateContext context)
                : base(context)
            {
            }

            protected override void DoReset()
            {
                Neko.SetImage("yawn.png");
            }

            protected override void DoUpdate(double elapsed)
            {
                const double MotionInterval = 1.5;
                if (MotionInterval <= TotalElapsed)
                {
                    _context.SetState(_context.DefaultState);
                }
            }
        }

        /// <summary>
        /// 睡眠状態
        /// </summary>
        public sealed class SleepingState : StateBase
        {
            public SleepingState(NekoStateContext context)
                : base(context)
            {
            }

            protected override void DoReset()
            {
                Neko.SetImage("sleep_0.png");
            }

            protected override void DoUpdate(double elapsed)
            {
                const double MotionInterval = 0.5;
                if (MotionInterval <= TotalElapsed)
                {
                    _motion = (_motion + 1) % 2;
                    Neko.SetImage($"sleep_{_motion}.png");
                    TotalElapsed -= MotionInterval;
                }
            }
        }
    }
}
