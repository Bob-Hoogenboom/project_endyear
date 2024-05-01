using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}