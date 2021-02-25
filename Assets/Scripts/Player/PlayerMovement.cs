﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // components
    public Rigidbody rigidBody;

    // misc variables
    bool grounded;
    bool cancellingGrounded;
    public LayerMask groundLayer; // layer which represents the ground

    // constants
    const float DownForce = 10f;
    const float CancelGroundedDelay = 3f;
    const float MaxSlopeAngle = 35f;
    const float MaxSpeed = 20f;
    const float MaxSprintingSpeed = 30f;
    

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        Movement();
    }

    void Movement() {
        rigidBody.AddForce(Vector3.down * DownForce * Time.deltaTime); // keep player on the ground.

        
    }

    private void OnCollisionStay(Collision collision) {
        int layer = collision.collider.gameObject.layer;
        if (groundLayer != (groundLayer | 1 << layer)) return; // no collision with the ground, so ground check isn't possible
        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.contacts[i].normal;
            if (CalculationHelper.IsFloor(normal, MaxSlopeAngle)) {
                grounded = true;
                cancellingGrounded = false;
                CancelInvoke(nameof(StopGrounded));
            }
        }
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * CancelGroundedDelay);
        }
    }

    void StopGrounded() {
        grounded = false;
    }
}
