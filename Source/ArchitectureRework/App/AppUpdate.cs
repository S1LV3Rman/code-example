using System;
using UnityEngine;

namespace Source
{
    public class AppUpdate : MonoBehaviour
    {
        private App _app;

        private void Start()
        {
            _app = App.Shared;
        }

        private void Update()
        {
            _app.Run();
        }

        private void OnDestroy()
        {
            _app.Destroy();
        }
    }
}