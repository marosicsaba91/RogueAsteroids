using UnityEngine;

public class SpaceshipInput : MonoBehaviour
{
	[SerializeField] KeyCode forwardInput = KeyCode.UpArrow;
	[SerializeField] KeyCode turboButton = KeyCode.LeftShift;
	[SerializeField] KeyCode turnLeftInput = KeyCode.LeftArrow;
	[SerializeField] KeyCode turnRightInput = KeyCode.RightArrow;

	public Vector2 InputVector
	{
		get
		{
			float forward =
				!Input.GetKey(forwardInput) ? -1 :
				Input.GetKey(turboButton) ? 1 : 0;

			float turn = Input.GetKey(turnRightInput) ? 1 : Input.GetKey(turnLeftInput) ? -1 : 0;
			return new Vector2(turn, forward);
		}
	}
}
