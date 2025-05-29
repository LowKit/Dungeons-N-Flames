using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu;
    [SerializeField] private UiUpdate uiUpdate;

    void Start()
    {
        uiUpdate = FindFirstObjectByType<UiUpdate>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pausa o tempo
            pauseMenu.SetActive(true);
            uiUpdate?.OcultarInteracao();
        }
        else
        {
            Time.timeScale = 1f; // Retoma o tempo
            pauseMenu.SetActive(false);
        }

        Debug.Log("Estado do jogo: " + (isPaused ? "PAUSADO" : "A DECORRER"));
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("O jogo foi encerrado.");
    }
}

