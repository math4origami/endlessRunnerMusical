/*
 * File:           GAFAssetPostProcessor.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GAFAssetPostProcessor : AssetPostprocessor
{
    public static void OnPostprocessAllAssets(
		  string[] importedAssets
		, string[] deletedAssets
		, string[] movedAssets
		, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (asset.EndsWith(".gaf"))
            {
				string path 		= asset;
				string fileName		= System.IO.Path.GetFileName(path);
				string assetName	= System.IO.Path.GetFileNameWithoutExtension(path);
				string dirName		= path.Remove(path.Length - fileName.Length);

				byte [] fileBytes = null;
				using (BinaryReader freader = new BinaryReader(File.OpenRead(path)))
				{
					fileBytes = freader.ReadBytes((int)freader.BaseStream.Length);
				}

				if (dirName.Contains("Resources"))
				{
					GAFAnimationAsset animationAsset = ScriptableObject.CreateInstance<GAFAnimationAsset>();
					animationAsset.init(dirName.Substring(dirName.IndexOf("Resources") + "Resources".Length + 1), fileBytes);

					if (animationAsset.data != null)
					{
						string newAssetName = dirName + assetName + ".asset";
						GAFAnimationAsset oldAsset = AssetDatabase.LoadAssetAtPath(newAssetName, typeof(GAFAnimationAsset)) as GAFAnimationAsset;
						if (oldAsset != null)
						{
							EditorUtility.CopySerialized( animationAsset, oldAsset );
							AssetDatabase.SaveAssets();
							AssetDatabase.Refresh(ImportAssetOptions.Default);
						}
						else
						{
							AssetDatabase.CreateAsset(animationAsset, newAssetName);
							AssetDatabase.Refresh(ImportAssetOptions.Default);
						}
					}
				}
				else
				{
					EditorUtility.DisplayDialog(
						  "GAF. Importer warning."
						, "Please move your animation to 'Resources' folder."
						, "OK");
				}
			}
        }
    }
}