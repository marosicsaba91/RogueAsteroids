using UnityEngine;

static class HelperMethods
{
	public static float GetAngleToRight(this Vector2 vector)
	{
		float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		return angle;
	}
}