/*
 * File:           TagDefineNamedParts.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TagDefineNamedParts : TagBase 
{
	public override void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		uint count = _GAFFileReader.ReadUInt32();
		for (uint i = 0; i < count; ++i)
		{
			uint 	objectIdRef = _GAFFileReader.ReadUInt32();
			string 	name		= GAFReader.ReadString(_GAFFileReader);

			_Data.NamedParts.Add(new GAFNamedPart(objectIdRef.ToString(), name));
		}
	}
}
