using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float jumpForce = 40000f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float cameraRotationLimit = 85f;

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Spring settings")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 jumpVelocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    private Rigidbody rb;
    private ConfigurableJoint joint;

    void Start() {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    private void Update () {
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        velocity = (_movHorizontal + _movVertical) * speed;

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask)) {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        } else {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        jumpVelocity = Vector3.zero;
        if (Input.GetButtonDown("Jump")) {
            jumpVelocity = Vector3.up * jumpForce;
            SetJointSettings(0f);
        } else {
            SetJointSettings(jointSpring);
        }

        float _yRot = Input.GetAxisRaw("Mouse X");

        rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        float _xRot = Input.GetAxisRaw("Mouse Y");

        cameraRotationX = _xRot * lookSensitivity;
    }

    private void FixedUpdate() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (jumpVelocity != Vector3.zero) {
            rb.AddForce(jumpVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null) {
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    private void SetJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
