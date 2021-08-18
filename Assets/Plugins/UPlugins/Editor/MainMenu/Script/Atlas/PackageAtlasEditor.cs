/////////////////////////////////////////////////////////////////////////////
//
//  Script : PackageAtlasEditor.cs
//  Info   : 自动打包图集，为图片根据文件夹添加图集信息
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2016
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using Aya.Util;
using UnityEditor;
using UnityEngine;
using FileUtil = Aya.Util.FileUtil;

namespace Aya.EditorScript
{
    public class PackageAtlasEditor : MonoBehaviour
    {

        /// <summary>
        /// 自动打包
        /// </summary>
        [MenuItem("Aya Game Studio/Atlas/Auto Pack(_MenuUI\\Texture\\..)", false)]
        private static void AutoPackageAtlas()
        {
            // 获取目录下所有文件
            var locPath = "/_MenuUI/Texture/";
            var filePathArray = Directory.GetFiles(Application.dataPath + locPath, "*", SearchOption.AllDirectories);
            var filePathList = new List<string>();
            // 过滤获得制定格式的文件
            foreach (var file in filePathArray)
            {
                if (FileUtil.FileFormatCheck(file, FileType.Picture))
                {
                    filePathList.Add(file);
                }
            }
            // 开始处理
            AssetDatabase.StartAssetEditing();
            var count = filePathList.Count;
            for (var i = 0; i < count; i++)
            {
                // 拼接图集名
                var allPath = filePathList[i].Replace('\\', '/');
                var assetPath = allPath.Substring(allPath.IndexOf("Assets", StringComparison.Ordinal));
                var import = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                var temp = allPath.Split('/');
                var atlasName = "Atlas_" + temp[temp.Length - 2];
                // 设置属性
                import.textureType = TextureImporterType.Sprite;
                import.spritePackingTag = atlasName;
                // Debug.Log(assetPath + "\t" + atlasName);
            }
            AssetDatabase.StopAssetEditing();
            Debug.Log("Auto set " + count + " sprite atlas tag.");
            // AutoPackageAtlas();
        }
    }
}

#endif