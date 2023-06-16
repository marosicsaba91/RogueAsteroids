using TMPro;
using UnityEngine;
class GameUIManager : SingletonMonoBehaviour<GameUIManager>
{
	[SerializeField] TMP_Text levelText;
	[SerializeField] TMP_Text rocketText;
	[SerializeField] TMP_Text healthText;
	[SerializeField] RectTransform maxHealthFrame;
	[SerializeField] RectTransform currentHealthBar;

	[SerializeField] AnimationCurve healthToLength;
	[SerializeField] float healthChangeSpeed = 10f;
	[SerializeField] float maxHealthChangeSpeed = 500f;

	protected sealed override void SingletonAwake()
	{
		AsteroidsGameManager.RestartGame += OnRestartGame;
		AsteroidsGameManager.NewLevelLoaded += _ => UpdateUI();
		Player.HealthChanged += UpdateUI;
	}

	void OnRestartGame()
	{
		// _shownMaxHealth = Player.MaxHealth;
		// _shownHealth = Player.CurrentHealth;
		UpdateUI();
	}

	void UpdateUI()
	{
		levelText.text = AsteroidsGameManager.Level.ToString("00");

		float targetHealth = Player.CurrentHealth;
		float targetMaxHealth = Player.MaxHealth;
		healthText.text = targetHealth.ToString("0") + " / " + targetMaxHealth.ToString("0");
	}

	float _shownHealth;
	float _shownMaxHealth;

	void Update()
	{
		ChangeHealthBar();
	}

	void ChangeHealthBar()
	{
		float targetHealth = Player.CurrentHealth;
		float targetMaxHealth = Player.MaxHealth;
		if (_shownHealth != targetHealth || _shownMaxHealth != targetMaxHealth)
		{
			_shownHealth = Mathf.MoveTowards(_shownHealth, targetHealth, Time.deltaTime * healthChangeSpeed);
			 
			float healthRate = Mathf.Clamp01(_shownHealth / _shownMaxHealth); 
			Vector2 anchors = currentHealthBar.anchorMax;
			anchors.x = healthRate;
			currentHealthBar.anchorMax = anchors; 

			_shownMaxHealth = Mathf.MoveTowards(_shownMaxHealth, targetMaxHealth, Time.deltaTime * maxHealthChangeSpeed);

			Vector2 sizeDelta = maxHealthFrame.sizeDelta;
			sizeDelta.x = healthToLength.Evaluate(_shownMaxHealth);
			maxHealthFrame.sizeDelta = sizeDelta;
		}
	}
}
