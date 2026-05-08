using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject startButton, quitButton, creditsButton, backButton;
    [SerializeField] private GameObject titleText, creditsText;

    private void Start()
    {
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);

        titleText.gameObject.SetActive(true);
        creditsText.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void GameQuit()
    {
        Application.Quit();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void Credits()
    {
        camera.transform.rotation = Quaternion.Euler(0, -180, 0);
        startButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        creditsText.gameObject.SetActive(true);
        titleText.gameObject.SetActive(false);
    }
    public void MenuBack()
    {
        camera.transform.rotation = Quaternion.Euler(115, 0, 0);
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(true);
    }
}