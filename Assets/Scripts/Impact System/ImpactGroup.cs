
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Asteroids/Asteroids ImpactGroup", fileName = "Asteroids ImpactGroup")]
public class ImpactGroup : ScriptableObject
{
	[Serializable]
	struct Multiplier
	{
		[FormerlySerializedAs("team")] public ImpactGroup impactGroup;
		public float multiplier;
	}
	
	[SerializeField] List<ImpactGroup> interactList = new();
	[SerializeField] List<Multiplier> damageMultipliers = new();
	[SerializeField] List<Multiplier> pushMultipliers = new();

	public bool CanInteract(ImpactGroup team) => interactList.Contains(team);
	public float DamageMultiplier(ImpactGroup team)
	{
		foreach (Multiplier m in damageMultipliers)
			if (m.impactGroup == team)
				return m.multiplier;
		return 1;
	}

	public float PushMultiplier(ImpactGroup team)
	{
		foreach (Multiplier m in pushMultipliers)
			if (m.impactGroup == team)
				return m.multiplier;
		return 1;
	}
}
