using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField]
    private float walkSpeed = 1.0f;
    public float WalkSpeed => walkSpeed;

    [Header("Movement")]
    [SerializeField]
    private Vector3 movementDirection;
    public Vector3 MovementDirection
    {
        get => movementDirection;
        set => movementDirection = value;
    }

    public Rigidbody2D Rigidbody
    {
        get
        {
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();
            return rigidbody;
        }
    }
    private Rigidbody2D rigidbody;
}
