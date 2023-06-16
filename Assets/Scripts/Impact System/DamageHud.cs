using TMPro;
using UnityEngine;

public class DamageHud : MonoBehaviour
{ 
	[SerializeField] TMP_Text text;
	[SerializeField] new MeshRenderer renderer;
	[SerializeField] float fadeTime = 1f;
	[SerializeField] AnimationCurve scaleOverAgeX;
	[SerializeField] AnimationCurve scaleOverAgeY;
	[SerializeField] float minimumDamageToShow;
	[SerializeField, Range(0, 1)] float positionAtDamagePoint = 0.5f;
	[SerializeField] float maxRotation = 20f;
	[SerializeField] int orderInLayer = 1;

	float _timer;
	Vector3 _impactPosition;
	Quaternion _rotation;
	float _damage;

	public ImpactDamageHudPlayer Owner { get; set; }
	public DamageHud Prefab { get; set; }

	private void OnValidate()
	{
		if (text == null)
			text = GetComponentInChildren<TMP_Text>();
		if (renderer == null)
			renderer = GetComponentInChildren<MeshRenderer>();

		if (renderer != null)
			renderer.sortingOrder = orderInLayer;
	}


	public void OnImpact(Vector2 position, float damage)
	{
		_damage += damage;
		_impactPosition = position;
		_rotation = Quaternion.identity;
		_rotation *= Quaternion.Euler(0, 0, Random.Range(-maxRotation, maxRotation));
		renderer.sortingOrder = orderInLayer;

		string message;
		if (_damage >= minimumDamageToShow)
			message = _damage.ToString("0");
		else
			return;

		_timer = fadeTime;
		text.text = message;
	}

	private void Update()
	{
		renderer.enabled = _timer > 0;

		if (_timer > 0)
			_timer -= Time.deltaTime;

		Vector3 position = Owner == null ? _impactPosition
			: Vector2.Lerp(Owner.transform.position, _impactPosition, positionAtDamagePoint);

		transform.SetPositionAndRotation(position, _rotation);


		float scaleX = scaleOverAgeX.Evaluate(1 - _timer / fadeTime);
		float scaleY = scaleOverAgeY.Evaluate(1 - _timer / fadeTime);
		transform.localScale = new Vector3(scaleX, scaleY, 1);

		if (_timer <= 0)
		{
			_damage = 0;

			if (Owner == null)
				ImpactDamageHudPlayer.NotUsed(Prefab, this);
		}
	}

}
