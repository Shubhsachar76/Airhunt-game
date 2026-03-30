using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public Transform[] cannons;
    public GameObject birdPrefab;

    void Update()
    {
        // Press K to fire
        if (Input.GetKeyDown(KeyCode.K))
        {
            FireBird();
        }
    }

    public void FireBird()
    {
        // Safety check
        if (cannons.Length == 0 || birdPrefab == null)
        {
            Debug.LogError("Cannons or Bird Prefab not assigned!");
            return;
        }

        // Pick random cannon
        int index = Random.Range(0, cannons.Length);
        Transform cannon = cannons[index];

        // Spawn bird
        GameObject bird = Instantiate(birdPrefab, cannon.position, Quaternion.identity);

        // Get BirdFlight script
        BirdFlight flight = bird.GetComponent<BirdFlight>();

        if (flight != null)
        {
            // If left → go right, if right → go left
            float dir = cannon.position.x < 0 ? 1f : -1f;
            flight.SetDirection(dir);
        }
        else
        {
            Debug.LogError("BirdFlight script missing on prefab!");
        }

        Debug.Log("Bird fired from cannon: " + cannon.name);
    }
}