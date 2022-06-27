using Sirenix.OdinInspector;
using UnityEngine;

namespace Source
{
    public class ID : MonoBehaviour
    {
        private static int nextId;

        [ReadOnly]
        public int id;

        public void Awake()
        {
            id = nextId++;
        }
        
        public static implicit operator int(ID id)
        {
            return id.id;
        }
    }
}