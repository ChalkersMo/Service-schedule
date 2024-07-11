using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    [SerializeField] private int SceneId;
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneId);
    }
}
