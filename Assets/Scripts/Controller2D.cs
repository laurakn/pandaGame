using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : RaycastController
{

    public float maxSlopeAngle = 80;

    Rigidbody2D rigidbody;

    RaycastController raycastController;

    Collisions previousCollisions = new Collisions();

    public override void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        raycastController = GetComponent<RaycastController>();
        base.Start();
    }

    public Vector2 GetPosition()
    {
        return rigidbody.position;
    }

    public Collisions Move(Vector2 moveAmount)
    {
        Collisions collisions = GetCollisions(moveAmount);

        Vector2 modifiedMoveAmount = moveAmount;

        RaycastHit2D? facingCollision = GetFacingCollision(collisions, moveAmount);
        if (facingCollision.HasValue)
        {
			if (facingCollision.Value.distance == 0) {
				return previousCollisions;
			}
            Vector2 normal = facingCollision.Value.normal;
            float slopeAngle = Vector2.Angle(normal, Vector2.up);
            if (slopeAngle <= maxSlopeAngle)
            {
                float moveDistance = Mathf.Abs(moveAmount.x);
                Vector2 slopeMove = Mathf.Sign(normal.x) * Vector2.Perpendicular(normal * moveDistance);
                modifiedMoveAmount.x = 0;
                modifiedMoveAmount += slopeMove;
            }
            else
            {
                modifiedMoveAmount.x = Mathf.Sign(moveAmount.x) * (facingCollision.Value.distance - skinWidth);
            }
        }

        if (moveAmount.y > 0 && collisions.top.HasValue)
        {
			if (collisions.top.Value.distance == 0) {
				return previousCollisions;
			}
            modifiedMoveAmount.y = collisions.top.Value.distance - skinWidth;
        }
        else if (moveAmount.y < 0 && collisions.bottom.HasValue)
        {
			if (collisions.bottom.Value.distance == 0) {
				return previousCollisions;
			}
            Vector2 normal = collisions.bottom.Value.normal;
            float slopeAngle = Vector2.Angle(normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle)
            {
                float moveDistance = moveAmount.y;
                Vector2 slopeMove = Mathf.Sign(normal.x) * Vector2.Perpendicular(normal * moveDistance);
                modifiedMoveAmount.y = 0;
                modifiedMoveAmount += slopeMove;
                collisions.bottom = null;
            }
            else
            {
                modifiedMoveAmount.y = -1 * (collisions.bottom.Value.distance - skinWidth);
            }
        }

        /** Downslope walking needs work
                if (moveAmount.x != 0 && moveAmount.y < 0 && previousCollisions.bottom.HasValue && !collisions.bottom.HasValue)
                {
                    Vector2 normal = previousCollisions.bottom.Value.normal;
                    float slopeAngle = Vector2.Angle(normal, Vector2.up);
                    if (slopeAngle <= maxSlopeAngle)
                    {
                        RaycastHit2D? toSlope = GetDownwardCollision(skinWidth*2, GetRaycastOrigins(Vector2.zero));
                        if (toSlope.HasValue) {
                            modifiedMoveAmount.y = -1 * toSlope.Value.distance;
                            collisions.bottom = toSlope;
                        }
                    }
                }
                */

        // If we're trying to move down and there's a top collision,
        // then we shouldn't move into this location because the ceiling is too low
        if (!collisions.top.HasValue || MovingAwayFromCeiling(collisions, modifiedMoveAmount))
        {
            rigidbody.MovePosition(rigidbody.position + modifiedMoveAmount);
        }

        previousCollisions = collisions;
        return collisions;
    }

    private bool MovingAwayFromCeiling(Collisions collisions, Vector2 moveAmount)
    {
        if (!collisions.bottom.HasValue)
        {
            return true;
        }

        if (collisions.top.Value.normal.x == 0)
        {
            if (moveAmount.x == 0)
            {
                return false;
            }

            // If the ceiling is flat then check the ground slope
            return Mathf.Sign(collisions.bottom.Value.normal.x) == Mathf.Sign(moveAmount.x);
        }

        return Mathf.Sign(collisions.top.Value.normal.x) == Mathf.Sign(moveAmount.x);
    }

    private RaycastHit2D? GetFacingCollision(Collisions collisions, Vector2 moveAmount)
    {
        if (moveAmount.x == 0)
        {
            return null;
        }

        return moveAmount.x > 0 ? collisions.right : collisions.left;
    }
}
