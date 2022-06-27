namespace Source
{
    public class ExecutionManagerInterface
    {
        private readonly BE2_ExecutionManager executionManager;

        private bool _playing;
        
        public bool Playing => _playing;

        public ExecutionManagerInterface(BE2_ExecutionManager executionManager)
        {
            this.executionManager = executionManager;
        }

        public void Play()
        {
            _playing = true;
            executionManager.Play();
        }

        public void Stop()
        {
            _playing = false;
            executionManager.Stop();
        }
    }
}