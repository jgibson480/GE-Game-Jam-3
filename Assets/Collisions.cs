using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{    
    [SerializeField]
    LayerMask platformsLayerMask;

    [SerializeField]
    LayerMask wallsLayerMask;

    [SerializeField]
    Vector2 v2GroundCollisionBoxOffset;
    [SerializeField]
    Vector2 v2GroundCollisionSize;
   
    public bool bGrounded;

    [SerializeField]
    Vector2 v2LeftWallCollisionBoxOffset;
    [SerializeField]
    Vector2 v2RightWallCollisionBoxOffset;
    [SerializeField]
    Vector2 v2WallCollisionSize;

    public bool bOnWall;
    public bool bOnRightWall;
    public bool bOnLeftWall;

    // Update is called once per frame
    void Update()
    {
        Vector2 v2GroundedBoxCheckPosition = (Vector2)transform.position + v2GroundCollisionBoxOffset;
        Vector2 v2GroundedBoxCheckScale = (Vector2)transform.localScale + v2GroundCollisionSize;
        bGrounded = Physics2D.OverlapBox(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, platformsLayerMask);

        Vector2 v2LeftWallBoxCheckPosition = (Vector2)transform.position + v2LeftWallCollisionBoxOffset;
        Vector2 v2RightWallBoxCheckPosition = (Vector2)transform.position + v2RightWallCollisionBoxOffset;
        Vector2 v2WallBoxCheckScale = (Vector2)transform.localScale + v2WallCollisionSize;
        bOnLeftWall = Physics2D.OverlapBox(v2LeftWallBoxCheckPosition, v2WallBoxCheckScale, 0, wallsLayerMask);
        bOnRightWall = Physics2D.OverlapBox(v2RightWallBoxCheckPosition, v2WallBoxCheckScale, 0, wallsLayerMask);
        bOnWall = bOnLeftWall ^ bOnRightWall;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube((Vector2)transform.position + v2GroundCollisionBoxOffset, (Vector2)transform.localScale + v2GroundCollisionSize);
        Gizmos.DrawCube((Vector2)transform.position + v2LeftWallCollisionBoxOffset, (Vector2)transform.localScale + v2WallCollisionSize);
        Gizmos.DrawCube((Vector2)transform.position + v2RightWallCollisionBoxOffset, (Vector2)transform.localScale + v2WallCollisionSize);
    }
}
