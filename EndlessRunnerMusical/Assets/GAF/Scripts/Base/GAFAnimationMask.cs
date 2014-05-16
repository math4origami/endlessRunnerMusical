/*
 * File:           GAFAnimationMask.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFAnimationMask 
{	
	#region Members

	private string m_ObjectName;
	private string m_MaskName;

	#endregion // Members

	#region Interface
	
	public GAFAnimationMask( string _ObjectName, string _MaskName )
	{
		m_ObjectName 	= _ObjectName;
		m_MaskName 		= _MaskName;
	}

	#endregion // Interface
	
	#region Properties

	public string ObjectName
	{
		get
		{
			return m_ObjectName;
		}
	}

	public string MaskName
	{
		get
		{
			return m_MaskName;
		}
	}

	#endregion
}
