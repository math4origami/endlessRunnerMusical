/*
 * File:           GAFNamedPart.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFNamedPart 
{
	#region Members

	private string m_PartID;
	private string m_Name;

	#endregion // Members

	#region Interface
	
	public GAFNamedPart( string _PartID, string _Name )
	{
		m_PartID 	= _PartID;
		m_Name 		= _Name;
	}

	public override string ToString ()
	{
		return string.Format(
			"[NamedPart: PartId={0}, Name={1}]"
			, PartID
			, Name);
	}

	#endregion // Interface
	
	#region Properties

	public string PartID
	{
		get
		{
			return m_PartID;
		}
	}

	public string Name
	{
		get
		{
			return m_Name;
		}
	}

	#endregion // Properties
}
