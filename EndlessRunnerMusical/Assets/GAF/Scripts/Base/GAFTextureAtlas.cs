/*
 * File:           GAFTextureAtlas.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;
using System.IO;

public class GAFTextureAtlas
{
    #region Members

    private float 			m_Scale;
    private GAFElement[] 	m_Elements;
    private GAFAtlas[] 		m_Atlases;

    #endregion

    #region Interface

    public GAFTextureAtlas(float _Scale, GAFElement[] _Elements, GAFAtlas[] _Atlases)
    {
        m_Scale 	= _Scale;
        m_Elements 	= _Elements;
        m_Atlases 	= _Atlases;
    }
	
    public GAFElement GetElementByName(string _ElementName)
    {
        foreach (GAFElement element in m_Elements)
        {
			if (element.Name == _ElementName)
            {
				return element;
            }
        }

        return null;
    }
	
    public GAFAtlas GetAtlasByID(int _ID)
    {
        foreach (GAFAtlas atlas in m_Atlases)
        {
			if (atlas.ID == _ID)
            {
				return atlas;
            }
        }

        return null;
    }
	
	#endregion // Interface

    #region Properties

    public GAFElement[] Elements
    {
        get
        {
            return m_Elements;
        }
        set
        {
            m_Elements = value;
        }

    }

    public GAFAtlas[] Atlases
    {
        get
        {
            return m_Atlases;
        }
        set
        {
            m_Atlases = value;
        }
    }
	
    public float Scale
    {
        get
        {
            return m_Scale;
        }
    }

	#endregion // Properties
}
