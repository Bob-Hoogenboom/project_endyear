using UnityEngine;
using System.Collections.Generic;

public class SpriteBillboard : MonoBehaviour
{
    public CameraDirection cameraDirection;
    public RotationSprite rotationSprite;

    private SpriteRenderer _sprite;
    private Transform _billboard;

    private Transform _t;

    [SerializeField] private Transform _vehicle;


    void Start()
    {
        _t = transform;
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _billboard = _sprite.transform;

        if ( _vehicle == null)
        {
            _vehicle = transform;
        }
    }

    void Update()
    {

        Vector3 targetPoint = new Vector3(cameraDirection.transform.position.x, _t.position.y, cameraDirection.transform.position.z) - transform.position;
        _billboard.rotation = Quaternion.LookRotation(-targetPoint, Vector3.up);

        // Change this with your own sprite animation stuff
        Vector3 Angle = cameraDirection.Angle;
        var kartRotation = _vehicle.localEulerAngles.y;
        var sumRotation = Angle.x - kartRotation;

        if (sumRotation > 360)
        {
            sumRotation -= 360;
        }
        else if (sumRotation < 0)
        {
            sumRotation += 360;
        }

        _sprite.sprite = rotationSprite.GetSpriteFromRotation(sumRotation);
        // Debug.Log(Angle.x + " & " + kartRotation + " = " + sumRotation);
    }
}