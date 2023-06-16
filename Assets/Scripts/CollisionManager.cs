using System.Collections.Generic;
using UnityEngine;

class CollisionManager : SingletonMonoBehaviour<CollisionManager>
{
	static ContactFilter2D _contactFilter = new();

	static readonly List<(Collider2D, Collider2D)> _twins = new();
	static readonly Collider2D[] _overlapTest = new Collider2D[25];
	 

	protected sealed override void SingletonAwake()
	{
		_contactFilter.NoFilter();
	}

	void Update()
	{ 
		for (int i = _twins.Count - 1; i >= 0; i--)
		{
			Collider2D twin1 = _twins[i].Item1;
			Collider2D twin2 = _twins[i].Item2;
			if (twin1 == null || twin2 == null)
			{
				_twins.RemoveAt(i);
				continue;
			}

			if (!Overlap(twin1, twin2)) 
			{
				Physics2D.IgnoreCollision(twin1, twin2, false);
				_twins.RemoveAt(i);
			}
		}
	}

	public static void OnEnableColliders(List<Collider2D> colliders)
	{ 
		for (int i = 0; i < colliders.Count; i++)
		{
			for (int j = i+1; j < colliders.Count; j++)
			{
				Collider2D c1 = colliders[i];
				Collider2D c2 = colliders[j];
				if (Overlap(c1, c2))
				{
					_twins.Add((c1, c2));
					Physics2D.IgnoreCollision(c1, c2, true);
				}
			} 
		}
	}

	static bool Overlap(Collider2D c1, Collider2D c2)
	{
		int n = Physics2D.OverlapCollider(c1, _contactFilter, _overlapTest);

		for (int k = 0; k < n; k++)
		{
			if (_overlapTest[k].gameObject == c2.gameObject)
				return true;
		}

		return false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		for (int i = _twins.Count - 1; i >= 0; i--)
		{
			Collider2D twin1 = _twins[i].Item1;
			Collider2D twin2 = _twins[i].Item2;
			if (twin1 == null || twin2 == null)
				continue;

			Gizmos.DrawLine(twin1.transform.position, twin2.transform.position);

		}
	}
}
