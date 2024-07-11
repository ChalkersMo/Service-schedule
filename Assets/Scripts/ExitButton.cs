using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ExitFromPanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ExitFromRoom()
    {
        SceneLoader.LoadScene(1);
    }
}

[System.Serializable]
public class SceneLoader
{
    public static void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}
