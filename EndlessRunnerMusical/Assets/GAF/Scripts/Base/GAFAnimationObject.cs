/*
 * File:           GAFAnimationObject.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFAnimationObject {
	
	#region Members

	private string m_ObjectName;
	private string m_AtlasElementName;

	#endregion // Members
	
	#region Interface 
	
	public GAFAnimationObject( string _ObjectName, string _AtlasElementName )
	{
		m_ObjectName 		= _ObjectName;
		m_AtlasElementName 	= _AtlasElementName;
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

	public string AtlasElementName
	{
		get
		{
			return m_AtlasElementName;
		}
	}

	#endregion // Properties
}
