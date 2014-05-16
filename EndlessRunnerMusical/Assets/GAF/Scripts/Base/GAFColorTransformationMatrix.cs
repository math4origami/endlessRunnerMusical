/*
 * File:           GAFColorTransformationMatrix.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFColorTransformationMatrix 
{	
	#region Members

	private float m_MultiplierR;
	private float m_MultiplierG;
	private float m_MultiplierB;
	private float m_MultiplierA;
	private float m_OffsetR;
	private float m_OffsetG;
	private float m_OffsetB;
	private float m_OffsetA;	

	#endregion // Members

	#region Interface 
	
	public GAFColorTransformationMatrix()
	{
		this.Multipliers 	= Color.white;
		this.Offsets 		= Color.black;
		m_OffsetA 			= 0f;
	}
	
	public GAFColorTransformationMatrix( Color _Multipliers, Color _Offsets )
	{
		this.Multipliers 	= _Multipliers;
		this.Offsets 		= _Offsets;
	}
	
	public override string ToString ()
	{
		return string.Format(
			"[ColorTranformationMatrix: Multipliers={0}, Offsets={1}]"
			, Multipliers
			, Offsets);
	}
	
	#endregion // Interface
	
	#region Properties

	public Color Multipliers
	{
		get
		{			
			return new Color( m_MultiplierR, m_MultiplierG, m_MultiplierB, m_MultiplierA );
		}
		set
		{
			m_MultiplierR = value.r;
			m_MultiplierG = value.g;
			m_MultiplierB = value.b;
			m_MultiplierA = value.a;
		}
	}

	public Color Offsets
	{
		get
		{
			return new Color( m_OffsetR, m_OffsetG, m_OffsetB, m_OffsetA );
		}
		set
		{
			m_OffsetR = value.r;
			m_OffsetG = value.g;
			m_OffsetB = value.b;
			m_OffsetA = value.a;
		}
	}

	#endregion // Properties
}
