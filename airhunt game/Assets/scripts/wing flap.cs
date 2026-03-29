using UnityEngine;

public class WingFlap : MonoBehaviour
{
    public float flapSpeed = 8f;
    public float flapAngle = 30f;

    private float startRotation;

    void Start()
    {
        startRotation = transform.localEulerAngles.z;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * flapSpeed) * flapAngle;
        transform.localRotation = Quaternion.Euler(0, 0, startRotation + angle);
    }
}