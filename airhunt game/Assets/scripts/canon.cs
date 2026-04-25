using UnityEngine;
using TMPro;

public class CannonManager : MonoBehaviour
{
    [Header("Cannons & Bird")]
    public Transform[] cannons;
    public GameObject birdPrefab;

    [Header("Survival Settings")]
    public float startTime = 30f;
    public float timePerKill = 3f;
    public float spawnInterval = 2f;

    [Header("Spawn Height Range")]
    public float minSpawnHeight = -3f;
    public float maxSpawnHeight = 3f;

    [Header("UI")]
public TextMeshProUGUI timerText;
public TextMeshProUGUI scoreText;
public TextMeshProUGUI finalScoreText; // add this
public GameObject gameOverScreen;

    float timeLeft;
    int score = 0;
    bool gameActive = false;
    float spawnTimer = 0f;

    void Start()
    {
        timeLeft = startTime;
        gameActive = true;
        gameOverScreen.SetActive(false);
    }

    void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            GameOver();
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            FireBird();
            FireBird();
        }
    }

    public void FireBird()
    {
        if (cannons.Length == 0 || birdPrefab == null) return;

        int index = Random.Range(0, cannons.Length);
        Transform cannon = cannons[index];

        // Random height offset added here
        float randomY = Random.Range(minSpawnHeight, maxSpawnHeight);
        Vector3 spawnPos = new Vector3(cannon.position.x, cannon.position.y + randomY, cannon.position.z);

        GameObject bird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
        BirdFlight flight = bird.GetComponent<BirdFlight>();

        if (flight != null)
        {
            float dir = cannon.position.x < 0 ? 1f : -1f;
            flight.SetDirection(dir);
        }
        else
        {
            Debug.LogError("BirdFlight script missing on prefab!");
        }
    }

    public void OnBirdKilled()
    {
        score++;
        timeLeft += timePerKill;
        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
    gameActive = false;
    Time.timeScale = 0f;
    gameOverScreen.SetActive(true);
    finalScoreText.text = "Final Score: " + score;
    }
}