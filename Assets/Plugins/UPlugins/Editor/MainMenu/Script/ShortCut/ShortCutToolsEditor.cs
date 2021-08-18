/////////////////////////////////////////////////////////////////////////////
//
//  Script : ShortCutToolsEditor.cs
//  Info   : 切换工具栏常用功能的快捷键
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio / Change from http://www.xuanyusong.com/archives/3900
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Aya.EditorScript
{
    public class ShortCutToolsEditor : MonoBehaviour
    {
        [MenuItem("Aya Game Studio/Short Cut/Toolbar/Hand &V", false)]
        static void ToolsMethodView()
        {
            Tools.current = Tool.View;
        }

        [MenuItem("Aya Game Studio/Short Cut/Toolbar/Move &M", false)]
        static void ToolsMethodMove()
        {
            Tools.current = Tool.Move;
        }

        [MenuItem("Aya Game Studio/Short Cut/Toolbar/Rotate &R", false)]
        static void ToolsMethodRotate()
        {
            Tools.current = Tool.Rotate;
        }

        [MenuItem("Aya Game Studio/Short Cut/Toolbar/Scale &S", false)]
        static void ToolsMethoScale放()
        {
            Tools.current = Tool.Scale;
        }

        [MenuItem("Aya Game Studio/Short Cut/Toolbar/Size #&S", false)]
        static void ToolsMethodRect()
        {
            Tools.current = Tool.Rect;
        }
    }
}
#endif