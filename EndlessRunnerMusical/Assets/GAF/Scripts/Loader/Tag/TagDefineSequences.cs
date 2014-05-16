/*
 * File:           TagDefineSequences.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TagDefineSequences : TagBase 
{
	public override void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		uint count = _GAFFileReader.ReadUInt32();
		
		for (uint i = 0; i < count; ++i)
		{
			string id = GAFReader.ReadString(_GAFFileReader);

			ushort start 	= _GAFFileReader.ReadUInt16();
			ushort end 		= _GAFFileReader.ReadUInt16();

			_Data.AnimationSequences.Add(new GAFAnimationSequence(id, start, end));
		}
	}
}
