using UnityEngine;

public class BirdFlight : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float bobHeight = 0.5f;
    public float bobSpeed = 2f;

    [Header("Direction Change")]
    public float directionChangeChance = 0.3f;
    public float changeInterval = 2f;

    private float direction = 1f;
    private float startY;

    public void SetDirection(float dir)
    {
        direction = dir;
        startY = transform.position.y;

        // Start random direction changes
        InvokeRepeating(nameof(RandomTurn), changeInterval, changeInterval);
    }

    void Update()
    {
        // Move horizontally
        float x = transform.position.x + direction * speed * Time.deltaTime;

        // Vertical bobbing
        float y = startY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = new Vector3(x, y, transform.position.z);

        // Face direction using ROTATION (no flatten bug)
        if (direction > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void RandomTurn()
    {
        if (Random.value < directionChangeChance)
        {
            direction *= -1;
        }
    }

    void OnDestroy()
    {
        CancelInvoke(); // clean up (you’ll forget this otherwise)
    }
}