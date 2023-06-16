using System; 
using UnityEngine; 

class AsteroidsGameManager : SingletonMonoBehaviour<AsteroidsGameManager>
{
	int _level;
	public static event Action RestartGame; 
	public static event Action<int> NewLevelLoaded;

	public static int Level => Instance._level; 

	void Start()
	{
		OnRestartGame();
	}
	 
	static void OnRestartGame()
	{
		Instance._level = 1;
		RestartGame?.Invoke();
		Instance.StartLevel();
	}

	public static void NextLevel()
	{		
		Instance._level++;
		Instance.StartLevel();
	}

	void StartLevel()
	{
		if (ApplicationHelper.IsQuitting)
		{
			Debug.Log("Quitting");
			return;
		}
		NewLevelLoaded.Invoke(_level); 
	}

	internal static void DoRestartGame() 
	{
		OnRestartGame();
	}
}
