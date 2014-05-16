/*
 * File:           GAFAnimationEditor.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GAFMovieClip))]
public class GAFAnimationEditor : Editor 
{
	#region Members
	
	private List<string> m_SequenceNames 		= null;
	private List<string> m_TextureAtlasScales	= null;
	private List<string> m_AtlasCSFs				= null;

	private Texture m_BeginAnimationButtonTexture 		= null;
	private Texture m_EndAnimationButtonTexture 		= null;
	private Texture m_LeftAnimationButtonTexture 		= null;
	private Texture m_RightAnimationButtonTexture 		= null;
	private Texture m_ConverterDownloadButtonTexture 	= null;

	#endregion // Members

	public override void OnInspectorGUI ()
	{
		GAFMovieClip movieClip = (GAFMovieClip) target;

		if (m_BeginAnimationButtonTexture 	 == null ||
		    m_EndAnimationButtonTexture 	 == null ||
		    m_LeftAnimationButtonTexture 	 == null ||
		    m_RightAnimationButtonTexture 	 == null ||
		    m_ConverterDownloadButtonTexture == null)
		{
			m_BeginAnimationButtonTexture 	 = AssetDatabase.LoadAssetAtPath("Assets/GAF/Data/BeginButton.png"		, typeof(Texture)) as Texture;
			m_EndAnimationButtonTexture 	 = AssetDatabase.LoadAssetAtPath("Assets/GAF/Data/EndButton.png"		, typeof(Texture)) as Texture;
			m_LeftAnimationButtonTexture 	 = AssetDatabase.LoadAssetAtPath("Assets/GAF/Data/OneFrameLeft.png"		, typeof(Texture)) as Texture;
			m_RightAnimationButtonTexture 	 = AssetDatabase.LoadAssetAtPath("Assets/GAF/Data/OneFrameRight.png"	, typeof(Texture)) as Texture;
			m_ConverterDownloadButtonTexture = AssetDatabase.LoadAssetAtPath("Assets/GAF/Data/BannerButtonImage.png", typeof(Texture)) as Texture;
		}

		if (movieClip.asset == null)
		{
			GUILayout.Space(3f);
			GUILayout.Label("Asset file:");
			movieClip.asset = (GAFAnimationAsset)EditorGUILayout.ObjectField(movieClip.asset, typeof(GAFAnimationAsset), false);
		}
		else if (movieClip.asset != null && !movieClip.isInitialized)
		{
			GUILayout.Space(3f);
			GUILayout.Label("Asset file:");
			movieClip.asset = (GAFAnimationAsset)EditorGUILayout.ObjectField(movieClip.asset, typeof(GAFAnimationAsset), false);

			GUILayout.Space(3f);
			if (GUILayout.Button("Create GAF movie clip"))
			{
				movieClip.asset.reloadData();
				movieClip.asset.reloadTextures();
				movieClip.asset.reimportTextures();

				movieClip.init();
				
				System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
				System.Type type = assembly.GetType("UnityEditor.GameView");
				EditorWindow gameview = EditorWindow.GetWindow(type);
				
				gameview.Repaint();
				SceneView.RepaintAll();
			}
		}
		else if (movieClip.asset != null && movieClip.isInitialized)
		{
			if (movieClip.data == null)
			{
				movieClip.reload();
			}

			if (m_SequenceNames == null)
			{
				m_SequenceNames = new List<string>();
				foreach(GAFAnimationSequence sequence in movieClip.data.AnimationSequences)
					m_SequenceNames.Add(sequence.ID);
			}

			if (m_TextureAtlasScales == null)
			{
				m_TextureAtlasScales = new List<string>();
				foreach(GAFTextureAtlas textureAtlas in movieClip.data.TextureAtlases)
					m_TextureAtlasScales.Add(textureAtlas.Scale.ToString());
			}

			if (m_AtlasCSFs == null)
			{
				m_AtlasCSFs = new List<string>();
				foreach (float csf in movieClip.data.TextureAtlases[0].Atlases[0].CSF)
					m_AtlasCSFs.Add(csf.ToString());
			}

			GUILayout.Space(10f);
			GUILayout.Label("Animation clip: " + System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(movieClip.asset)), EditorStyles.boldLabel);

			GUILayout.BeginVertical(EditorStyles.textField);

			if (!Application.isPlaying)
			{
				GUILayout.Space(3f);
				movieClip.settings.playAutomatically = GUILayout.Toggle(movieClip.settings.playAutomatically, " - Play automatically");

				GUILayout.Space(3f);
				movieClip.settings.ignoreTimeScale = GUILayout.Toggle(movieClip.settings.ignoreTimeScale, " - Ignore time scale");

				GUILayout.Space(3f);
				movieClip.settings.perfectTiming = GUILayout.Toggle(movieClip.settings.perfectTiming, " - Perfect timing (possible frames skip)");

				GUILayout.Space(3f);
				movieClip.settings.playInBackground = GUILayout.Toggle(movieClip.settings.playInBackground, " - Play in backgound");

				{
					GUILayout.Space(10f);
					float previouse = EditorGUILayout.FloatField("Pixels per unit: ", movieClip.settings.pixelsPerUnit);
					previouse = Mathf.Clamp(previouse, 0.001f, 1000f);
					if (previouse != movieClip.settings.pixelsPerUnit)
					{
						movieClip.settings.pixelsPerUnit = previouse;
						movieClip.reload();
					}
				}

				{
					GUILayout.Space(3f);
					int currentIndex = 0;
					for (; currentIndex < m_TextureAtlasScales.Count; currentIndex++)
						if (movieClip.settings.scale.ToString() == m_TextureAtlasScales[currentIndex])
							break;

					float previouse = float.Parse(m_TextureAtlasScales[EditorGUILayout.Popup("Texture atlas scale:", currentIndex, m_TextureAtlasScales.ToArray())]);
					if (previouse != movieClip.settings.scale)
					{
						movieClip.settings.scale = previouse;
						movieClip.reload();
					}
				}

				{
					GUILayout.Space(3f);
					int currentIndex = 0;
					for (; currentIndex < m_AtlasCSFs.Count; currentIndex++)
						if (movieClip.settings.csf.ToString() == m_AtlasCSFs[currentIndex])
							break;

					float previouse = float.Parse(m_AtlasCSFs[EditorGUILayout.Popup("Content scale factor:", currentIndex, m_AtlasCSFs.ToArray())]);
					if (previouse != movieClip.settings.csf)
					{
						movieClip.settings.csf = previouse;
						movieClip.reload();
					} 
				}

				GUILayout.Space(10f);
				movieClip.settings.wrapMode = (GAFWrapMode) EditorGUILayout.EnumPopup("Wrap mode:", movieClip.settings.wrapMode);

				GUILayout.Space(3f);
				movieClip.setSequence(movieClip.data.AnimationSequences[ EditorGUILayout.Popup("Sequence:", (int)movieClip.getCurrentSequenceIndex(), m_SequenceNames.ToArray() ) ].ID);

				GUILayout.Space(10f);
				movieClip.settings.targetFPS = (uint)EditorGUILayout.IntField("Target FPS:", (int)movieClip.settings.targetFPS);
				movieClip.settings.targetFPS = (uint)Mathf.Clamp((int)movieClip.settings.targetFPS, 0, 1000);
			}

			if (!Application.isPlaying)
			{
				GUILayout.Space(3f);
				EditorGUILayout.LabelField("Frames timeline:");

				movieClip.gotoAndStop((uint)EditorGUILayout.IntSlider(
					  (int)movieClip.getCurrentFrameNumber()
					, (int)movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].StartFrame
					, (int)movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].EndFrame));
			}
			else
			{
				GUILayout.Space(3f);
				EditorGUILayout.LabelField("Frames timeline:");

				EditorGUILayout.IntSlider(
					  (int)movieClip.getCurrentFrameNumber()
					, (int)movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].StartFrame
					, (int)movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].EndFrame);
			}

			if (!Application.isPlaying)
			{
				EditorGUILayout.BeginHorizontal();

				GUI.enabled = movieClip.getCurrentFrameNumber() > movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].StartFrame;
				if (GUILayout.Button(m_BeginAnimationButtonTexture, EditorStyles.miniButtonLeft))
				{
					movieClip.gotoAndStop(movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].StartFrame);
				}
				GUI.enabled = true;

				GUI.enabled = movieClip.getCurrentFrameNumber() > movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].StartFrame;
				if (GUILayout.Button(m_LeftAnimationButtonTexture, EditorStyles.miniButtonMid))
				{
					movieClip.gotoAndStop(movieClip.getCurrentFrameNumber() - 1);
				}
				GUI.enabled = true;

				GUI.enabled = movieClip.getCurrentFrameNumber() < movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].EndFrame;
				if (GUILayout.Button(m_RightAnimationButtonTexture, EditorStyles.miniButtonMid))
				{
					movieClip.gotoAndStop(movieClip.getCurrentFrameNumber() + 1);
				}
				GUI.enabled = true;

				GUI.enabled = movieClip.getCurrentFrameNumber() < movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].EndFrame;
				if (GUILayout.Button(m_EndAnimationButtonTexture, EditorStyles.miniButtonRight))
				{
					movieClip.gotoAndStop(movieClip.data.AnimationSequences[(int)movieClip.getCurrentSequenceIndex()].EndFrame);
				}
				GUI.enabled = true;

				EditorGUILayout.EndHorizontal();
			}

			if (!Application.isPlaying)
			{
				GUILayout.Space(15f);

				if (GUILayout.Button("Clear animation"))
				{
					m_SequenceNames	= null;
					m_TextureAtlasScales = null;
					m_AtlasCSFs = null;
					movieClip.clear();
				}
			}

			GUILayout.EndVertical();
		}

		GUIContent content = new GUIContent();
		content.text = "  Download GAF Converter and receive 50 SWF conversions";
		content.image = m_ConverterDownloadButtonTexture;

		GUILayout.Space(7f);
		GUI.backgroundColor = new Color(0.016f, 0.423f, 0.537f, 1);
		if (GUILayout.Button(content))
		{
			Application.OpenURL("http://gafmedia.com/converter/?action=downloads");
		}
		GUILayout.Space(7f);
	}
}
