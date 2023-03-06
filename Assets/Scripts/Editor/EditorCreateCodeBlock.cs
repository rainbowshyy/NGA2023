using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIDataManager))]
public class EditorCreateCodeBlock : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UIDataManager uIDataManager = (UIDataManager)target;
        foreach (CodeBlockTypes type in Enum.GetValues(typeof(CodeBlockTypes)))
        {
            if (GUILayout.Button("Create " + type.ToString()))
            {
                uIDataManager.CreateCodeBlock(CodeBlockManager.GetCodeFromStruct(new CodeBlockStruct(type, new int[3] { 1, 1, 1}, null, 0), false));
            }
        }
    }
}
