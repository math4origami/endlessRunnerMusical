/*
 * File:           GAFHeader.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using System.IO;
using UnityEngine;

[System.Serializable]
public class GAFHeader 
{
	#region Enums

	public enum CompressionType
	{
		__CompressionDefault 	= 0 			// Internal
		, CompressedNone 		= 0x00474146	// GAF
		, CompressedZip 		= 0x00474143,  	// GAC
	};

	#endregion // Enums
	
	#region Members
	
	[HideInInspector][SerializeField] private int 		m_Compression;
	[HideInInspector][SerializeField] private int 		m_FileLength;
	[HideInInspector][SerializeField] private short 	m_Version;
	
	#endregion

	#region Interface

	public GAFHeader()
	{
	}

	public void Read(BinaryReader _Reader)
	{
		m_Compression = _Reader.ReadInt32();
		if (!IsValid)
			return;

		m_Version		= (short)_Reader.ReadUInt16();
		m_FileLength	= (int)	 _Reader.ReadUInt32();
	}

	public bool IsValid
	{
		get
		{
			return 	m_Compression == (int) CompressionType.CompressedNone ||
					m_Compression == (int) CompressionType.CompressedZip;
		}
	}

	public CompressionType Compression
	{
		get
		{
			return (CompressionType)m_Compression;
		}
	}

	public ushort Version
	{
		get
		{
			return (ushort)m_Version;
		}
	}

	public uint FileLength
	{
		get
		{
			return (uint)m_FileLength;
		}
	}

	public static int HeaderDataOffset
	{
		get
		{
			return sizeof(int) + sizeof(uint) + sizeof(ushort);
		}
	}

	#endregion
}
