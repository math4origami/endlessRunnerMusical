/*
 * File:           TagDefineEnd.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;

public class TagDefineEnd : TagBase
{
	public override void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		_Data.AnimationSequences.Add(new GAFAnimationSequence(
			"Default"
			, 1
			, _Data.AnimationFrames[_Data.AnimationFrames.Count - 1].FrameNumber));
	}
}
