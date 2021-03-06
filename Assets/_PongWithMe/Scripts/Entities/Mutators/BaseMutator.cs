namespace PongWithMe
{
    public abstract class BaseMutator
    {
        public abstract string Announcement { get; }
        protected IStateManager _stateManager = null;
        public abstract void ActivateMutator();

        protected BaseMutator(IStateManager stateManager = null)
        {
            _stateManager = stateManager ?? StateManager.Instance;
        }

        public virtual bool CanActivate()
        {
            return true;
        }
    }
}