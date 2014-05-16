/*
 * File:           GAFAnimationBehaviour.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public abstract class GAFAnimationBehaviour : MonoBehaviour 
{
	#region Members

	private GAFMovieClip m_Player 		= null;
	private GAFState	 m_CurrentState = null;

	#endregion // Members

	#region Interface

	public virtual void Init()
	{
		m_CurrentState = new GAFState(element.Name);
	}

	#endregion // Interface

	#region Properties

	public GAFMovieClip movieClip
	{
		get
		{
			if (m_Player == null)
			{
				m_Player = transform.parent.GetComponent<GAFMovieClip> ();
			}

			return m_Player;
		}
	}

	public GAFTextureAtlas textureAtlas
	{
		get
		{
			if (m_Player == null)
			{
				m_Player = transform.parent.GetComponent<GAFMovieClip> ();
			}

			return m_Player.asset.getTextureAtlas(m_Player.settings.scale);
		}
	}

	public GAFElement element
	{
		get
		{
			return textureAtlas.GetElementByName(gameObject.name.Split ('_') [0]);
		}
	}

	public GAFAtlas atlas
	{
		get
		{
			return textureAtlas.GetAtlasByID(element.AtlasID);
		}
	}

	public GAFState currentState
	{
		get
		{
			return m_CurrentState;
		}
	}

	#endregion // Properties

	#region Implementation

	protected void setCurrentState(GAFState _State)
	{
		m_CurrentState = _State;
	}

	#endregion // Implementation
}