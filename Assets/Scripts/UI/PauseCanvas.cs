using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
	[SerializeField] KeyCode pauseKey = KeyCode.Escape;

	[SerializeField] Animator pauseAnimator;
	[SerializeField] Button continueButton;
	[SerializeField] Button pauseButton;
	[SerializeField] Button restartButton;

	void OnValidate()
	{
		if (pauseAnimator == null)
			pauseAnimator = GetComponent<Animator>(); 
	}

	void Start()
	{
		continueButton.onClick.AddListener(Pause);
		pauseButton.onClick.AddListener(Pause);
		restartButton.onClick.AddListener(Restart);
	}

	void Restart() 
	{
		Pause();
		UpgradeManager.EnableSpawn(false);
		AsteroidsGameManager.DoRestartGame();
	}

	void Update()
	{
		if (Input.GetKeyDown(pauseKey))
			Pause();
	}

	public void Pause()
	{
		bool pause = Time.timeScale == 1;
		pauseAnimator.SetBool("IsOpen", pause);
		Time.timeScale = pause ? 0 : 1;
	}
}
