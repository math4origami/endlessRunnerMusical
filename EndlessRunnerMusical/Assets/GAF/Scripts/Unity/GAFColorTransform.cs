/*
 * File:           GAFColorTransform.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
[ExecuteInEditMode]
public class GAFColorTransform : GAFAnimationBehaviour 
{
	#region Members

	private static readonly string 	m_sShaderName 		= "GAF/GAFObject";
	private static readonly Color 	m_sDisabledOffset 	= new Color( 0, 0, 0, 0);

	#endregion // Members

	#region Interface

	public override void Init ()
	{
		base.Init ();

		if (renderer == null) 
		{
			gameObject.AddComponent<MeshRenderer> ();
		}

		Material material 		= new Material(Shader.Find(m_sShaderName));
		material.color 			= new Color(1f, 1f, 1f, 1f);
		material.mainTexture	= atlas.GetTexture(movieClip.settings.csf);
		
		renderer.material 		= material;
		renderer.castShadows 	= false;
		renderer.receiveShadows = false;
	}

	public virtual void UpdateColor(GAFState _State, bool _Force = false)
	{
		if (_Force ||
		    currentState.Alpha != _State.Alpha)
		{
			renderer.enabled = _State.Alpha != 0;
			renderer.sharedMaterial.SetFloat("_Alpha", _State.Alpha);
		}

		if (_Force ||
		    currentState.ColorTansformationMatrix != _State.ColorTansformationMatrix)
		{
			renderer.sharedMaterial.SetColor("_ColorMult",  _State.ColorTansformationMatrix != null ? _State.ColorTansformationMatrix.Multipliers 	: Color.white );
			renderer.sharedMaterial.SetColor("_ColorShift", _State.ColorTansformationMatrix != null ? _State.ColorTansformationMatrix.Offsets 		: m_sDisabledOffset);
		}

		setCurrentState(_State);
	}

	#endregion // Interface
}
