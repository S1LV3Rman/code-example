using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source
{
    [CreateAssetMenu(fileName = "BlockSectionsConfig", menuName = "Config/BlockSections", order = 0)]
    public class BlockSectionsConfig : SerializedScriptableObject
    {
        [DictionaryDrawerSettings]
        public Dictionary<BlockSectionType, BlockSection> blockSections;
    }

    [Serializable]
    public struct BlockSection
    {
        public string name;
        public Color color;
    }

    public enum BlockSectionType
    {
        None,
        Controls,
        Movement,
        Rangefinder,
        Sensors,
        Indicators,
        Operators,
        Variables
    }
}