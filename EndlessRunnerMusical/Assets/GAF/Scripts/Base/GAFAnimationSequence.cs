/*
 * File:           GAFAnimationSequence.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public class GAFAnimationSequence 
{
	#region Members
	
	private string m_ID;
	private uint m_StartFrame;
	private uint m_EndFrame;
	
	#endregion // Members
	
	#region Interface
	
	public GAFAnimationSequence( string _ID, uint _StartFrame, uint _EndFrame )
	{
		m_ID 			= _ID;
		m_StartFrame 	= _StartFrame;
		m_EndFrame 		= _EndFrame;
	}

	public override string ToString ()
	{
		return string.Format ("[AnimationSequence: Id={0}, StartFrame={1}, EndFrame={2}]", ID, StartFrame, EndFrame);
	}

	#endregion // Interface

	#region Properties

	public string ID
	{
		get
		{
			return m_ID;
		}

		set
		{
			m_ID = value;
		}
	}

	public uint StartFrame
	{
		get
		{
			return m_StartFrame;
		}

		set
		{
			m_StartFrame = value;
		}
	}

	public uint EndFrame
	{
		get
		{
			return m_EndFrame;
		}

		set
		{
			m_EndFrame = value;
		}
	}
	
	#endregion // Properties
}
