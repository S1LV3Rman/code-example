using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source
{
    [CreateAssetMenu(fileName = "ModulesConfig", menuName = "Config/Modules", order = 0)]
    public class ModulesConfig : SerializedScriptableObject
    {
        public Dictionary<ModuleType, ModulePrefabs> prefabs;
    }

    [Serializable]
    public struct ModulePrefabs
    {
        public GameObject real;
        public GameObject configuration;
    }
}