using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public Transform[] cannons;
    public GameObject birdPrefab;

    public void FireBird()
    {
        int index = Random.Range(0, cannons.Length);
        Transform cannon = cannons[index];

        GameObject bird = Instantiate(birdPrefab, cannon.position, Quaternion.identity);

        BirdFlight flight = bird.GetComponent<BirdFlight>();

        // If left → go right, if right → go left
        float dir = cannon.position.x < 0 ? 1f : -1f;

        flight.SetDirection(dir);
    }
}