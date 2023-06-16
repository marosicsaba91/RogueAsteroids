using System.Collections.Generic;
using UnityEngine;

class MagnetTarget : MonoBehaviour
{
	[SerializeField] float maxMagnetForce = 1;
	[SerializeField] float magnetRange = 5;

	static readonly List<MagnetTarget> _targets = new();

	public static IReadOnlyList<MagnetTarget> Targets => _targets;
	void OnEnable()
	{
		_targets.Add(this);
	}

	void OnDisable()
	{
		_targets.Remove(this);
	}


	public static MagnetTarget GetClosest(Vector2 position, out float distance)
	{
		MagnetTarget closest = null;
		float closestSqrDistance = float.MaxValue;
		foreach (MagnetTarget target in Targets)
		{
			float sqrDistance = (position - (Vector2)target.transform.position).sqrMagnitude;
			if (sqrDistance < closestSqrDistance)
			{
				closest = target;
				closestSqrDistance = sqrDistance;
			}
		}

		distance = Mathf.Sqrt(closestSqrDistance);
		return closest;
	}

	public static Vector2 ForceVectorAt(Vector2 position)
	{
		Vector2 force = Vector2.zero;
		foreach (MagnetTarget target in Targets)
		{
			Vector2 direction = (Vector2)target.transform.position - position;
			float sqrDistance = direction.sqrMagnitude;
			if (sqrDistance == 0)
				continue; // skip self (or skip zero distance?)

			float sqrMagnetRange = target.magnetRange * target.magnetRange;
			if (sqrDistance < sqrMagnetRange)
			{
				float distance = Mathf.Sqrt(sqrDistance);
				float currentForce = 1 - (distance / target.magnetRange);
				// currentForce *= currentForce;
				currentForce *= target.maxMagnetForce;
				force += direction * (currentForce / distance);
			}
		}
		return force;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, magnetRange);
	}

}