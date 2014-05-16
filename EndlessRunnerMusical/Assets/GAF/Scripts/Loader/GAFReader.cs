/*
 * File:           GAFReader.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

#define GAF_SUPPORT_COMPRESSED

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

#if GAF_SUPPORT_COMPRESSED
using Ionic.Zlib;
#endif // GAF_SUPPORT_COMPRESSED

public class GAFReader 
{	
	#region Interface
	
	public GAFAnimationData Load(byte [] _AssetData)
	{
		GAFHeader 			header 	= new GAFHeader ();
		GAFAnimationData 	data 	= new GAFAnimationData ();

		MemoryStream fstream = new MemoryStream(_AssetData);
		using (BinaryReader freader = new BinaryReader(fstream))
		{
			if (freader.BaseStream.Length <= GAFHeader.HeaderDataOffset)
				return null;

			header.Read(freader);

			if (header.IsValid)
			{
				data.Version = header.Version;

				switch(header.Compression)
				{
				case GAFHeader.CompressionType.CompressedNone:
					Read(freader, data);
					break;

				case GAFHeader.CompressionType.CompressedZip:
#if GAF_SUPPORT_COMPRESSED
					using (ZlibStream zlibStream = new ZlibStream(fstream, CompressionMode.Decompress))
					{
						byte [] uncompressedBuffer = new byte[header.FileLength];
						zlibStream.Read(uncompressedBuffer, 0, uncompressedBuffer.Length);

						using (BinaryReader reader = new BinaryReader(new MemoryStream(uncompressedBuffer)))
						{
							Read(reader, data);
						}
					}
					break;
#else
					GAFUtils.Assert(false, "GAF. Compressed gaf format is not supported in your plugin!");
					break;
#endif // GAF_SUPPORT_COMPRESSED
				}
			}
		}

		return data;
	}

	#endregion

	#region Static Interface

	public static Vector2 ReadVector2(BinaryReader _Reader)
	{
		Vector2 retVal = new Vector2();
		retVal.x = _Reader.ReadSingle();
		retVal.y = _Reader.ReadSingle();
		return retVal;
	}

	public static Rect ReadRect(BinaryReader _Reader)
	{
		Rect retVal = new Rect();
		retVal.x 		= _Reader.ReadSingle();
		retVal.y 		= _Reader.ReadSingle();
		retVal.width 	= _Reader.ReadSingle();
		retVal.height	= _Reader.ReadSingle();
		return retVal;
	}

	public static string ReadString(BinaryReader _Reader)
	{
		ushort len = _Reader.ReadUInt16();

		byte [] data = _Reader.ReadBytes(len);

		System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
		return enc.GetString(data);
	}

	#endregion

	#region Implementation

	private void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		_Data.FramesCount	= _GAFFileReader.ReadUInt16();
		_Data.FrameSize		= ReadRect(_GAFFileReader);
		_Data.Pivot			= ReadVector2(_GAFFileReader);

		Dictionary<TagBase.TagType, TagBase> tagReaders = new Dictionary<TagBase.TagType, TagBase>();
		tagReaders.Add(TagBase.TagType.TagDefineAtlas				, new TagDefineAtlas());
		tagReaders.Add(TagBase.TagType.TagDefineAnimationMasks		, new TagDefineAnimationMasks());
		tagReaders.Add(TagBase.TagType.TagDefineAnimationObjects	, new TagDefineAnimationObjects());
		tagReaders.Add(TagBase.TagType.TagDefineAnimationFrames		, new TagDefineAnimationFrames());
		tagReaders.Add(TagBase.TagType.TagDefineNamedParts			, new TagDefineNamedParts());
		tagReaders.Add(TagBase.TagType.TagDefineSequences			, new TagDefineSequences());
		//tagReaders.Add(TagBase.TagType.TagDefineTextFields			, new TagDefineTextFields ());
		//tagReaders.Add(TagBase.TagType.TagDefineAtlas2				, new TagDefineAtlas2());
		//tagReaders.Add(TagBase.TagType.TagDefineStage				, new TagDefineStage());
		tagReaders.Add(TagBase.TagType.TagEnd						, new TagDefineEnd());

		while (_GAFFileReader.BaseStream.Position < _GAFFileReader.BaseStream.Length)
		{
			TagRecord record;
			try
			{
				record = OpenTag(_GAFFileReader);
			}
			catch (System.Exception _exception)
			{
				throw new GAFException("GAFReader::Read - Failed to open tag! Stream position - " + _GAFFileReader.BaseStream.Position.ToString() + "\nException - " + _exception);
			}

			if (record.Type != TagBase.TagType.TagInvalid)
			{
				if (tagReaders.ContainsKey(record.Type))
				{
					try
					{
						tagReaders[record.Type].Read(_GAFFileReader, _Data);
					}
					catch
					{
						throw new GAFException("GAFReader::Read - Failed to read tag - " + record.Type.ToString(), record);
					}
					finally
					{
						CheckTag(record, _GAFFileReader);
					}

					CheckTag(record, _GAFFileReader);
				}
				else
				{
					throw new GAFException("GAFReader::Read - Tag is present in enum but not defined - " + record.Type.ToString(), record);
				}
			}
			else
			{
				CloseTag(record, _GAFFileReader);
			}
		}
	}

	private TagRecord OpenTag(BinaryReader _Reader)
	{
		TagRecord record = new TagRecord();

		TagBase.TagType tagType = (TagBase.TagType)_Reader.ReadUInt16 ();
		if (!System.Enum.IsDefined(typeof(TagBase.TagType), tagType))
			tagType = TagBase.TagType.TagInvalid;

		record.Type 					= tagType;
		record.TagSize 					= _Reader.ReadUInt32();
		record.ExpectedStreamPosition	= _Reader.BaseStream.Position + record.TagSize;

		return record;
	}

	private void CheckTag(TagRecord _Record, BinaryReader _Reader)
	{
		if (_Reader.BaseStream.Position != _Record.ExpectedStreamPosition)
		{
			Debug.LogError(
				"GAFReader::CloseTag - " +
				"Tag " + _Record.Type.ToString() + " " +
				"hasn't been correctly read, tag length is not respected. " +
				"Expected " + _Record.ExpectedStreamPosition + " " +
				"but actually " + _Reader.BaseStream.Position + " !");
		}
	}

	private void CloseTag(TagRecord _Record, BinaryReader _Reader)
	{
		_Reader.BaseStream.Position = _Record.ExpectedStreamPosition;
	}

	#endregion
}
