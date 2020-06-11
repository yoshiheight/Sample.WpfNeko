namespace Sample.WpfNeko
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NekoStateContext
    {
        public Neko Neko { get; }
        private NekoStates.StateBase _state;

        public NekoStates.StateBase DefaultState { get; }
        public NekoStates.StateBase AwareState { get; }
        public NekoStates.StateBase MovingState { get; }
        public NekoStates.StateBase ScratchState { get; }
        public NekoStates.StateBase FootState { get; }
        public NekoStates.StateBase YawnState { get; }
        public NekoStates.StateBase SleepingState { get; }

        public NekoStateContext(Neko neko)
        {
            Neko = neko;

            DefaultState = new NekoStates.DefaultState(this);
            AwareState = new NekoStates.AwareState(this);
            MovingState = new NekoStates.MovingState(this);
            ScratchState = new NekoStates.ScratchState(this);
            FootState = new NekoStates.FootState(this);
            YawnState = new NekoStates.YawnState(this);
            SleepingState = new NekoStates.SleepingState(this);

            SetState(DefaultState);
        }

        public void SetState(NekoStates.StateBase newState)
        {
            _state = newState;
            _state.Reset();
        }

        public void Update(double elapsed)
        {
            _state.Update(elapsed);

            if (!(_state is NekoStates.AwareState)
                && !(_state is NekoStates.MovingState)
                && Neko.TrackingTargetPos.HasValue && !Neko.GetCollisionRect().Contains(Neko.TrackingTargetPos.Value))
            {
                SetState(AwareState);
            }
        }
    }
}
