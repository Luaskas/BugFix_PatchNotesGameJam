using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum GameScene
{
    MainMenu,
    Level1,
    Level2,
    Level3,
    Credits
}
public static class SceneLoader
{
    public static GameScene currentScene;
    public static void LoadScene(GameScene scene)
    {
        SceneManager.LoadScene(scene.ToString());
        currentScene = scene;
    } 
}
