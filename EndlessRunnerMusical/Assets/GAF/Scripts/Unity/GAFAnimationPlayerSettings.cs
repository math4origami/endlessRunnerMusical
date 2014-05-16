/*
 * File:           GAFAnimationPlayerSettings.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

public enum GAFWrapMode
{
	  Once
	, Loop
}

[System.Serializable]
public class GAFAnimationPlayerSettings 
{
	#region Members

	[HideInInspector][SerializeField] private float 				m_Scale				= 1f;
	[HideInInspector][SerializeField] private float 				m_CSF				= 1f;
	[HideInInspector][SerializeField] private float 				m_PixelsPerUnit		= 1f;
	[HideInInspector][SerializeField] private bool 					m_PlayAutomatically = true;
	[HideInInspector][SerializeField] private bool 					m_IgnoreTimeScale 	= false;
	[HideInInspector][SerializeField] private bool 					m_PerfectTiming		= true;
	[HideInInspector][SerializeField] private bool 					m_PlayInBackground	= false;
	[HideInInspector][SerializeField] private GAFWrapMode 			m_WrapMode 			= GAFWrapMode.Once;
	[HideInInspector][SerializeField] private int 					m_TargetFPS 		= 30;

	#endregion // Members

	#region Properties

	public float scale
	{
		get
		{
			return m_Scale;
		}

		set
		{
			m_Scale = value;
		}
	}

	public float csf
	{
		get
		{
			return m_CSF;
		}

		set
		{
			m_CSF = value;
		}
	}

	public float pixelsPerUnit
	{
		get
		{
			return m_PixelsPerUnit;
		}

		set
		{
			m_PixelsPerUnit = value;
		}
	}

	public bool playAutomatically
	{
		get
		{
			return m_PlayAutomatically;
		}

		set
		{
			m_PlayAutomatically = value;
		}
	}

	public bool ignoreTimeScale
	{
		get
		{
			return m_IgnoreTimeScale;
		}

		set
		{
			m_IgnoreTimeScale = value;
		}
	}

	public bool perfectTiming
	{
		get
		{
			return m_PerfectTiming;
		}
		
		set
		{
			m_PerfectTiming = value;
		}
	}

	public bool playInBackground
	{
		get
		{
			return m_PlayInBackground;
		}

		set
		{
			m_PlayInBackground = value;
		}
	}

	public GAFWrapMode wrapMode
	{
		get
		{
			return m_WrapMode;
		}

		set
		{
			m_WrapMode = value;
		}
	}

	public uint targetFPS
	{
		get
		{
			return (uint)m_TargetFPS;
		}

		set
		{
			m_TargetFPS = (int)value;
		}
	}

	public float targetSPF
	{
		get
		{
			return 1f / m_TargetFPS;
		}
	}

	#endregion // Properties
}
