/*
 * File:           GAFAnimationData.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAFAnimationData
{
	#region SerializedMembers

	private List<GAFTextureAtlas> 		m_TextureAtlases 		= new List<GAFTextureAtlas>();
	private List<GAFAnimationSequence> 	m_AnimationSequences 	= new List<GAFAnimationSequence>();
	private List<GAFAnimationObject> 	m_AnimationObjects 		= new List<GAFAnimationObject>();
	private List<GAFAnimationFrame> 	m_AnimationFrames 		= new List<GAFAnimationFrame>();
	private List<GAFNamedPart> 			m_NamedParts 			= new List<GAFNamedPart>();
	private List<GAFAnimationMask> 		m_AnimationMasks 		= new List<GAFAnimationMask>();
	private uint 						m_FramesCount 			= 0;
	private ushort 						m_Version 				= 0;
	private Rect 						m_FrameSize;
	private Vector2 					m_Pivot;

	#endregion // SerializedMembers

	#region Properties

	public List<GAFTextureAtlas> TextureAtlases
	{
		get
		{
			return m_TextureAtlases;
		}

		set
		{
			m_TextureAtlases = value;
		}
	}
	
	public List<GAFAnimationObject> AnimationObjects
	{
		get
		{
			return m_AnimationObjects;
		}

		set
		{
			m_AnimationObjects = value;
		}
	}

	public List<GAFAnimationMask> AnimationMasks
	{
		get
		{
			return m_AnimationMasks;
		}
		
		set
		{
			m_AnimationMasks = value;
		}
	}

	public List<GAFAnimationFrame> AnimationFrames
	{
		get
		{
			return m_AnimationFrames;
		}

		set
		{
			m_AnimationFrames = value;
		}
	}
	
	public List<GAFAnimationSequence> AnimationSequences
	{
		get
		{
			return m_AnimationSequences;
		}

		set
		{
			m_AnimationSequences = value;
		}
	}

	public List<GAFNamedPart> NamedParts
	{
		get
		{
			return m_NamedParts;
		}

		set
		{
			m_NamedParts = value;
		}
	}

	public uint FramesCount
	{
		get
		{
			return m_FramesCount;
		}

		set
		{
			m_FramesCount = value;
		}
	}

	public ushort Version
	{
		get
		{
			return m_Version;
		}

		set
		{
			m_Version = value;
		}
	}

	public Rect FrameSize
	{
		get
		{
			return m_FrameSize;
		}

		set
		{
			m_FrameSize = value;
		}
	}

	public Vector2 Pivot
	{
		get
		{
			return m_Pivot;
		}

		set
		{
			m_Pivot = value;
		}
	}

	#endregion // Properties
}
