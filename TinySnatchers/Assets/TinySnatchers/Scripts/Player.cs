using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float YAW_ROTATION = 100f;
    private const float SPEED = 20f;

    [SerializeField] private TractorBeam _tractorBeam;

    void Update()
    {
        float yaw = Input.GetAxis("Horizontal");
        float speed = Input.GetAxis("Vertical");
        float tractorBeam = Input.GetAxis("Fire1");

        transform.Rotate(transform.up, yaw * YAW_ROTATION * Time.deltaTime, Space.World);
        transform.RotateAround(Vector3.zero, transform.right, speed * SPEED * Time.deltaTime);

        if (tractorBeam == 1)
        {
            _tractorBeam.gameObject.SetActive(true);
        }
        else
        {
            _tractorBeam.gameObject.SetActive(false);
        }
    }
}
