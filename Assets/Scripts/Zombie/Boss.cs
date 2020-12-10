using UnityEngine;

public class Boss : MonoBehaviour
{
    private Zombie zombie;
    private SceneLoader sceneLoader;

    private void Start()
    {
        zombie = GetComponent<Zombie>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }
    private void Update()
    {
        if( zombie.SetAlive() == false)
        {
            sceneLoader.LoadNextLevel();
        }
    }
}
