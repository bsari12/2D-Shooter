using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;

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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.transform.GetChild(0).gameObject.SetActive(false);
    }

}