using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundMgr : MonoBehaviour {
	private static SoundMgr instance;
	private AudioSource audioSource;


	#region public property

	public static SoundMgr Instance {
		get;
		private set;
	}

	#endregion


	#region private property

	[SerializeField]
	private AudioClip _slash;
	public AudioClip slash
	{
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

	#endregion


	// Use this for initialization
	void Awake () {
		if(Instance != null){
			Destroy (gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad (gameObject);

		audioSource = GetComponent<AudioSource>();
	}


	// 指定されたクリップを再生する
	public void PlayClip(AudioClip clip) {
		audioSource.PlayOneShot(clip);
	}
		
}