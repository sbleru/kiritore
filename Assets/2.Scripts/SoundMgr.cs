using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundMgr : MonoBehaviour {

	#region public property

	public static SoundMgr Instance {
		get;
		private set;
	}

	#endregion


	#region private property

	[SerializeField]
	private AudioClip _bgm_title;
	public AudioClip bgm_title
	{
		get { return this._bgm_title; }
		set { this._bgm_title = value; }
	}
	[SerializeField]
	private AudioClip _bgm_play;
	public AudioClip bgm_play
	{
		get { return this._bgm_play; }
		set { this._bgm_play = value; }
	}
	[SerializeField]
	private AudioClip _slash;
	public AudioClip slash {
		get { return this._slash; }
		set { this._slash = value; }
	}
	[SerializeField]
	private AudioClip _touch;
	public AudioClip touch
	{
		get { return this._touch; }
		set { this._touch = value; }
	}

	private static SoundMgr instance;
	private AudioSource audioSource_bgm, audioSource_se;

	#endregion


	#region event

	// Use this for initialization
	void Awake () {
		if(Instance != null){
			Destroy (gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad (gameObject);

		AudioSource[] audioSources = GetComponents<AudioSource>();
		audioSource_bgm = audioSources[0];
		audioSource_se = audioSources[1];

	}

	#endregion


	#region public method

	// 指定されたクリップを再生する
	public void PlayClip(AudioClip clip) {
		audioSource_se.PlayOneShot(clip);
	}

	//指定されたBGMを再生する
	public void PlayBGM(AudioClip clip){
		audioSource_bgm.clip = clip;
		audioSource_bgm.Play ();
	}

	#endregion
		
}