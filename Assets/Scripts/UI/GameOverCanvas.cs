using UnityEngine;
using UnityEngine.UI;

class GameOverCanvas : SingletonMonoBehaviour<GameOverCanvas>
{
	[SerializeField] Animator gameOverScreenAnimator;
	[SerializeField] Button restartButton;

	void OnValidate()
	{
		if (gameOverScreenAnimator == null)
			gameOverScreenAnimator = GetComponent<Animator>();
		if (restartButton == null)
			restartButton = GetComponentInChildren<Button>();
	}

	protected sealed override void SingletonAwake()
	{
		restartButton.onClick.AddListener(CloseUI);
		Player.SpaceshipDied += OpenGameOverMenu;
	}


	void OpenGameOverMenu()
	{
		gameOverScreenAnimator.SetBool("IsOpen", true);
	}

	// FROM ANIM METHOD
	public void OnRestartGame()
	{
		AsteroidsGameManager.DoRestartGame();
	}

	void CloseUI()
	{
		gameOverScreenAnimator.SetBool("IsOpen", false);
	}
}
