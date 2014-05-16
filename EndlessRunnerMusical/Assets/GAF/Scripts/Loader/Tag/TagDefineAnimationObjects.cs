/*
 * File:           TagDefineAnimationObjects.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TagDefineAnimationObjects : TagBase 
{
	public override void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		uint count = _GAFFileReader.ReadUInt32();
		
		for (uint i = 0; i < count; ++i)
		{
			uint objectId 			= _GAFFileReader.ReadUInt32();
			uint elementAtlasIdRef	= _GAFFileReader.ReadUInt32();

			_Data.AnimationObjects.Add(new GAFAnimationObject(objectId.ToString(), elementAtlasIdRef.ToString()));
		}
	}
}
