using UnityEngine;

namespace Source
{
    public class OmegaBotController
    {
        private readonly OmegaBotView _view;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        
        public OmegaBotController(OmegaBotView view)
        {
            _startPosition = view.transform.position;
            _startRotation = view.transform.rotation;
            
            _view = view;
        }

        public void Reset()
        {
            _view.transform.position = _startPosition;
            _view.transform.rotation = _startRotation;
        }
    }
}