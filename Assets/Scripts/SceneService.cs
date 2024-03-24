using UnityEngine.SceneManagement;

public class SceneService 
{
    public static void ReloadScene()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}