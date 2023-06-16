using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
	[SerializeField] KeyCode pauseKey = KeyCode.Escape;

	[SerializeField] Animator pauseAnimator;
	[SerializeField] Button continueButton;

	void OnValidate()
	{
		if (pauseAnimator == null)
			pauseAnimator = GetComponent<Animator>();
		if (continueButton == null)
			continueButton = GetComponentInChildren<Button>();
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
