// Copyright (c) 2024, Awessets

using System.IO;
using UnityEditor;
using UnityEngine;

namespace MergeIt.Editor.Helpers
{
    public class SavesHelper : UnityEditor.Editor
    {
        [MenuItem("Tools/Merge Toolkit/Clear saves", false, 4)]
        public static void ClearSaves()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Saves");

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                string meta = Path.ChangeExtension(path, "meta");

                if (File.Exists(meta))
                {
                    File.Delete(meta);
                }
                
                AssetDatabase.Refresh();
            }
        }
    }
}