using UnityEngine;

public class Effect : MonoBehaviour
{
	[SerializeField] ParticleSystem particleSys;
	[SerializeField] AudioSource[] audioSources;

	[SerializeField] AudioClip[] audioClips;

	private void OnValidate()
	{
		if (particleSys == null)
			particleSys = GetComponentInChildren<ParticleSystem>();
		audioSources = GetComponentsInChildren<AudioSource>();
	}

	void Start()
	{
		foreach (ParticleSystem ps in particleSys.GetComponentsInChildren<ParticleSystem>())
		{
			ParticleSystem.MainModule main = ps.main;
			main.loop = false;
		}

		float duration = 0;
		if (particleSys != null)
		{
			duration = Mathf.Max(duration, particleSys.main.duration);
			particleSys.Play();
		}

		if(audioClips.Length!= 0 && audioSources.Length!= 0)
			audioSources[0].clip = audioClips[Random.Range(0, audioClips.Length)];

		foreach (AudioSource audioSource in audioSources)
		{
			if (audioSource != null)
			{
				duration = Mathf.Max(duration, audioSource.clip.length);
				audioSource.Play();
			}
		}

		Invoke(nameof(Die), duration);
	}

	void Die()
	{
		Destroy(gameObject);
	}
}
