using System;

namespace Source
{
    public class BE2Controller
    {
        private readonly BE2View _view;
        
        public BE2Controller(BE2View view)
        {
            _view = view;
        }
        
        public void Play()
        {
            _view.ExecutionManager.Play();
        }

        public void Stop()
        {
            _view.ExecutionManager.Stop();
        }
    }
}