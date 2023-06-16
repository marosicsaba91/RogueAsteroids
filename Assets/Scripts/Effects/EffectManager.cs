using UnityEngine;

class EffectManager : SingletonMonoBehaviour<EffectManager>
{  
	public static void Play(Effect effect, Vector2 position, float angle, Transform parent = null)
	{
		if (effect == null) return; 
		Effect newEffect = Instantiate(effect, position, Quaternion.Euler(0, 0, angle));

		if (parent != null)
			newEffect.transform.SetParent(parent, true);
	}
}
