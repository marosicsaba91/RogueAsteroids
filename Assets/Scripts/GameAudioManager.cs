using System.Collections;
using UnityEngine;

class GameAudioManager : SingletonMonoBehaviour<GameAudioManager>
{
	[SerializeField] AudioSource gameMusic;
	[SerializeField] AudioSource looseAudio;
	[SerializeField] AudioSource levelDoneAudio;

	[SerializeField] float musicOutFadeTime = 1f;
	[SerializeField] float musicInFadeTime = 1f;
	[SerializeField] float minimumMusicVolume = 0.2f;
	[SerializeField] float maximumMusicVolume = 1f;
	[SerializeField] float minimumMusicPitch = 0.7f;
	[SerializeField] float maximumMusicPitch = 1f;

	protected override void SingletonAwake()
	{
		AsteroidsGameManager.NewLevelLoaded += (_) => SetMusic(true);
		Player.SpaceshipDied += OnSpaceshipDied;
		AsteroidManager.AsteroidsCleared += OnAsteroidsCleared;
		gameMusic.volume = minimumMusicVolume;
	}

	void OnSpaceshipDied()
	{
		looseAudio.Play();
		SetMusic(false);
	}
	void OnAsteroidsCleared()
	{
		levelDoneAudio.Play();
		SetMusic(false);
	}

	void SetMusic(bool enable) 
	{
		StartCoroutine(SetMusicCoroutine(enable));	
	}

	IEnumerator SetMusicCoroutine(bool enable) 
	{
		float musicFadeTime = enable ? musicInFadeTime : musicOutFadeTime;
		float startTime = Time.time;
		float t = 0;
		float startVolume = gameMusic.volume;
		float startPitch = gameMusic.pitch;
		float endVolume = enable ? maximumMusicVolume : minimumMusicVolume;
		float endPitch = enable ? maximumMusicPitch : minimumMusicPitch;

		if (enable)
			gameMusic.Play();

		while (t<1)
		{
			t = (Time.time - startTime )/ musicFadeTime;
			gameMusic.volume = Mathf.Lerp(startVolume, endVolume, t);
			gameMusic.pitch = Mathf.Lerp(startPitch, endPitch, t);
			yield return null;
		}	
	}
}