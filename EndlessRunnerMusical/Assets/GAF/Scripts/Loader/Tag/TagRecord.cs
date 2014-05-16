/*
 * File:           TagRecord.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;

public class TagRecord
{
	public long ExpectedStreamPosition
	{
		get
		{
			return m_ExpectedStreamPos;
		}
		set
		{
			m_ExpectedStreamPos = value;
		}
	}
	
	public long TagSize
	{
		get
		{
			return m_TagSize;
		}
		set
		{
			m_TagSize = value;
		}
	}
	
	
	public TagBase.TagType Type
	{
		get
		{
			return m_TagType;
		}
		set
		{
			m_TagType = value;
		}
	}
	
	private long 				m_ExpectedStreamPos;
	private long 				m_TagSize;
	private TagBase.TagType 	m_TagType;
}
