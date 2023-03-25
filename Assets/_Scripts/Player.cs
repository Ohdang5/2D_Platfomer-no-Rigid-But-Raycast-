using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class Player : MonoBehaviour
{
    PlayerController playerCtlr;
    public float moveSpeed = 6;

    public float jumpH = 4;
    public float timeTojumpApex = .4f;

    float gravity;
    float jumpPower;
    Vector3 velocity;

    void Awake()
    {
        playerCtlr = GetComponent<PlayerController>();
        gravity = -(2 * jumpH) / Mathf.Pow(timeTojumpApex, 2);
        jumpPower = Mathf.Abs(gravity) * timeTojumpApex;
        print("Gravity: " + gravity + "Jump Power: " + jumpPower);
    }
    void Update()
    {
        if (playerCtlr.collisions.above || playerCtlr.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && playerCtlr.collisions.below)
        {
            velocity.y = jumpPower;
        }
            velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        playerCtlr.Move(velocity * Time.deltaTime);
    }
}
