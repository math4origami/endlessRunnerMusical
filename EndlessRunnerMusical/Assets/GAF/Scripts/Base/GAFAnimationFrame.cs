/*
 * File:           GAFAnimationFrame.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GAFAnimationFrame 
{	
	#region Members
	
	private uint 			m_FrameNumber;
	private List<GAFState> 	m_States;

	#endregion // Members
	
	#region Interface

	public GAFAnimationFrame( uint _FrameNumber )
	{
		m_FrameNumber = _FrameNumber;

		m_States = new List<GAFState>();
	}	

	public void AddState( GAFState state )
	{
		m_States.Add( state );
	}

	#endregion // Interface
	
	#region Properties

	public uint FrameNumber
	{
		get
		{
			return m_FrameNumber;
		}
	}

	public List<GAFState> States
	{
		get
		{
			return m_States;
		}
	}

	#endregion // Properties
}
