using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
