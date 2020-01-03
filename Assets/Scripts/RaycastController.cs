using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{

    public LayerMask collisionMask;

    public float skinWidth = .015f;
    const float dstBetweenRays = .15f;
    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

	public Collisions GetCollisions(Vector2 move) {
		// TODO: Look into this... not sure that this will always work
		RaycastOrigins newOrigins = GetRaycastOrigins(move);
		Collisions collisions = new Collisions();

		collisions.top = GetUpwardCollision(Mathf.Max(move.y, 0), newOrigins);
		collisions.bottom = GetDownwardCollision(Mathf.Abs(Mathf.Min(move.y, 0)), newOrigins);
		collisions.right = GetRightCollision(Mathf.Max(move.x, 0), newOrigins);
		collisions.left = GetLeftCollision(Mathf.Abs(Mathf.Min(move.x, 0)), newOrigins);

		return collisions;
	}

    private RaycastHit2D? GetUpwardCollision(float distance, RaycastOrigins origins)
    {
		return GetVerticalCollision(distance, origins, Vector2.up);
	}

    protected RaycastHit2D? GetDownwardCollision(float distance, RaycastOrigins origins)
    {
		return GetVerticalCollision(distance, origins, Vector2.down);
	}

    private RaycastHit2D? GetVerticalCollision(float distance, RaycastOrigins origins, Vector2 direction)
    {
        RaycastHit2D? nearestHit = null;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = direction.Equals(Vector2.up) ? origins.topLeft : origins.bottomLeft;
            rayOrigin.x += verticalRaySpacing * i;

            Debug.DrawRay(rayOrigin, direction, Color.blue);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance + skinWidth, collisionMask);
            if (hit.collider != null && (!nearestHit.HasValue || hit.distance < nearestHit.Value.distance))
            {
                nearestHit = hit;
            }
        }

        if (nearestHit.HasValue)
        {
            Debug.DrawRay(nearestHit.Value.point, nearestHit.Value.normal, Color.white);
        }

        return nearestHit;
    }

	private RaycastHit2D? GetLeftCollision(float distance, RaycastOrigins origins) {
		return GetHorizontalCollision(distance, origins, Vector2.left);
	}

	private RaycastHit2D? GetRightCollision(float distance, RaycastOrigins origins) {
		return GetHorizontalCollision(distance, origins, Vector2.right);
	}

    private RaycastHit2D? GetHorizontalCollision(float distance, RaycastOrigins origins, Vector2 direction)
    {
        RaycastHit2D? nearestHit = null;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = direction.Equals(Vector2.right) ? origins.topRight : origins.topLeft;
            rayOrigin.y -= horizontalRaySpacing * i;

            Debug.DrawRay(rayOrigin, direction, Color.magenta);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance + skinWidth, collisionMask);
            if (hit.collider != null && (nearestHit == null || hit.distance < nearestHit.Value.distance))
            {
                nearestHit = hit;
            }
        }

        if (nearestHit.HasValue)
        {
            Debug.DrawRay(nearestHit.Value.point, nearestHit.Value.normal, Color.black);
        }

        return nearestHit;
    }

    public RaycastOrigins GetRaycastOrigins(Vector2 move)
    {
		CalculateRaySpacing();

        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        RaycastOrigins newOrigins = new RaycastOrigins();
        newOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y) + move;
        newOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y) + move;
        newOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y) + move;
        newOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y) + move;

		return newOrigins;
    }

    public void UpdateRaycastOrigins()
    {
		CalculateRaySpacing();

        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = 5; // Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = 3; // Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

	public struct Collisions {
		public RaycastHit2D? top;
		public RaycastHit2D? bottom;
		public RaycastHit2D? left;
		public RaycastHit2D? right;
	}

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
