using UnityEngine.SceneManagement;

public class SceneService 
{
    public void ReloadScene()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}