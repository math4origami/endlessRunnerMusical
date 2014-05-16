/*
 * File:           TagDefineAtlas.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TagDefineAtlas : TagBase 
{
	public override void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		float scale = _GAFFileReader.ReadSingle();

		List<GAFAtlas> 		atlasInfos 		= new List<GAFAtlas>();
		List<GAFElement>	atlasElements	= new List<GAFElement>();

		byte atlasesCount = _GAFFileReader.ReadByte();
		for (byte i = 0; i < atlasesCount; ++i)
		{	
			uint id = _GAFFileReader.ReadUInt32();
			
			byte sourcesCount = _GAFFileReader.ReadByte();

			List<string> 	sources = new List<string>();
			List<float>		csfs	= new List<float>();

			for (byte j = 0; j < sourcesCount; ++j)
			{
				sources.Add(GAFReader.ReadString(_GAFFileReader));
				csfs.Add(_GAFFileReader.ReadSingle());
			}

			atlasInfos.Add(new GAFAtlas(sources.ToArray(), csfs.ToArray(), (int)id));
		}

		uint elementsCount = _GAFFileReader.ReadUInt32();
		for (uint i = 0; i < elementsCount; ++i)
		{
			Vector2 pivotPoint 		= GAFReader.ReadVector2(_GAFFileReader);
			Vector2 origin			= GAFReader.ReadVector2(_GAFFileReader);
			float 	elementScale	= _GAFFileReader.ReadSingle();
			float 	width			= _GAFFileReader.ReadSingle();
			float 	height			= _GAFFileReader.ReadSingle();
			uint 	atlasIdx		= _GAFFileReader.ReadUInt32();
			uint 	elementAtlasIdx	= _GAFFileReader.ReadUInt32();

			atlasElements.Add(new GAFElement(
				  elementAtlasIdx.ToString()
				, (int)pivotPoint.x
				, (int)pivotPoint.y
				, (int)origin.x
				, (int)origin.y
				, (int)width
				, (int)height
				, (int)atlasIdx
				, elementScale));
		}

		_Data.TextureAtlases.Add (new GAFTextureAtlas (scale, atlasElements.ToArray (), atlasInfos.ToArray ()));
	}
}
