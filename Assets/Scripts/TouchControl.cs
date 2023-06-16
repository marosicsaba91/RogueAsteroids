using System;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
	[SerializeField] float maxForwardDistanceInCm = 1;
	[SerializeField] float maxTurnDistanceInCm = 1;

	public float Forward { get; private set; }
	public float Turn { get; private set; }
	public event Action Fire;

	int _movementIndex = -1;
	Vector2 _startPositionInPixel;
	Vector2 _offsetInCm;

	void Update()
	{
		GetMovementControl();
		GetFireControl();

		Forward = Mathf.Clamp(_offsetInCm.y, 0, maxForwardDistanceInCm);
		Forward /= maxForwardDistanceInCm;
		Turn = Mathf.Clamp(_offsetInCm.x, -maxTurnDistanceInCm, maxTurnDistanceInCm) / maxTurnDistanceInCm;
		Turn /= maxTurnDistanceInCm;
	}

	void GetMovementControl()
	{
		if (_movementIndex < 0)
		{
			for (int index = 0; index < Input.touches.Length; index++)
			{
				// START
				Touch touch = Input.touches[index];
				if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2f)
				{
					_movementIndex = index;
					_startPositionInPixel = touch.position;
				}
			}
		}
		else
		{
			// EXIT
			Touch movement = Input.GetTouch(_movementIndex);
			if (movement.phase is TouchPhase.Ended or TouchPhase.Canceled)
			{
				_movementIndex = -1;
				_offsetInCm = Vector2.zero;
			}
			else
			{
				// CONTROL
				_offsetInCm = movement.position - _startPositionInPixel;
				_offsetInCm /= Screen.dpi;
				const float inch2Cm = 2.54f;
				_offsetInCm *= inch2Cm;
			}
		}
	}

	void GetFireControl()
	{
		for (int index = 0; index < Input.touches.Length; index++)
		{
			if (index == _movementIndex)
				continue;
			Touch touch = Input.touches[index];
			if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2f)
				Fire?.Invoke();
		}
	}
}