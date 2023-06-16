using System.Collections.Generic;
using UnityEngine;

public class ImpactDamageHudPlayer : MonoBehaviour, IImpactHandler
{
	[SerializeField] DamageHud prefab;

	static readonly Dictionary<DamageHud, List<DamageHud>> hudPool = new();

	DamageHud ownedHud;

	public void OnImpact(ImpactReceiver receiver, Vector2 direction, Vector2 position, float damage, float push)
	{
		DamageHud hud = GetHud(prefab);
		hud.Owner = this;
		hud.OnImpact(position, damage);
	}

	private DamageHud GetHud(DamageHud prefab) 
	{
		if(ownedHud != null)
			return ownedHud;

		if(!hudPool.ContainsKey(prefab))
			hudPool.Add(prefab, new List<DamageHud>());

		List<DamageHud> notUsed = hudPool[prefab];

		if (notUsed.Count > 0)
		{
			ownedHud = notUsed[0];
			notUsed.RemoveAt(0);
			return ownedHud;
		}

		DamageHud hud = Instantiate(prefab);
		hud.Prefab = prefab;
		return hud;
	}

	public void OnLostAllHealth(ImpactReceiver receiver)
	{
		if (!hudPool.ContainsKey(prefab))
			hudPool.Add(prefab, new List<DamageHud>());

		List<DamageHud> notUsed = hudPool[prefab];

		if (ownedHud != null)
		{
			notUsed.Add(ownedHud);
			ownedHud.Owner = null;
		}
	}

	internal static void NotUsed(DamageHud prefab, DamageHud damageHud)
	{
		if (!hudPool.ContainsKey(prefab))
			hudPool.Add(prefab, new List<DamageHud>());

		List<DamageHud> notUsed = hudPool[prefab];

		notUsed.Add(damageHud);
	}
}
