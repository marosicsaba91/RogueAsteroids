using UnityEngine;

interface IGun
{
	float Damage { get; }
	float Duration { get; }
	float TargetSpeed { get; }
	float Push { get; }
	float Magnetism { get; }
	Transform Transform { get; }
	ImpactGroup ImpactGroup { get; }
}
