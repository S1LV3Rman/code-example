using System;
using System.Collections;
using UnityEngine;

namespace Source
{
    public class LineSensor : Module, ISensor
    {
        [SerializeField] private Transform rayStartPoint;
        
        public event Action<BotPort, int> OnValueChange;

        private void FixedUpdate()
        {
            if (TryGetColor(out var color))
                OnValueChange?.Invoke(Port, (int) ((1f - color.grayscale) * 1023f));
        }

        private bool TryGetColor(out Color color)
        {
            color = new Color();
            
            if (!Physics.Raycast(rayStartPoint.position, rayStartPoint.forward, out var hit))
                return false;

            var rend = hit.transform.GetComponent<Renderer>();
            var meshCollider = hit.collider as MeshCollider;
            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return false;

            var tex = rend.material.mainTexture as Texture2D;
            if (tex == null)
                return false;
            
            var pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

            color = tex.GetPixel ((int)pixelUV.x, (int)pixelUV.y);

            return true;
        }
    }
}