using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SceneTransition transition;

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("target").Length == 0)
        {
            transition.StartTransition();
        }
    }
}