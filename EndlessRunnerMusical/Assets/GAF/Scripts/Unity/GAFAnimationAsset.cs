using UnityEngine;
using System.Collections.Generic;

public class GAFAnimationAsset : ScriptableObject 
{
	#region Members

	[HideInInspector][SerializeField] private string 	m_AnimationDir 	= string.Empty;
	[HideInInspector][SerializeField] private byte [] 	m_AssetData		= null;
	
	private GAFAnimationData m_Data = null;

	#endregion // Members

	#region Interface

#if UNITY_EDITOR

	public void init(string _AnimationDir, byte [] _Data)
	{
		m_AnimationDir 	= _AnimationDir;
		m_AssetData		= _Data;

		reloadData();
	}

	public void reimportTextures()
	{
		foreach(GAFTextureAtlas textureAtlas in m_Data.TextureAtlases)
		{
			foreach(GAFAtlas atlas in textureAtlas.Atlases)
			{
				try
				{
					atlas.ReImportTextures();
				}
				catch (GAFException _Exception)
				{
					GAFUtils.Error(_Exception.Message);
					return;
				}
			}
		}
	}

#endif // UNITY_EDITOR

	public void reloadData()
	{
		if (m_AssetData != null)
		{
			GAFReader reader = new GAFReader();
			try
			{
				m_Data = reader.Load(m_AssetData);
			}
			catch (GAFException _Exception)
			{
				GAFUtils.Error(_Exception.Message);
			}
		}
	}
	
	public void reloadTextures()
	{
		List<Texture2D> allTextures = new List<Texture2D> (Resources.LoadAll<Texture2D> (animationPath));
		foreach(GAFTextureAtlas textureAtlas in m_Data.TextureAtlases)
		{
			foreach(GAFAtlas atlas in textureAtlas.Atlases)
			{
				try
				{
					atlas.LoadTextures(allTextures);
				}
				catch (GAFException _Exception)
				{
					GAFUtils.Error(_Exception.Message);
					return;
				}
			}
		}
	}

	#endregion // Interface

	#region Properties

	public string animationPath
	{
		get
		{
			return m_AnimationDir;
		}
	}

	public GAFAnimationData data
	{
		get
		{
			return m_Data;
		}
	}

	public GAFElement getElementByName(string _Name, float _Scale)
	{
		foreach (GAFTextureAtlas textureAtlas in data.TextureAtlases)
		{
			if (textureAtlas.Scale == _Scale)
			{
				foreach (GAFElement element in textureAtlas.Elements)
				{
					if (element.Name == _Name)
					{
						return element;
					}
				}
			}
		}
		
		return null;
	}
	
	public GAFTextureAtlas getTextureAtlas(float _Scale)
	{
		foreach( GAFTextureAtlas textureAtlas in data.TextureAtlases )
		{
			if (textureAtlas.Scale == _Scale)
			{
				return textureAtlas;
			}
		}
		
		return null;
	}

	#endregion // Properties
}
