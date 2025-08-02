using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour
{
    private const float G = 667.4f;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        foreach (Attractable attractable in Attractable.Attractables)
        {
            Attract(attractable);
        }
    }

    void Attract(Attractable attractable)
    {
        Rigidbody attractableRb = attractable.rb;

        Vector3 direction = _rb.position - attractableRb.position;
        float distance = direction.magnitude;

        if (distance == 0f)
        {
            return;
        }

        float forceMagnitude = G * (_rb.mass * attractableRb.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        attractableRb.AddForce(force);
    }
}
