using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Controller2D : MonoBehaviour
{
	private static float fourtyFiveDegrees = Mathf.Cos(Mathf.PI/4);

    public float maxSlopeAngle = 80;

    public float skinWidth;

    public float minMoveDistance;

    public CollisionInfo collisions;

    public LayerMask collisionMask;

    [HideInInspector]
    public Vector2 playerInput;

    Collider2D collider;

    List<RaycastHit2D> hits;

    public void Start()
    {
        collider = GetComponent<Collider2D>();
        hits = new List<RaycastHit2D>();
    }

    public bool IsGrounded()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + (skinWidth * 2), collisionMask);
        return groundHit.collider != null;
    }

    public void Move(Vector2 moveAmount)
    {
        collisions = new CollisionInfo();

		if (moveAmount.magnitude < minMoveDistance) {
			return;
		}

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(collisionMask);

        Vector2 direction = moveAmount;
        direction.Normalize();

		float cosAngle = Mathf.Abs(Vector2.Dot(direction, Vector2.right));
		if (cosAngle < fourtyFiveDegrees) {
			cosAngle = Mathf.Abs(Vector2.Dot(direction, Vector2.down));
		}

		float modifiedSkinWidth = skinWidth/cosAngle;

        float distance = moveAmount.magnitude;

        int numHits = collider.Cast(direction, filter, hits, distance + modifiedSkinWidth);

        Debug.DrawRay(collider.bounds.center, direction * (distance + modifiedSkinWidth), Color.red);

        if (numHits == 0)
        {
            gameObject.transform.Translate(moveAmount);
        }
        else
        {
            RaycastHit2D minHit = hits[0];
            Debug.DrawRay(minHit.point, minHit.normal, Color.magenta);
            for (int i = 1; i < numHits; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.distance < minHit.distance)
                {
                    minHit = hit;
                }
                Debug.DrawRay(hit.point, hit.normal, Color.magenta);
            }

            Vector2 newMove = direction * (minHit.distance - modifiedSkinWidth);

            if (newMove.magnitude > minMoveDistance)
            {
                gameObject.transform.Translate(newMove);

                float collisionAngle = Vector2.Angle(minHit.normal, -1 * direction);
                if (collisionAngle > maxSlopeAngle)
                {
                    float residualDistance = distance - newMove.magnitude;
                    Vector2 residualDirection = Vector2.Perpendicular(minHit.normal);
                    residualDirection = residualDirection * Mathf.Sign(Vector2.Dot(direction, residualDirection));
                    collisions.residualMovement = residualDirection * residualDistance;
                    Debug.DrawRay(minHit.point, residualDirection, Color.black);
                }
            }

            if (collider.ClosestPoint(minHit.point - newMove).y < collider.bounds.extents.y * .02)
            {
                collisions.below = minHit;
            }
        }
    }

    public struct CollisionInfo
    {
        public RaycastHit2D? left;
        public RaycastHit2D? right;
        public RaycastHit2D? above;
        public RaycastHit2D? below;

        public Vector2? residualMovement;
    }
}