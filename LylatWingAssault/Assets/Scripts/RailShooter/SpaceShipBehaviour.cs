using UnityEngine;
using Cinemachine;

public class SpaceShipBehaviour : MonoBehaviour
{
    [Header("Public References")]
    public Transform _playerModel;
    public Transform aimTarget;
    public Transform cameraParent;
    public CinemachineDollyCart dolly;

    public float xyspeed = 16f;
    public float lookSpeed = 340f;
    public float forwardSpeed = 6f;

    public bool joystick = true;

    public float leanMultiplier = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        _playerModel = transform.GetChild(0);
        SetSpeed(forwardSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        float h = joystick ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float v = joystick ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");

        //Debug.Log(h + " horizontal + " + v + " vertical");

        LocalMove(h, v, xyspeed);
        RotationLook(h, v, lookSpeed);
        HorizontalLean(_playerModel, h, 45, .1f);
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = Vector3.Slerp(aimTarget.localPosition, new Vector3(h* leanMultiplier, v * leanMultiplier, 1.5f), Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    private void SetSpeed(float x)
    {
        dolly.m_Speed = x;
    }

    private void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);

    }
}
