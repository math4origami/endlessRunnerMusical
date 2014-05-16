/*
 * File:           GAFMovieClip.cs
 * Version:        3.3.1
 * Last changed:   Date: 2014/05/08
 * Author:         Alexey Nikitin
 * Copyright:      © Catalyst Apps
 * Product:        GAF Animation Player
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("GAF/GAFMovieClip")]
[ExecuteInEditMode]
public class GAFMovieClip : MonoBehaviour
{
	#region Events
	
	public delegate void GAFMovieClipCallback(GAFMovieClip _Clip);
	
	public event GAFMovieClipCallback on_start_play;
	public event GAFMovieClipCallback on_stop_play;
	public event GAFMovieClipCallback on_goto;
	public event GAFMovieClipCallback on_sequence_change;
	public event GAFMovieClipCallback on_clear;
		
	#endregion // Events

	#region MovieClip Interface

	public void play()
	{
		SetPlaying (true);
	}
	
	public void pause()
	{
		SetPlaying (false);
	}

	public void stop()
	{
		UpdateToFrame (data.AnimationSequences [(int)getCurrentSequenceIndex ()].StartFrame);
		SetPlaying (false);
	}
	
	public void gotoAndStop(uint _FrameNumber)
	{
		_FrameNumber = (uint)Mathf.Clamp (
			  (int)_FrameNumber
			, (int)data.AnimationSequences [(int)getCurrentSequenceIndex ()].StartFrame
			, (int)data.AnimationSequences [(int)getCurrentSequenceIndex ()].EndFrame);

		UpdateToFrame (_FrameNumber);

		if (on_goto != null)
			on_goto (this);

		SetPlaying (false);
	}

	public void gotoAndPlay(uint _FrameNumber)
	{
		_FrameNumber = (uint)Mathf.Clamp (
			  (int)_FrameNumber
			, (int)data.AnimationSequences [(int)getCurrentSequenceIndex ()].StartFrame
			, (int)data.AnimationSequences [(int)getCurrentSequenceIndex ()].EndFrame);
		
		UpdateToFrame (_FrameNumber);

		if (on_goto != null)
			on_goto (this);
		
		SetPlaying (true);
	}

	public void setSequence(string _SequenceName, bool _PlayImmediately = false)
	{
		int sequenceIndex = -1;
		for(int i = 0; i < data.AnimationSequences.Count; ++i)
		{
			if (data.AnimationSequences[i].ID == _SequenceName)
			{
				sequenceIndex = i;
				break;
			}
		}

		if (sequenceIndex >= 0 &&
		    m_SequenceIndex != sequenceIndex)
		{
			m_SequenceIndex = sequenceIndex;

			UpdateToFrame (data.AnimationSequences[(int)getCurrentSequenceIndex()].StartFrame);

			if (on_sequence_change != null)
				on_sequence_change (this);

			SetPlaying (_PlayImmediately);
		}
	}

	public void setDefaultSequence(bool _PlayImmediately = false)
	{
		setSequence ("Default", _PlayImmediately);
	}

	public uint getCurrentSequenceIndex()
	{
		return (uint)m_SequenceIndex;
	}

	public uint getCurrentFrameNumber()
	{
		return (uint)m_CurrentFrameNumber;
	}
	
	public uint getFramesCount()
	{
		return data.FramesCount;
	}

	public GAFWrapMode getAnimationWrapMode()
	{
		return settings.wrapMode;
	}

	public void setAnimationWrapMode(GAFWrapMode _Mode)
	{
		settings.wrapMode = _Mode;
	}
	
	public bool isPlaying()
	{
		return m_IsPlaying;
	}

	public float duration()
	{
		return (data.AnimationSequences[(int)getCurrentSequenceIndex()].EndFrame - data.AnimationSequences[(int)getCurrentSequenceIndex()].StartFrame) * settings.targetSPF;
	}

	public string addTrigger(GAFMovieClipCallback _Callback, uint _FrameNumber)
	{
		if (_FrameNumber < data.FramesCount)
		{
			GAFFrameEvent triggerEvent = new GAFFrameEvent (_Callback);
			if (m_FrameEvents.ContainsKey(_FrameNumber))
			{
				m_FrameEvents [_FrameNumber].Add (triggerEvent);
			}
			else
			{
				m_FrameEvents.Add(_FrameNumber, new List<GAFFrameEvent>());
				m_FrameEvents[_FrameNumber].Add(triggerEvent);
			}

			return triggerEvent.ID;
		}

		return string.Empty;
	}

	public void removeTrigger(string _ID)
	{
		foreach(KeyValuePair<uint, List<GAFFrameEvent>> pair in m_FrameEvents)
		{
			pair.Value.RemoveAll(delegate(GAFFrameEvent _event) 
			{
				return _event.ID == _ID;
			});
		}
	}

	public void removeAllTriggers(uint _FrameNumber)
	{
		if (_FrameNumber < data.FramesCount)
		{
			if (m_FrameEvents.ContainsKey(_FrameNumber))
			{
				m_FrameEvents[_FrameNumber].Clear();
			}
		}
	}

	public void removeAllTriggers()
	{
		m_FrameEvents.Clear ();
	}

	public void init()
	{
		if (!isInitialized)
		{
			if (asset != null)
			{
				asset.reloadData();
				if (data != null)
				{
					asset.reloadTextures ();

					CreateMaskElements (data);
					CreateAnimationObjects (data);

					m_IsInitialized = true;

					Start ();
				}
			}
		}
	}

	public void clear()
	{
		if (on_clear != null)
			on_clear(this);

		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in transform)
			children.Add(child.gameObject);

		children.ForEach (delegate(GameObject child) 
		{
			if (Application.isPlaying)
				Destroy (child);
			else
				DestroyImmediate(child);
		});

		if (m_ConfigurationFrames != null)
		{
			m_ConfigurationFrames.Clear ();
			m_ConfigurationFrames = null;
		}

		if (m_CurrentStates != null)
		{
			m_CurrentStates.Clear ();
			m_CurrentStates = null;
		}

		if (m_Transforms != null)
		{
			m_Transforms.Clear ();
			m_Transforms = null;
		}

		if (m_ColorTransforms != null)
		{
			m_ColorTransforms.Clear ();
			m_ColorTransforms = null;
		}

		if (m_Masked != null)
		{
			m_Masked.Clear ();
			m_Masked = null;
		}

		if (m_Masks != null)
		{
			m_Masks.Clear ();
			m_Masks = null;
		}

		m_GAFAsset	 			= null;
		m_Settings 				= new GAFAnimationPlayerSettings ();
		m_SequenceIndex 		= 0;
		m_CurrentFrameNumber 	= 0;
		m_Stopwatch 			= 0.0f;

		m_IsInitialized = false;
	}

	public void reload()
	{
		if (m_ConfigurationFrames != null)
		{
			m_ConfigurationFrames.Clear ();
			m_ConfigurationFrames = null;
		}
		
		if (m_CurrentStates != null)
		{
			m_CurrentStates.Clear ();
			m_CurrentStates = null;
		}
		
		if (m_Transforms != null)
		{
			m_Transforms.Clear ();
			m_Transforms = null;
		}
		
		if (m_ColorTransforms != null)
		{
			m_ColorTransforms.Clear ();
			m_ColorTransforms = null;
		}
		
		if (m_Masked != null)
		{
			m_Masked.Clear ();
			m_Masked = null;
		}
		
		if (m_Masks != null)
		{
			m_Masks.Clear ();
			m_Masks = null;
		}

		m_Stopwatch	= 0.0f;

		if (asset != null)
		{
			asset.reloadData();
			if (data != null)
			{
				asset.reloadTextures ();
				
				GAFAnimationBehaviour [] behaviors = GetComponentsInChildren<GAFAnimationBehaviour>();
				foreach(GAFAnimationBehaviour behavior in behaviors)
					behavior.Init();
				
				UpdateToFrame (getCurrentFrameNumber());
				
				if (Application.isPlaying)
					SetPlaying (settings.playAutomatically);
			}
		}
	}

	#endregion // MovieClip Interface

	#region Properties

	public GAFAnimationAsset asset
	{
		get
		{
			return m_GAFAsset;
		}

		set
		{
			if (m_GAFAsset != value)
			{
				clear();
				m_GAFAsset = value;
			}
		}
	}

	public GAFAnimationData data
	{
		get
		{
			return asset == null ? null : asset.data;
		}
	}

	public GAFAnimationPlayerSettings settings
	{
		get
		{
			return m_Settings;
		}
	}

	public bool isInitialized
	{
		get
		{
			return m_IsInitialized;
		}
	}

	#endregion // Properties

	#region MonoBehaviour

	private void Start()
	{
		reload();
	}

	private void FixedUpdate()
	{
		if (data != null &&
		    isPlaying() &&
		   !settings.ignoreTimeScale)
		{
			OnUpdate(Time.deltaTime);
		}
	}

	private void Update()
	{
		if (data != null &&
		    isPlaying() &&
		    settings.ignoreTimeScale)
		{
			float deltaTime = Mathf.Clamp(Time.realtimeSinceStartup - m_PreviouseUpdateTime, 0f, Time.maximumDeltaTime);
			OnUpdate(deltaTime);
			m_PreviouseUpdateTime = Time.realtimeSinceStartup;
		}
	}

	void OnApplicationFocus(bool _FocusStatus) 
	{
		if (!settings.playInBackground)
		{
			SetPlaying(_FocusStatus);
		}
	}

	void OnApplicationPause(bool _PauseStatus) 
	{
		if (!settings.playInBackground)
		{
			SetPlaying(_PauseStatus);
		}
	}

	#endregion // MonoBehaviour

	#region Implementation

	private void OnUpdate(float _TimeDelta)
	{
		m_Stopwatch += _TimeDelta;

		if (m_Stopwatch >= settings.targetSPF)
		{
			int framesCount = 1;
			if (settings.perfectTiming)
			{
				m_StoredTime += m_Stopwatch - settings.targetSPF;
				if (m_StoredTime > settings.targetSPF)
				{
					int additionalFrames = Mathf.FloorToInt(m_StoredTime / settings.targetSPF);
					m_StoredTime = m_StoredTime - (additionalFrames * settings.targetSPF);
					framesCount += additionalFrames;
				}
			}

			m_Stopwatch = 0f;

			if (getCurrentFrameNumber() + framesCount > data.AnimationSequences[(int)getCurrentSequenceIndex()].EndFrame)
			{
				switch(settings.wrapMode)
				{
				case GAFWrapMode.Once:
					SetPlaying (false);
					return;

				case GAFWrapMode.Loop:
					UpdateToFrame(data.AnimationSequences[(int)getCurrentSequenceIndex()].StartFrame);
					
					if (on_stop_play != null)
						on_stop_play(this);

					if (on_start_play != null)
						on_start_play(this);

					return;

				default:
					SetPlaying (false);
					return;
				}
			}
			
			UpdateToFrame (getCurrentFrameNumber() + (uint)framesCount);
		}
	}

	private void CreateMaskElements(GAFAnimationData _Data)
	{
		if (_Data.AnimationMasks != null)
		{
			for (uint i = 0; i < _Data.AnimationMasks.Count; ++i)
			{
				GAFAnimationMask mask = _Data.AnimationMasks[(int)i];
				GameObject maskObject = new GameObject(
					  mask.MaskName + "_" + mask.ObjectName + "_mask"
					, new System.Type[] 
						{
							  typeof(GAFTransform)
							, typeof(GAFMaskBehaviour)
							//, typeof(GAFColorTransform) // Uncomment to view masks
						}
					);

				maskObject.transform.parent = transform;
			}
		}
	}
	
	private void CreateAnimationObjects(GAFAnimationData _Data)
	{
		for(uint i = 0; i < _Data.AnimationObjects.Count; ++i)
		{
			GAFAnimationObject _object = _Data.AnimationObjects[(int)i];
			
			GameObject animationObject = new GameObject(
				  _object.AtlasElementName + "_" + _object.ObjectName
				, new System.Type[] 
					{
						  typeof(GAFTransform)
						, typeof(GAFColorTransform)
						, typeof(GAFMaskedBehaviour)
					}
				);

			animationObject.transform.parent = transform;
		}
	}

	private void SetupData()
	{
		if (data != null && isInitialized)
		{
			if (m_CurrentStates == null)
			{
				m_CurrentStates = new Dictionary<string, GAFState>();
				foreach (GAFAnimationObject element in data.AnimationObjects)
				{
					GAFState state = new GAFState(element.ObjectName);
					m_CurrentStates.Add(element.ObjectName, state);
				}
			}

			if (m_ConfigurationFrames == null)
			{
				m_ConfigurationFrames = new Dictionary<uint, GAFAnimationFrame>();
				foreach(GAFAnimationFrame _frame in data.AnimationFrames)
				{
					m_ConfigurationFrames.Add(_frame.FrameNumber, _frame);
					
					if (_frame.FrameNumber <= getCurrentFrameNumber())
					{
						foreach(GAFState _state in _frame.States)
						{
							m_CurrentStates[_state.Name] = _state;
						}
					}
				}
			}

			if (m_Transforms == null)
			{
				m_Transforms = new Dictionary<string, GAFTransform>();
				GAFTransform [] transforms = GetComponentsInChildren<GAFTransform> ();
				foreach(GAFTransform _transform in transforms)
					m_Transforms.Add(_transform.name.Split('_')[1], _transform);
			}

			if (m_ColorTransforms == null)
			{
				m_ColorTransforms = new Dictionary<string, GAFColorTransform>();
				GAFColorTransform [] colorTransforms = GetComponentsInChildren<GAFColorTransform> ();
				foreach(GAFColorTransform _colorTransform in colorTransforms)
					m_ColorTransforms.Add(_colorTransform.name.Split('_')[1], _colorTransform);
			}

			if (m_Masked == null)
			{
				m_Masked = new Dictionary<string, GAFMaskedBehaviour>();
				GAFMaskedBehaviour [] masked = GetComponentsInChildren<GAFMaskedBehaviour> ();
				foreach(GAFMaskedBehaviour _masked in masked)
					m_Masked.Add(_masked.name.Split('_')[1], _masked);
			}

			if (m_Masks == null)
			{
				m_Masks = new Dictionary<string, GAFMaskBehaviour>();
				GAFMaskBehaviour [] masks = GetComponentsInChildren<GAFMaskBehaviour> ();
				foreach(GAFMaskBehaviour _mask in masks)
					m_Masks.Add(_mask.name.Split('_')[1], _mask);
			}
		}
	}

	private void UpdateToFrame(uint _FrameNumber)
	{
		SetupData ();

		GAFAnimationFrame frame = GetFrameByNumber (_FrameNumber);

		bool force = _FrameNumber - getCurrentFrameNumber () != 1;

		foreach ( GAFState animationState in frame.States )
		{
			if (m_Transforms.ContainsKey(animationState.Name))
				m_Transforms[animationState.Name].UpdateTransform(animationState, force);

			if (m_ColorTransforms.ContainsKey(animationState.Name))
				m_ColorTransforms[animationState.Name].UpdateColor(animationState, force);
		}

		foreach ( GAFState animationState in frame.States )
		{
			if (animationState.MaskID != string.Empty)
			{
				if (m_Masks.ContainsKey(animationState.MaskID) &&
				    m_Masked.ContainsKey(animationState.Name))
				{
					m_Masked[animationState.Name].UpdateMask(m_Masks[animationState.MaskID]);
				}
			}
		}

		m_CurrentFrameNumber = (int)_FrameNumber;

		if (m_FrameEvents.ContainsKey(_FrameNumber))
		{
			foreach(GAFFrameEvent _event in m_FrameEvents[_FrameNumber])
			{
				_event.Trigger(this);
			}
		}
	}

	private GAFAnimationFrame GetFrameByNumber(uint _FrameNumber)
	{
		GAFAnimationFrame frame = new GAFAnimationFrame(_FrameNumber);
		if (_FrameNumber - getCurrentFrameNumber() == 1 ||
		    _FrameNumber == getCurrentFrameNumber())
		{
			if (m_ConfigurationFrames.ContainsKey(frame.FrameNumber))
			{
				foreach(GAFState _state in m_ConfigurationFrames[frame.FrameNumber].States)
				{
					m_CurrentStates[_state.Name] = _state;
				}
			}
		}
		else
		{
			m_CurrentStates.Clear ();
			foreach (GAFAnimationObject element in data.AnimationObjects)
			{
				GAFState state = new GAFState(element.ObjectName);
				m_CurrentStates.Add(element.ObjectName, state);
			}
			
			foreach(GAFAnimationFrame _frame in data.AnimationFrames)
			{
				if (_frame.FrameNumber > _FrameNumber)
					break;
				
				foreach(GAFState _state in _frame.States)
				{
					m_CurrentStates[_state.Name] = _state;
				}
			}
		}

		foreach(KeyValuePair<string, GAFState> pair in m_CurrentStates)
		{
			frame.States.Add(pair.Value);
		}

		return frame;
	}

	private void SetPlaying(bool _IsPlay)
	{
		if (m_IsPlaying != _IsPlay)
		{
			m_IsPlaying = _IsPlay;

			if (m_IsPlaying)
			{
				if (on_start_play != null)
					on_start_play(this);

				m_Stopwatch = 0.0f;
				m_PreviouseUpdateTime = 0f;
			}
			else
			{
				if (on_stop_play != null)
					on_stop_play(this);

				m_Stopwatch = 0.0f;
				m_PreviouseUpdateTime = 0f;
			}
		}
	}

	#endregion // Implementation

	#region Classes

	private class GAFFrameEvent
	{
		private GAFMovieClipCallback 	m_Callback 	= null;
		private string					m_ID		= string.Empty;
		
		public GAFFrameEvent(GAFMovieClipCallback _Callback)
		{
			m_Callback 	= _Callback;
			m_ID 		= System.Guid.NewGuid().ToString();
		}
		
		public string ID
		{
			get
			{
				return m_ID;
			}
		}
		
		public void Trigger(GAFMovieClip _Clip)
		{
			m_Callback (_Clip);
		}
	}

	#endregion // Classes

	#region Members

	[HideInInspector][SerializeField] private GAFAnimationAsset				m_GAFAsset				= null;
	[HideInInspector][SerializeField] private GAFAnimationPlayerSettings 	m_Settings				= null;
	[HideInInspector][SerializeField] private int 							m_SequenceIndex			= 0;
	[HideInInspector][SerializeField] private int 							m_CurrentFrameNumber 	= 0;
	[HideInInspector][SerializeField] private bool 							m_IsInitialized			= false;

	private Dictionary<uint, GAFAnimationFrame> 	m_ConfigurationFrames 	= null;
	private Dictionary<string, GAFState>			m_CurrentStates			= null;
	private Dictionary<string, GAFTransform>		m_Transforms			= null;
	private Dictionary<string, GAFColorTransform>	m_ColorTransforms		= null;
	private Dictionary<string, GAFMaskedBehaviour>	m_Masked				= null;
	private Dictionary<string, GAFMaskBehaviour>	m_Masks					= null;

	private Dictionary<uint, List<GAFFrameEvent>> m_FrameEvents	= new Dictionary<uint  , List<GAFFrameEvent>>();
	
	private bool 	m_IsPlaying 	= false;
	private float 	m_Stopwatch 	= 0f;
	private float 	m_StoredTime 	= 0f;

	private float 	m_PreviouseUpdateTime = 0f;

	#endregion // Members
}
