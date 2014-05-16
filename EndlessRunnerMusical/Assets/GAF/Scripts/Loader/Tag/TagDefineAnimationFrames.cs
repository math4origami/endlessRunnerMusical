/*
 * File:           TagDefineAnimationFrames.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TagDefineAnimationFrames : TagBase 
{
	enum GAFColorTransformIndex
	{
		  GAFCTI_R
		, GAFCTI_G
		, GAFCTI_B
		, GAFCTI_A
		, COUNT
	};

	enum GAFFilterType
	{
		GFT_Blur = 1
	};

	public override void Read(BinaryReader _GAFFileReader, GAFAnimationData _Data)
	{
		uint framesCount = _GAFFileReader.ReadUInt32();
		for (uint i = 0; i < framesCount; ++i)
		{
			uint frameNumber = _GAFFileReader.ReadUInt32();
			GAFAnimationFrame frame = new GAFAnimationFrame(frameNumber);
			
			uint statesCount = _GAFFileReader.ReadUInt32();
			for (uint j = 0; j < statesCount; ++j)
			{
				frame.AddState(ExctractState(_GAFFileReader));
			}
			
			_Data.AnimationFrames.Add(frame);
		}
	}

	private GAFState ExctractState(BinaryReader _Reader)
	{
		bool hasColorTransform 	= System.Convert.ToBoolean(_Reader.ReadByte());
		bool hasMasks			= System.Convert.ToBoolean(_Reader.ReadByte());
		bool hasEffect			= System.Convert.ToBoolean(_Reader.ReadByte());

		uint objectIdRef = _Reader.ReadUInt32();
		GAFState state = new GAFState(objectIdRef.ToString());

		state.ZOrder	= _Reader.ReadInt32();
		state.Alpha		= _Reader.ReadSingle();
		state.A 		= _Reader.ReadSingle();
		state.B 		= _Reader.ReadSingle();
		state.C 		= _Reader.ReadSingle();
		state.D 		= _Reader.ReadSingle();
		state.Tx 		= _Reader.ReadSingle();
		state.Ty 		= _Reader.ReadSingle();
		
		if (hasColorTransform)
		{
			float [] ctxMul = new float[(int)GAFColorTransformIndex.COUNT];
			float [] ctxOff = new float[(int)GAFColorTransformIndex.COUNT];

			ctxMul[(int)GAFColorTransformIndex.GAFCTI_A] = state.Alpha;
			ctxOff[(int)GAFColorTransformIndex.GAFCTI_A] = _Reader.ReadSingle();
			
			ctxMul[(int)GAFColorTransformIndex.GAFCTI_R] = _Reader.ReadSingle();
			ctxOff[(int)GAFColorTransformIndex.GAFCTI_R] = _Reader.ReadSingle();
			
			ctxMul[(int)GAFColorTransformIndex.GAFCTI_G] = _Reader.ReadSingle();
			ctxOff[(int)GAFColorTransformIndex.GAFCTI_G] = _Reader.ReadSingle();
			
			ctxMul[(int)GAFColorTransformIndex.GAFCTI_B] = _Reader.ReadSingle();
			ctxOff[(int)GAFColorTransformIndex.GAFCTI_B] = _Reader.ReadSingle();

			Color colorMult = new Color(
				  ctxMul[(int)GAFColorTransformIndex.GAFCTI_R]
				, ctxMul[(int)GAFColorTransformIndex.GAFCTI_G]
				, ctxMul[(int)GAFColorTransformIndex.GAFCTI_B]
				, ctxMul[(int)GAFColorTransformIndex.GAFCTI_A]);

			Color colorOff = new Color(
				ctxOff[(int)GAFColorTransformIndex.GAFCTI_R]
				, ctxOff[(int)GAFColorTransformIndex.GAFCTI_G]
				, ctxOff[(int)GAFColorTransformIndex.GAFCTI_B]
				, ctxOff[(int)GAFColorTransformIndex.GAFCTI_A]);

			state.ColorTansformationMatrix = new GAFColorTransformationMatrix(colorMult, colorOff);
		}
		
		if (hasEffect)
		{
			byte effectsCount = _Reader.ReadByte();
			for (byte e = 0; e < effectsCount; ++e)
			{
				GAFFilterType type = (GAFFilterType)_Reader.ReadUInt32();
				if (type == GAFFilterType.GFT_Blur)
				{
					Vector2 blurSize = GAFReader.ReadVector2(_Reader);
					state.HorizontalBlur = blurSize.x;
					state.VerticalBlur	 = blurSize.y;
				}
			}
		}
		
		if (hasMasks)
		{
			state.MaskID = _Reader.ReadUInt32().ToString();
		}
		
		return state;
	}
}
