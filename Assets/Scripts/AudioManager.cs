using UnityEngine.Audio;
using UnityEngine;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public enum SoundsList
{
	StartGame,LevelWin,GameOver,SelectLevel,Click
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;
	
	public Sound musicBackground;
	[Header("SFX")]
	public Sound[] sounds;
	public Sound[] pigOnGrab;
	public Sound[] pigHit;
	
	void Awake () {
		if(Instance != null) return;
		Instance = this;
		
		foreach (var s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
		foreach (var s in pigOnGrab)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
		foreach (var s in pigHit)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}

		musicBackground.source = gameObject.AddComponent<AudioSource>();
		musicBackground.source.clip = musicBackground.clip;
		musicBackground.source.volume = musicBackground.volume;
		musicBackground.source.pitch = musicBackground.pitch;
		musicBackground.source.loop = true;
		musicBackground.source.Play();
	}

	public void UpdateVolume(float volume)
	{
		foreach (var s in sounds)
		{
			s.source.volume = volume;
			s.volume = volume;
		}
	}

	public void UpdateMusicVolume(float volume)
	{
		musicBackground.source.volume = volume;
		musicBackground.volume = volume;
	}
	
	public void Play(SoundsList sName)
	{
		print("PLAY : " + sName.ToString());
		try
		{
			var s = Array.Find(sounds, sound => sound.name == sName);
			s.source.Play();
		}
		catch
		{
			Debug.LogError("Sound " + sName.ToString() + " not Found");
		}
		
	}
	
	public void PlayRandomPigGrab()
	{
		var random = Random.Range(0, pigOnGrab.Length);
		pigOnGrab[random].source.Play();
	}

	private AudioSource _hitSrc;
	
	public void PlayRandomPigHit()
	{
		if(_hitSrc!= null && _hitSrc.isPlaying) return;
		var random = Random.Range(0, pigHit.Length);
		_hitSrc = pigHit[random].source;
		_hitSrc.Play();
	}

}
