using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class SpaceShipBehaviour : MonoBehaviour
{
    [Header("References")]
    public Transform _playerModel;
    public Transform aimTarget;
    public Transform cameraParent;
    public CinemachineDollyCart dolly;
    [Space]
    public GameObject bulletPrefab;
    public Transform shootOrigin;

    [Header("Variables")]
    public float xyspeed = 16f;
    public float lookSpeed = 340f;
    public float forwardSpeed = 6f;

    public bool joystick = true;

    public float leanMultiplier = 1.5f;

    [SerializeField] private bool endOfTrackReached = false;

    [Header("Events")]
    public UnityEvent onDollyTrackEnded;


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


        LocalMove(h, v, xyspeed);
        RotationLook(h, v, lookSpeed);
        HorizontalLean(_playerModel, h, 45, 1f);

        DollyTrackCheck();

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
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
        aimTarget.localPosition = Vector3.Slerp(aimTarget.localPosition, new Vector3(h* leanMultiplier, v * leanMultiplier, 2f), Time.deltaTime);
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

    private void Shoot()
    {
        Instantiate(bulletPrefab, shootOrigin.position, shootOrigin.rotation);
    }

    //# Put this on a seperate script on the dolly itself, not on the player*
    private void DollyTrackCheck()
    {
        if (dolly.m_Position >= dolly.m_Path.PathLength - 5 && !endOfTrackReached)
        {
            endOfTrackReached = true;
            Debug.Log("End of the line kid");
            onDollyTrackEnded.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);
    }
}
