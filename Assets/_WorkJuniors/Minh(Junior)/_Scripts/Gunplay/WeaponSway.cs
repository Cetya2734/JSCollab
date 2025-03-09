// using UnityEngine;
//
// public class WeaponSway : MonoBehaviour
// {
//     [Header("Position")]
//     public float amount = 0.02f,
//         maxAmount = 0.06f,
//         smoothAmount = 6f;
//
//     [Header("Rotation")]
//     public float rotationAmount = 4f,
//         maxRotationAmount = 5f,
//         smoothRotation = 12f;
//
//     [Space]
//     public bool rotationX = true, rotationY = true, rotationZ = true;
//
//     private Vector3 initialPosition;
//     private Quaternion initialRotation;
//
//     private float InputX, InputY;
//
//     private void Start()
//     {
//         initialPosition = transform.localPosition;
//         initialRotation = transform.localRotation;
//     }
//
//     private void Update()
//     {
//         CalculateSway();
//         MoveSway();
//         TiltSway();
//     }
//
//     private void CalculateSway()
//     {
//         InputX = Input.GetAxis("Mouse X");
//         InputY = Input.GetAxis("Mouse Y");
//     }
//
//     private void MoveSway()
//     {
//         float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
//         float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);
//
//         Vector3 finalPosition = new Vector3(moveX, moveY, 0);
//
//         transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
//     }
//
//     private void TiltSway()
//     {
//         float tiltY = Mathf.Clamp(InputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
//         float tiltX = Mathf.Clamp(InputY * rotationAmount, -maxRotationAmount, maxRotationAmount);
//
//         Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? -tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));
//             
//         transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, smoothRotation * Time.deltaTime);
//     }
// }

using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] public Transform weaponTransform;

    [Header("Sway Properties")] [SerializeField]
    private float swayAmount = 0.01f;

    [SerializeField] public float maxSwayAmount = 0.5f;
    [SerializeField] public float swaySmooth = 50f;
    [SerializeField] public AnimationCurve swayCurve;

    [Range(0f, 1f)] [SerializeField] public float swaySmoothCounteraction = 1f;

    [Header("Rotation")] [SerializeField] public float rotationSwayMultiplier = -1f;

    [Header("Position")] [SerializeField] public float positionSwayMultiplier = 9f;


    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector2 sway;
    Quaternion lastRot;

    private void Reset()
    {
        Keyframe[] ks = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0) };
        swayCurve = new AnimationCurve(ks);
    }

    private void Start()
    {
        if (!weaponTransform)
            weaponTransform = transform;
        lastRot = transform.localRotation;
        initialPosition = weaponTransform.localPosition;
        initialRotation = weaponTransform.localRotation;
    }

    private void Update()
    {
        var angularVelocity = Quaternion.Inverse(lastRot) * transform.rotation;

        float mouseX = FixAngle(angularVelocity.eulerAngles.y) * swayAmount;
        float mouseY = -FixAngle(angularVelocity.eulerAngles.x) * swayAmount;

        lastRot = transform.rotation;

        sway = Vector2.MoveTowards(sway, Vector2.zero,
            swayCurve.Evaluate(Time.deltaTime * swaySmoothCounteraction * sway.magnitude * swaySmooth));
        sway = Vector2.ClampMagnitude(new Vector2(mouseX, mouseY) + sway, maxSwayAmount);

        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition,
            new Vector3(sway.x, sway.y, 0) * positionSwayMultiplier * Mathf.Deg2Rad + initialPosition,
            swayCurve.Evaluate(Time.deltaTime * swaySmooth));
        weaponTransform.localRotation = Quaternion.Slerp(weaponTransform.localRotation,
            initialRotation *
            Quaternion.Euler(Mathf.Rad2Deg * rotationSwayMultiplier * new Vector3(-sway.y, sway.x, 0)),
            swayCurve.Evaluate(Time.deltaTime * swaySmooth));
    }

    private float FixAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 180f;
    }
}