using UnityEngine;

public class ScreenTeleport : MonoBehaviour
{
	[SerializeField] SpriteRenderer shapeRenderer;
	[SerializeField] Collider2D shapeCollider;

	[SerializeField] bool preferCollider = true;

	Camera cam;

	void Start()
	{
		cam = Camera.main;
	}

	private void OnValidate()
	{
		if (shapeRenderer == null)
			shapeRenderer = GetComponentInChildren<SpriteRenderer>();
		if (shapeCollider == null)
			shapeCollider = GetComponentInChildren<Collider2D>();
	}

	void LateUpdate()
	{
		Physics2D.SyncTransforms();

		if (!TryGetShape(out Bounds shape))
			return;

		Rect shipRect = new(shape.min, shape.size);
		Rect cameraRect = CameraRect;


		Vector2 shipCenter = shipRect.center;

		if (shipCenter.x < cameraRect.xMin - shipRect.size.x / 2)
		{
			transform.position += new Vector3(cameraRect.size.x + shipRect.width, 0, 0);
			OnTeleport();
		}
		else if (shipCenter.x > cameraRect.xMax + shipRect.size.x / 2)
		{
			transform.position -= new Vector3(cameraRect.size.x + shipRect.width, 0, 0);
			OnTeleport();
		}

		if (shipCenter.y < cameraRect.yMin - shipRect.size.y / 2)
		{
			transform.position += new Vector3(0, cameraRect.size.y + shipRect.height, 0);
			OnTeleport();
		}
		else if (shipCenter.y > cameraRect.yMax + shipRect.size.y / 2)
		{
			transform.position -= new Vector3(0, cameraRect.size.y + shipRect.height, 0);
			OnTeleport();
		}

	}

	bool TryGetShape(out Bounds shape)
	{
		if (preferCollider && shapeCollider != null)
			shape = shapeCollider.bounds;
		else if (shapeRenderer != null)
			shape = shapeRenderer.bounds;
		else
		{
			shape = default;
			return false;
		}
		return true;
	}

	void OnTeleport()
	{
		foreach (ITeleportHandler handler in GetComponents<ITeleportHandler>())
			handler.OnTeleport();
	}

	Rect CameraRect
	{
		get
		{
			Rect cameraRect = new(
				cam.transform.position.x - cam.orthographicSize * cam.aspect,
				cam.transform.position.y - cam.orthographicSize,
				cam.orthographicSize * 2 * cam.aspect,
				cam.orthographicSize * 2);
			return cameraRect;
		}
	}

	void OnDrawGizmos()
	{
		if (TryGetShape(out Bounds shape))
		{
			Rect shipRect = new(shape.min, shape.size);
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(shipRect.center, shipRect.size);
		}

		if (cam != null)
		{
			Rect cameraRect = CameraRect;
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(cameraRect.center, cameraRect.size);
		}
	}
}
