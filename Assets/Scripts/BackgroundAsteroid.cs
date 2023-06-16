using System.Collections.Generic;
using UnityEngine;

public class BackgroundAsteroid : MonoBehaviour
{
	[SerializeField] List<Sprite> sprites;
	[SerializeField] float minSpeed = 0.5f;
	[SerializeField] float maxSpeed = 1.5f;
	[SerializeField] float minAngularSpeed = 50;
	[SerializeField] float maxAngularSpeed = 130;

	[SerializeField] AnimationCurve scaleMultiplier;
	[SerializeField] AnimationCurve speedMultiplier;
	[SerializeField] int minSortingOrder;
	[SerializeField] int maxSortingOrder;
	[SerializeField] Color minColor;
	[SerializeField] Color maxColor;


	Vector3 _velocity;
	float angularSpeed;

    void Start()
    {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];

		float r = Random.value;
		float scale = scaleMultiplier.Evaluate(r);
		transform.localScale = new Vector3(scale, scale, 1);
		float speed = speedMultiplier.Evaluate(r);
		spriteRenderer.sortingOrder = Mathf.RoundToInt(Mathf.Lerp(minSortingOrder, maxSortingOrder, r));
		spriteRenderer.color = Color.Lerp(minColor, maxColor, r);



		Vector2 random01 = new(Random.value, Random.value);
		Vector2 worldPos = Camera.main.ViewportToWorldPoint(random01);
		transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
		
		Vector3 randomDirection = Random.insideUnitCircle.normalized;
		_velocity = randomDirection * (speed * Random.Range(minSpeed, maxSpeed));
		angularSpeed = Random.Range(minAngularSpeed, maxAngularSpeed);
    }

	void Update()
	{
		transform.localPosition += _velocity * Time.deltaTime;
		transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);		
	}
}
