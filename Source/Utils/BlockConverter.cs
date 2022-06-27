using System;
using System.Collections;
using UnityEngine;

namespace Source
{
    [ExecuteInEditMode]
    public class BlockConverter : MonoBehaviour
    {
        public GameObject prefab;
        
        private void OnValidate()
        {
            if(prefab != null)
                StartCoroutine(Convert());
        }

        private IEnumerator Convert()
        {
            var blockLayout = GetComponent<BE2_BlockVerticalLayout>();
            var block = GetComponent<BE2_Block>();
            
            var operationDrag = GetComponent<BE2_DragOperation>();
            var operationHeader = GetComponent<BE2_BlockSectionHeader_Operation>();
            
            var triggerDrag = GetComponent<BE2_DragTrigger>();
            var stack = GetComponent<BE2_BlocksStack>();
            
            var blockDrag = GetComponent<BE2_DragBlock>();

            var instruction = GetComponent<BE2_InstructionBase>();

            var sectionCount = transform.childCount;
            for (var i = 0; i < sectionCount; ++i)
            {
                var section = transform.GetChild(i).gameObject;
                var outer = section.GetComponent<BE2_SpotOuterArea>();
                var blockSection = section.GetComponent<BE2_BlockSection>();
                
                var headerCount = section.transform.childCount;
                for (var j = 0; j < headerCount; ++j)
                {
                    var header = section.transform.GetChild(j).gameObject;
                    var bodySection = header.GetComponent<BE2_BlockSectionBody>();
                    var bodySpot = header.GetComponent<BE2_SpotBlockBody>();
                    var headerSection = header.GetComponent<BE2_BlockSectionHeader>();
                
                    var fieldCount = header.transform.childCount;
                    for (var k = 0; k < fieldCount; ++k)
                    {
                        var field = header.transform.GetChild(k).gameObject;
                        var label = field.GetComponent<BE2_BlockSectionHeader_Label>();
                        var dropdown = field.GetComponent<BE2_BlockSectionHeader_Dropdown>();
                        var input = field.GetComponent<BE2_BlockSectionHeader_InputField>();
                        var inputSpot = field.GetComponent<BE2_SpotBlockInput>();
                        
                        DestroyImmediate(label);
                        DestroyImmediate(dropdown);
                        DestroyImmediate(input);
                        DestroyImmediate(inputSpot);
                    }
                    
                    DestroyImmediate(bodySection);
                    DestroyImmediate(bodySpot);
                    DestroyImmediate(headerSection);
                }
                
                DestroyImmediate(outer);
                DestroyImmediate(blockSection);
            }
                
            DestroyImmediate(instruction);
            DestroyImmediate(stack);
            DestroyImmediate(triggerDrag);
            DestroyImmediate(operationHeader);
            DestroyImmediate(operationDrag);
            DestroyImmediate(blockDrag);
            DestroyImmediate(block);
            DestroyImmediate(blockLayout);

            var selectionBlock = gameObject.GetComponent<BE2_UI_SelectionBlock>();
            if (selectionBlock == null)
                selectionBlock = gameObject.AddComponent<BE2_UI_SelectionBlock>();
            selectionBlock.prefabBlock = prefab;
            
            var dragSelectionBlock = gameObject.GetComponent<BE2_DragSelectionBlock>();
            if (dragSelectionBlock == null)
                gameObject.AddComponent<BE2_DragSelectionBlock>();
            
            yield return new WaitForSeconds(0.2f);
            
            DestroyImmediate(this);
        }
    }
}