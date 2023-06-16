using UnityEngine;

class SingletBase : MonoBehaviour
{
}

class SingletonMonoBehaviour<T> : SingletBase where T : SingletBase
{
	protected static T Instance { get; private set; }

	protected virtual void Awake()
	{
		if (Instance == null)
		{
			Instance = (T)(SingletBase)this;
		}
		else
		{
			Debug.LogError("SingletonMonoBehaviour: More than one instance of a singleton class detected.");
		}
		SingletonAwake();
	}

	protected virtual void SingletonAwake() { }
}
