using UnityEngine;

public class SpaceshipInput : MonoBehaviour
{
	public Vector2 InputVector
	{
		get
		{
			float forward = Input.GetAxisRaw("Vertical") - 1;
			forward += Input.GetAxisRaw("Turbo");

			float turn = Input.GetAxisRaw("Horizontal");
			return new Vector2(turn, forward);
		}
	}
}
