using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractable : MonoBehaviour
{
    public static List<Attractable> Attractables;

    [HideInInspector]
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        if (Attractables == null)
        {
            Attractables = new List<Attractable>();
        }

        Attractables.Add(this);
    }

    void OnDisable()
    {
        Attractables.Remove(this);
    }
}
