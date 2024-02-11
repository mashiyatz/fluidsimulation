using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour
{
    private Vector3 position;
    private Vector3 velocity;
    private Rigidbody rb;
    private float diameter;

    [SerializeField]
    private float collisionDamping = 0.8f;

    void Start()
    {
        diameter = GetComponent<MeshRenderer>().bounds.size.x;
        rb = GetComponent<Rigidbody>();
        position = transform.position;
    }

    void Update()
    {
        velocity += ParticleSetup.gravity * Time.deltaTime * Vector3.down;
        position += velocity * Time.deltaTime;

        rb.velocity = velocity;

        if (rb.position.y < (diameter / 2))
        {
            position.y = diameter / 2 * Mathf.Sign(position.y);
            velocity.y *= -1f * collisionDamping;
        }

        if (Mathf.Abs(rb.position.x) > (FishTankSetup.boundaryWidth - diameter / 2))
        {
            position.x = diameter / 2 * Mathf.Sign(position.x);
            velocity.x *= -1f * collisionDamping;
        }

        rb.position = position;
    }
}
