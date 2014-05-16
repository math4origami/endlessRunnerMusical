/*
 * File:           GAFTransform.cs
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
public class GAFTransform : GAFAnimationBehaviour 
{
	#region Members

	protected Vector3[] m_Vertices = new Vector3[4];

	#endregion // Members

	#region Interface

	public override void Init()
	{
		base.Init ();

		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter == null)
			filter = gameObject.AddComponent<MeshFilter> ();
		
		string elementName = gameObject.name.Split ('_') [0];

		float scale = /*movieClip.settings.scale */ element.Scale * movieClip.settings.pixelsPerUnit;
		float scaledPivotX 	= (float)element.PivotX / scale;
		float scaledPivotY 	= (float)element.PivotY / scale;
		float scaledWidth 	= (float)element.Width  / scale;
		float scaledHeight 	= (float)element.Height / scale;
		
		m_Vertices[0] = new Vector3(-scaledPivotX				, scaledPivotY - scaledHeight	, 0f);
		m_Vertices[1] = new Vector3(-scaledPivotX				, scaledPivotY					, 0f);
		m_Vertices[2] = new Vector3(-scaledPivotX + scaledWidth	, scaledPivotY					, 0f);
		m_Vertices[3] = new Vector3(-scaledPivotX + scaledWidth	, scaledPivotY - scaledHeight	, 0f);

		Texture2D atlasTexture = atlas.GetTexture (movieClip.settings.csf);
		float scaledElementLeftX 	= (float) element.X * movieClip.settings.csf																	/ atlasTexture.width;
		float scaledElementRightX 	= (float)(element.X + element.Width) * movieClip.settings.csf													/ atlasTexture.width;
		float scaledElementTopY 	= (float)(atlasTexture.height - element.Y * movieClip.settings.csf - element.Height * movieClip.settings.csf)	/ atlasTexture.height;
		float scaledElementBottomY	= (float)(atlasTexture.height - element.Y * movieClip.settings.csf)												/ atlasTexture.height;

		Vector2 [] uv = new Vector2[4];
		uv [0] = new Vector2 (scaledElementLeftX	, scaledElementTopY);
		uv [1] = new Vector2 (scaledElementLeftX	, scaledElementBottomY);
		uv [2] = new Vector2 (scaledElementRightX	, scaledElementBottomY);
		uv [3] = new Vector2 (scaledElementRightX	, scaledElementTopY);
		
		Vector3 [] normals = new Vector3[4];
		normals[0] = new Vector3(0f, 0f, -1f);
		normals[1] = new Vector3(0f, 0f, -1f);
		normals[2] = new Vector3(0f, 0f, -1f);
		normals[3] = new Vector3(0f, 0f, -1f);
		
		int [] triangles = new int[6];
		triangles[0] = 2;
		triangles[1] = 0;
		triangles[2] = 1;
		triangles[3] = 3;
		triangles[4] = 0;
		triangles[5] = 2;
		
		Mesh mesh = new Mesh ();
		mesh.name = "Element_" + elementName;
		
		mesh.vertices 	= m_Vertices;
		mesh.uv 		= uv;
		mesh.triangles 	= triangles;
		mesh.normals 	= normals;
		
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		
		filter.mesh = mesh;
	}

	public virtual void UpdateTransform(GAFState _State, bool _Force = false)
	{
		transform.localRotation = Quaternion.identity;
		transform.localScale 	= Vector3.one;

		if (_Force ||
			currentState.Tx 	!= _State.Tx ||
		    currentState.Ty 	!= _State.Ty ||
		    currentState.ZOrder != _State.ZOrder)
		{
			float scale = movieClip.settings.pixelsPerUnit / movieClip.settings.scale;
			transform.localPosition = new Vector3(_State.Tx / scale, -_State.Ty / scale, -_State.ZOrder / scale);
		}

		if (_Force ||
		    currentState.A != _State.A  ||
		    currentState.B != _State.B  ||
		    currentState.C != _State.C  ||
		    currentState.D != _State.D)
		{
			Matrix4x4 _transform = Matrix4x4.identity;
			_transform[0, 0] =  _State.A;
			_transform[0, 1] = -_State.C;
			_transform[1, 0] = -_State.B;
			_transform[1, 1] =  _State.D;
			
			Vector3 [] vertices = new Vector3[m_Vertices.Length];
			for(int i = 0; i< vertices.Length; i++)
				vertices[i] = _transform * m_Vertices[i];

			MeshFilter filter = GetComponent<MeshFilter>();
			filter.sharedMesh.vertices = vertices;
			filter.sharedMesh.RecalculateBounds();
		}

		setCurrentState(_State);
	}

	#endregion // Interface
}
