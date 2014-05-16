/*
 * File:           GAFMaskBehaviour.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("")]
[ExecuteInEditMode]
public class GAFMaskBehaviour : GAFAnimationBehaviour 
{
	#region Members

	private Texture2D 		m_MaskTexture 	= null;
	private GAFTransform 	m_Transform 	= null;

	#endregion // Members

	#region Interface

	public override void Init()
	{
		base.Init ();

		InitTexture ();

		m_Transform = GetComponent<GAFTransform> ();
	}

	#endregion // Interface

	#region Properties

	public Texture2D texture
	{
		get
		{
			if (m_MaskTexture == null)
			{
				InitTexture();
			}

			return m_MaskTexture;
		}
	}

	new public GAFState currentState
	{
		get
		{
			if (m_Transform == null)
			{
				m_Transform = GetComponent<GAFTransform> ();
			}

			return m_Transform.currentState;
		}
	}

	#endregion // Properties

	#region Implementation

	private void InitTexture()
	{
		m_MaskTexture = new Texture2D(element.Width * (int)movieClip.settings.csf, element.Height * (int)movieClip.settings.csf, TextureFormat.ARGB32, false);

		Color [] textureColor = texture.GetPixels ();
		for (uint i = 0; i < textureColor.Length; ++i)
			textureColor [i] = Color.black;
		
		m_MaskTexture.SetPixels( textureColor );
		m_MaskTexture.Apply();

		Texture2D atlasTexture = atlas.GetTexture (movieClip.settings.csf);
		Color [] maskTexturePixels = atlasTexture.GetPixels (
			  element.X * (int)movieClip.settings.csf
			, atlasTexture.height - element.Y * (int)movieClip.settings.csf - element.Height * (int)movieClip.settings.csf
			, element.Width * (int)movieClip.settings.csf
			, element.Height * (int)movieClip.settings.csf);

		m_MaskTexture.SetPixels(
			  0
			, 0
			, element.Width * (int)movieClip.settings.csf
			, element.Height *  (int)movieClip.settings.csf
			, maskTexturePixels);
		
		m_MaskTexture.Apply(true);
		
		m_MaskTexture.filterMode 	= FilterMode.Bilinear;
		m_MaskTexture.wrapMode 		= TextureWrapMode.Clamp;
		
		m_MaskTexture.Apply();
	}

	#endregion // Implementation
}
