using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public void LoadActiveScene()
    {
        var getActiveIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(getActiveIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadNextLevel()
    {

        StartCoroutine(nameof(LoadLevel));
    }
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
