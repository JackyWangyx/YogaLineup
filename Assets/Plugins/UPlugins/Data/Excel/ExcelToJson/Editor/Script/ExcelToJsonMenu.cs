/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ExcelToJsonMenu.cs
//  Info     : Excel To Json 菜单
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEditor;

namespace Aya.Data.Excel
{
    public static class ExcelToJsonMenu
    {
        [MenuItem("Aya Game Studio/Data/Excel/Export Json", false, 1000)]
        public static void ExportJson()
        {
            ExcelToJsonSetting.Ins.ExportJson();
        }

        [MenuItem("Aya Game Studio/Data/Excel/Export C#", false, 1000)]
        public static void ExportCSharp()
        {
            ExcelToJsonSetting.Ins.ExportCSharp();
        }

        [MenuItem("Aya Game Studio/Data/Excel/Export Json + C#", false, 1000)]
        public static void ExportJsonAndCSharp()
        {
            ExcelToJsonSetting.Ins.ExportJson();
            ExcelToJsonSetting.Ins.ExportCSharp();
        }

        [MenuItem("Aya Game Studio/Data/Excel/Open Input Excel Path", false, 2000)]
        public static void OpenInputExcelPath()
        {
            ExcelToJsonSetting.Ins.OpenInputExcelPath();
        }

        [MenuItem("Aya Game Studio/Data/Excel/Open 0utput Json Path", false, 2001)]
        public static void OpenOutputJsonPath()
        {
            ExcelToJsonSetting.Ins.OpenOutputJsonPath();
        }

        [MenuItem("Aya Game Studio/Data/Excel/Open 0utput C# Path", false, 2002)]
        public static void OpenOutputCSharpPath()
        {
            ExcelToJsonSetting.Ins.OpenOutputCSharpPath();
        }

        [MenuItem("Aya Game Studio/Data/Excel/Setting", false, 3000)]
        public static void Setting()
        {
            EditorGUIUtility.PingObject(ExcelToJsonSetting.Ins);
            Selection.activeObject = ExcelToJsonSetting.Ins;
        }
    }
}
#endif