using UnityEngine;

namespace Source
{
    [CreateAssetMenu(fileName = "CursorsConfig", menuName = "Config/Cursors", order = 0)]
    public class CursorsConfig : ScriptableObject
    {
        public Texture2D Default;
        public Texture2D Drag;
        public Texture2D HResize;
        public Texture2D VResize;
        public Texture2D Move;
        public Texture2D Rotate;
    }
}