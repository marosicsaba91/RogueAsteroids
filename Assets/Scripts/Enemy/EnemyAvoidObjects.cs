
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidObjects : MonoBehaviour
{
	static readonly List<EnemyAvoidObjects> _toAvoid = new();

	[SerializeField] float push = 2;
	[SerializeField] float range = 2;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}

	void OnEnable() => _toAvoid.Add(this);

	void OnDisable() => _toAvoid.Remove(this);

	public static Vector3 GetPushVector(Vector3 position, GameObject ignore) 
	{
		Vector3 result = Vector3.zero;
		foreach (EnemyAvoidObjects avoid in _toAvoid)
		{ 
			if (avoid == null) continue;
			if (avoid.gameObject == ignore) continue;
			Vector3 avoidable = avoid.transform.position; 
			Vector3 distanceVector = position - avoidable;
			float distance = distanceVector.magnitude;
			if (distance < avoid.range)
			{
				Vector3 direction = distanceVector / distance;
				float relativeDistance = distance / avoid.range;
				result += direction * (avoid.push * relativeDistance);
			}
		}
		return result;
	} 
}
