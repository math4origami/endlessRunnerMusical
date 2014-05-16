/*
 * File:           GAFElement.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFElement 
{
	#region Members

	private string 	m_Name;
	private int 	m_PivotX;
	private int 	m_PivotY;
	private int 	m_X;
	private int 	m_Y;
	private int 	m_Width;
	private int 	m_Height;
	private int 	m_AtlasID;
	private float 	m_Scale;

	#endregion
	
	#region Interface
	
	public GAFElement(
		  string 	_Name
		, int 		_PivotX
		, int 		_PivotY
		, int 		_X
		, int 		_Y
		, int 		_Width
		, int	 	_Height
		, int 		_AtlasID
		, float 	_Scale)
	{
		m_Name 		= _Name;
		m_PivotX 	= _PivotX;
		m_PivotY 	= _PivotY;
		m_X 		= _X;
		m_Y 		= _Y;
		m_Width 	= _Width;
		m_Height 	= _Height;
		m_AtlasID 	= _AtlasID;
		m_Scale 	= _Scale;
	}

	public override string ToString ()
	{
		return string.Format(
			"[Element: Name={0}, PivotX={1}, PivotY={2}, X={3}, Y={4}, Width={5}, Height={6}, AtlasId={7}]"
			, Name
			, PivotX
			, PivotY
			, X
			, Y
			, Width
			, Height
			, AtlasID);
	}

	#endregion // Interface
	
	#region Properties

	public string Name
	{
		get
		{
			return m_Name;
		}
	}

	public int PivotX
	{
		get
		{
			return m_PivotX;	
		}
	}

	public int PivotY
	{
		get
		{
			return m_PivotY;	
		}
	}

	public int X
	{
		get
		{
			return m_X;	
		}
	}

	public int Y
	{
		get
		{
			return m_Y;
		}
	}

	public int Width
	{
		get
		{
			return m_Width;
		}
	}

	public int Height
	{
		get
		{
			return m_Height;
		}
	}

	public int AtlasID
	{
		get
		{
			return m_AtlasID;
		}
	}
	
	public float Scale
	{
		get
		{
			return m_Scale;
		}
	}

	#endregion // Properties
}
