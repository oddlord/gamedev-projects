using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        Transform pulledObjectTransform = other.transform;
        pulledObjectTransform.position = new Vector3(pulledObjectTransform.position.x, pulledObjectTransform.position.y + 0.1f, transform.position.z);
    }
}
