
using UnityEngine;

class CameraManager : SingletonMonoBehaviour<CameraManager>
{
	[SerializeField] Camera mainCamera;
	[SerializeField] float sizeChange = 5;

	float _startSize;
	float _targetSize;

	public static float TargetSize
	{
		get => Instance._targetSize;
		set => Instance._targetSize = value;
	}

	protected override void SingletonAwake() 
	{
		_startSize = mainCamera.orthographicSize;
		AsteroidsGameManager.RestartGame += RestartGame;
	}

	void RestartGame()
	{
		_targetSize = _startSize;
	}

	private void OnValidate()
	{
		if (mainCamera == null)
			mainCamera = Camera.main;
	}

	public static float ScreenSize
	{
		get => Instance.mainCamera.orthographicSize;
		set => Instance.mainCamera.orthographicSize = value;
	}

	void FixedUpdate()
	{
		if (mainCamera.orthographicSize == _targetSize) return;
		float size  = Mathf.Lerp(mainCamera.orthographicSize, _targetSize, Time.fixedDeltaTime * sizeChange);
		mainCamera.orthographicSize = size;
	}
}
