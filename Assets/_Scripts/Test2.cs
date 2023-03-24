using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class Player : MonoBehaviour
{
    PlayerController playerCtlr;
    public float moveSpeed = 6;
    public float gravity = -20;
    Vector3 velocity;

    void Awake()
    {
        playerCtlr = GetComponent<PlayerController>();
    }
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        playerCtlr.Move(velocity * Time.deltaTime);
    }
}
