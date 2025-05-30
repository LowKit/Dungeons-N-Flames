using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("backup");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("O jogo foi encerrado.");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
