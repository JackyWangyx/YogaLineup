/////////////////////////////////////////////////////////////////////////////
//
//  Script : PlayerPrefsEditor.cs
//  Info   : 玩家存档数据操作
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Aya.EditorScript
{
    public class PlayerPrefsEditor : MonoBehaviour
    {
        /// <summary>
        /// 清除清除PersistantData目录
        /// </summary>
        [MenuItem("Aya Game Studio/Data/Save Data/Clear PlayerPrefs (Save Data)", false)]
        static void DeleteAllData()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Delete all PlayerPrefs success!");
        }
    }
}
#endif