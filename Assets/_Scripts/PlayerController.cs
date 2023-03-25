using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    //레이가 일정부분 GameObject 안쪽에서 발사되기 위한 여분의 너비
    const float skinWidth = .015f;

    //수평 및 수직으로 발사될 광선의 갯수
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public CollisionInfo collisions;
    //Space between Each Ray
    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D boxCld2D;
    RaycastOrigins raycastOrigins;

    public LayerMask collisionMask;


    //광선 구조체
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    // Start is called before the first frame update
    void Awake()
    {
        boxCld2D = GetComponent<BoxCollider2D>();
        RaySpacing();
    }


    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();

        collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        //Draw Ray
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigins = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigins += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigins, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigins, Vector2.right * directionX * rayLength, Color.red);

            if (rayHit)
            {
                velocity.x = (rayHit.distance - skinWidth) * directionX;
                rayLength = rayHit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        //Draw Ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigins = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigins += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D rayHit = Physics2D.Raycast(rayOrigins, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigins, Vector2.up * directionY * rayLength, Color.red);

            if (rayHit)
            {
                velocity.y = (rayHit.distance-skinWidth) * directionY;
                rayLength = rayHit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    

    void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCld2D.bounds;
        bounds.Expand(skinWidth * -2);

        //광선의 원점 설정
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    //Calculate Ray Spacing
    void RaySpacing()
    {
        Bounds bounds = boxCld2D.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);

    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;

        }
    }

    // Update is called once per frame
    
}
