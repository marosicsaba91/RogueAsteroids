using System;
using UnityEngine;

[Serializable]
public class FixTicker
{
	public bool isRunning = true;
	public float virtualDeltaTime = 0.01f;
	public event Action<float> Tick;

	float _virtualTime;
	float _lastVirtualTime = 0; 

	public float VirtualTime => _virtualTime;
	public float LastVirtualTime => _lastVirtualTime;

	public void Reset()
	{
		_virtualTime = Time.time; 
	}

	public void Update()
	{ 

		if(virtualDeltaTime <= 0)
		{
			Debug.LogError("virtualDeltaTime must be greater than 0");
			return;
		}

		if (isRunning)
		{
			while (Time.time > _virtualTime)
			{
				OnTick();
				_virtualTime += virtualDeltaTime;
			}
		}
		else if (Time.time > _virtualTime)
		{
			_virtualTime = Time.time ;
		}

	}

	void OnTick()
	{ 
		float virtualTimeDelay = _virtualTime - Time.time;
		Tick?.Invoke(virtualTimeDelay);
		_lastVirtualTime = _virtualTime;
	}
}
