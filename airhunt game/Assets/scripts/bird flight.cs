using UnityEngine;

public class BirdFlight : MonoBehaviour
{
    public float speed = 4f;
    public float bobHeight = 0.5f;
    public float bobSpeed = 2f;

    private float direction;
    private float startY;

    public void SetDirection(float dir)
    {
        direction = dir;
        startY = transform.position.y;

        InvokeRepeating(nameof(RandomTurn), 1.5f, 2f);
    }

    void Update()
    {
        // Move
        float x = transform.position.x + direction * speed * Time.deltaTime;

        // Bob
        float y = startY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = new Vector3(x, y, transform.position.z);

        // Face direction
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void RandomTurn()
    {
        if (Random.value > 0.7f)
        {
            direction *= -1;
        }
    }
}