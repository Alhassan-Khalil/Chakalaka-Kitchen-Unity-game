

using UnityEngine.SceneManagement;

static class Loader 
{

    public enum Scene
    {
        _MainMenuScene,
        GameScene,
        LoadingScreenScene,
        Level1,

    }
    private static Scene targetScene;


    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Loader.Scene.LoadingScreenScene.ToString());


    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());

    }
}
