namespace Source
{
    public interface IAppState<TCore> : IAppState where TCore : SceneContext
    {
        public void Init(TCore core);
    }

    public interface IAppState
    {
        public bool Initialized { get; }
        public void Run();
    }
}