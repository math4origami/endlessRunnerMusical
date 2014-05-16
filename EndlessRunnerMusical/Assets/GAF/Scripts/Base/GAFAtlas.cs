/*
 * File:           GAFAtlas.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GAFAtlas 
{
	#region Members

	private float[] 		m_Csfs;
	private string[] 		m_FileNames;
	private int 			m_ID;
	private Texture2D[] 	m_Textures;

	#endregion // Members

	#region Interface

	public GAFAtlas( string [] _FileNames, float [] _CSF, int _ID )
	{
		m_FileNames 	= _FileNames;
		m_ID 			= _ID;
		m_Csfs 			= _CSF;
		m_Textures 		= new Texture2D[_FileNames.Length];
	}

	public void LoadTextures(List<Texture2D> _Textures)
	{
		string lostTextures = string.Empty;
		for( int i = 0; i < m_FileNames.Length; i++)
		{
			Texture2D texture = _Textures.Find(delegate(Texture2D _texture) 
			{
				return _texture.name == Path.GetFileNameWithoutExtension(m_FileNames[i]);
			});

			if (texture == null)
			{
				lostTextures += m_FileNames[i];
				lostTextures += "\n";
			}

			m_Textures[i] = texture;
		}

		if (lostTextures.Length > 0)
		{
			throw new GAFException("GAFAtlas::InitTextures - Textures not found - " + lostTextures);
		}
	}

	public Texture2D GetTexture(float _CSF)
	{
		int currentIndex = 0;
		for (; currentIndex < m_Csfs.Length; currentIndex++)
			if (_CSF == m_Csfs[currentIndex])
				break;

		return m_Textures[currentIndex];
	}

#if UNITY_EDITOR

	public void ReImportTextures()
	{
		for( int i = 0; i < m_FileNames.Length; i++)
		{
			string assetPath = AssetDatabase.GetAssetPath( m_Textures[i] );
			
			TextureImporter  textureImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;
			if (textureImporter == null)
			{
				throw new GAFException("GAFAtlas::InitTextures - Texture found but importer cannot process it, please try reimport gaf file.");
			}
			
			textureImporter.textureType 	= TextureImporterType.Advanced;
			textureImporter.npotScale 		= TextureImporterNPOTScale.None;
			textureImporter.mipmapEnabled 	= false;
			textureImporter.isReadable 		= true;
			textureImporter.maxTextureSize 	= 2048;
			
#if UNITY_EDITOR_OSX
			textureImporter.textureFormat = TextureImporterFormat.PVRTC_RGBA4;
#else
			textureImporter.textureFormat = TextureImporterFormat.ARGB32;	
#endif // UNITY_EDITOR_OSX
			
			TextureImporterSettings st = new TextureImporterSettings();
			textureImporter.ReadTextureSettings( st );
			st.wrapMode = TextureWrapMode.Clamp;
			textureImporter.SetTextureSettings( st );				
			
			AssetDatabase.ImportAsset( assetPath, ImportAssetOptions.ForceUpdate );	
		}
	}

#endif // UNITY_EDITOR

	#endregion // Interface

	#region Properties

	public string[] FileNames
	{
		get
		{
			return m_FileNames;
		}
		set
		{
			m_FileNames = value;
		}
	}

	public int ID
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

	public float[] CSF
	{
		get
		{
			return m_Csfs;
		}
		set
		{
			m_Csfs = value;
		}
	}

	#endregion // Properties
}
