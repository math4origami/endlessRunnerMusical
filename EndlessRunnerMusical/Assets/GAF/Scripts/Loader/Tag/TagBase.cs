/*
 * File:           TagBase.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;

public abstract class TagBase 
{
	#region enums

	public enum TagType
	{
		  TagInvalid				= -1
		, TagEnd 					= 0
		, TagDefineAtlas 			= 1
		, TagDefineAnimationMasks 	= 2
		, TagDefineAnimationObjects = 3
		, TagDefineAnimationFrames 	= 4
		, TagDefineNamedParts 		= 5
		, TagDefineSequences 		= 6
		//, TagDefineTextFields 	= 7
		//, TagDefineAtlas2 		= 8
		//, TagDefineStage 			= 9
	}

	#endregion

	#region interface

	public abstract void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data);

	#endregion
}
