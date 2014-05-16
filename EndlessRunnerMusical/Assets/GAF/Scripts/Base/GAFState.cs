/*
 * File:           GAFState.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFState 
{	
	#region Members	

	private string 	m_Name 				= string.Empty;
	private int 	m_ZOrder			= 0;
	private float 	m_A					= 0f;
	private float 	m_B					= 0f;
	private float 	m_C					= 0f;
	private float  	m_D					= 0f;
	private float 	m_Tx				= 0f;
	private float 	m_Ty				= 0f;
	private float 	m_Alpha				= 0f;
	private string 	m_MaskID 			= string.Empty;
	private float 	m_HorizontalBlur 	= 0f;
	private float  	m_VerticalBlur 		= 0f;
	private GAFColorTransformationMatrix m_ColorTransformationMatrix;
	
	#endregion // Members
	
	#region Interface
	
	public GAFState(string _Name)
	{
		m_Name = _Name;
	}
	
	public override string ToString ()
	{
		return string.Format(
			"[State: Name={0}, ZOrder={1}, a={2}, b={3}, c={4}, d={5}, tx={6}, ty={7}, Alpha={8}, MaskId={9}, HorizontalBlur={10}, VerticalBlur={11}, ColorTansformaitonMtx={12}]"
			, Name
			, ZOrder
			, A
			, B
			, C
			, D
			, Tx
			, Ty
			, Alpha
			, MaskID
			, HorizontalBlur
			, VerticalBlur
			, ColorTansformationMatrix);
	}
	
	#endregion // Interface
	
	#region Properties

	public string Name
	{
		get
		{
			return m_Name;
		}

		set
		{
			m_Name = value;
		}
	}

	public int ZOrder
	{
		get
		{
			return m_ZOrder;
		}

		set
		{
			m_ZOrder = value;
		}
	}

	public float A
	{
		get
		{
			return m_A;
		}

		set
		{
			m_A = value;
		}
	}

	public float B
	{
		get
		{
			return m_B;
		}

		set
		{
			m_B = value;
		}
	}

	public float C
	{
		get
		{
			return m_C;
		}

		set
		{
			m_C = value;
		}
	}

	public float D
	{
		get
		{
			return m_D;
		}
		set
		{
			m_D = value;
		}
	}

	public float Tx
	{
		get
		{
			return m_Tx;
		}

		set
		{
			m_Tx = value;
		}
	}

	public float Ty
	{
		get
		{
			return m_Ty;
		}

		set
		{
			m_Ty = value;
		}
	}

	public float Alpha
	{
		get
		{
			return m_Alpha;
		}

		set
		{
			m_Alpha = value;
		}
	}

	public string MaskID
	{
		get
		{
			return m_MaskID;
		}

		set
		{
			m_MaskID = value;
		}
	}

	public float HorizontalBlur
	{
		get
		{
			return m_HorizontalBlur;
		}

		set
		{
			m_HorizontalBlur = value;
		}
	}

	public float VerticalBlur
	{
		get
		{
			return m_VerticalBlur;
		}

		set
		{
			m_VerticalBlur = value;
		}
	}

	public GAFColorTransformationMatrix ColorTansformationMatrix
	{
		get
		{
			return m_ColorTransformationMatrix;
		}
		set
		{
			m_ColorTransformationMatrix = value;
		}
	}

	#endregion // Properties
}
