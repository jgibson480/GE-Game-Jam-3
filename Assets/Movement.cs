using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float fJumpVelocity = 12;

    Rigidbody2D rigid;

    float fJumpPressedRemember = 0;
    [SerializeField]
    float fJumpPressedRememberTime = 0.2f;

    float fGroundedRemember = 0;
    [SerializeField]
    float fGroundedRememberTime = 0.25f;

    [SerializeField]
    float fHorizontalAcceleration = 1;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingBasic = 0.2f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenStopping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float fHorizontalDampingWhenTurning = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    float fCutJumpHeight = 0.5f;

    public Collisions collisions_script;

    public bool bWallGrab;
    public bool bCanWallGrab;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }
    void Move()
    {
        float fHorizontalVelocity = rigid.velocity.x;
        float fVerticalVelocity = rigid.velocity.y;
        fHorizontalVelocity += Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
            fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * 5f);
        else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(fHorizontalVelocity))
            fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * 5f);
        else
            fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingBasic, Time.deltaTime * 5f);

        if(bWallGrab == true)
        {
            rigid.gravityScale = 0f;
            fVerticalVelocity = 0f;
            fHorizontalVelocity = 0f;
        }
        else
            rigid.gravityScale = 2f;

        if (Input.GetButton("Wall Grab") & collisions_script.bOnWall && bCanWallGrab)
            bWallGrab = true;
        else
            bWallGrab = false;

        rigid.velocity = new Vector2(fHorizontalVelocity, fVerticalVelocity) * fHorizontalAcceleration;
        Debug.Log(rigid.velocity);
    }

    void Jump()
    {
        fGroundedRemember -= Time.deltaTime;
        if (collisions_script.bGrounded || collisions_script.bOnWall)
        {
            fGroundedRemember = fGroundedRememberTime;
        }

        if(collisions_script.bOnWall && Input.GetButtonDown("Jump"))
            StartCoroutine(WallJumpDisableMovement(0.2f));

        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (rigid.velocity.y > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * fCutJumpHeight);
            }
        }

        if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, fJumpVelocity);
        }
    }

    IEnumerator WallJumpDisableMovement(float time)
    {
        bWallGrab = false;
        bCanWallGrab = false;
        yield return new WaitForSeconds(time);
        bCanWallGrab = true;
    }

}