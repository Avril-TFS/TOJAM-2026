using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton, quitButton, restartButton;
    [SerializeField] private GameObject pauseText, gameOverText;
    private bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        isGameOver = false;

        pauseText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        //mainMenuButton.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            if(Time.timeScale == 0 || isGameOver == true)
            {
                return;
            }
            else{
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseText.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
      // mainMenuButton.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseText.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
       // mainMenuButton.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        isGameOver= true;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        //mainMenuButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {

        SceneManager.LoadScene("MainScene");
    }
   /* public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }*/
    public void QuitGame()
    {
        Application.Quit();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
