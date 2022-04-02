using Assets.Scripts.LD50.Interact.Items;
using LD50.Interact.Items;
using System;
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

    [Header("Interactions")]
    [SerializeField]
    private float interactionMaxDistance = 1.0f;
    public float InteractionMaxDistance => interactionMaxDistance;
    public Vector3 MovementDirection
    {
        get => movementDirection;
        set => movementDirection = value;
    }

    public Rigidbody2D Rigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();
            return _rigidbody;
        }
    }
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private List<Item> inventory;
    public List<Item> Inventory => inventory;

    public bool RemoveItemFromInventory (Item item)
    {
        if (item == null) return false;

        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            return true;
        }
        return false;
    }

    public void AddItemToInteventory (Item item)
    {
        if(item == null) return;

        inventory.Add(item);
    }
}
