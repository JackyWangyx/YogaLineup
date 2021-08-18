/////////////////////////////////////////////////////////////////////////////
//
//  Script : ShortCutCenterLocalEditor.cs
//  Info   : 切换工具栏常用功能的快捷键
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio / Change from http://www.xuanyusong.com/archives/3900
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Aya.EditorScript
{
    public class ShortCutCenterLocalEditor : MonoBehaviour
    {
        [MenuItem("Aya Game Studio/Short Cut/Center/Switch Position Center Point #&P", false)]
        static void ChangePivotMode()
        {
            Tools.pivotMode = (Tools.pivotMode == PivotMode.Center) ? PivotMode.Pivot : PivotMode.Center;
            Refresh();
        }

        [MenuItem("Aya Game Studio/Short Cut/Center/Switch Rotation Center Point #&R", false)]
        static void ChangePivotRotation()
        {
            Tools.pivotRotation = (Tools.pivotRotation == PivotRotation.Global) ? PivotRotation.Local : PivotRotation.Global;
            Refresh();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        static void Refresh()
        {
            var info = typeof(Tools).GetMethod("CenterLoaclEditor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
            info.Invoke(null, null);
        }
    }
}
#endif