using UnityEngine;

class AdaptiveDrag : MonoBehaviour
{
	[SerializeField] new Rigidbody2D rigidbody2D;
	public AnimationCurve dragOverVelocity;
	public AnimationCurve angularDragOverVelocity;

	void OnValidate()
	{
		if (rigidbody2D == null)
			rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		float av = Mathf.Abs(rigidbody2D.angularVelocity);
		rigidbody2D.angularDrag = angularDragOverVelocity.Evaluate(av);
		float v = rigidbody2D.velocity.magnitude;
		rigidbody2D.drag = dragOverVelocity.Evaluate(v);
	}
}