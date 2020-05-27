using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound{
	public SoundsList name;
	public AudioClip clip;

	[Range(0f,1f)] public float volume;
	[Range(0f, 1f)] public float pitch = 1;

	[HideInInspector] public AudioSource source;
}
