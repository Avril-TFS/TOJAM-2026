using UnityEngine;
using TMPro;

public class PlayerContoller : MonoBehaviour
{
    public enum Player { PlayerA, PlayerB, PlayerC }

    [Header("Player")]
    public Player player;

    public Rope leftRope;
    public Rope rightRope;
    public AK.Wwise.Event Play_Player_Impact_Object_Body;

    [Header("Lives")]
    [SerializeField] private int totalLives = 3;
    private int currentLives = 3;
    [SerializeField] public TMP_Text livesText;
    [SerializeField] public TMP_Text gameOverText;
    [SerializeField] public TMP_Text gameWinText;
    public PauseMenu pauseMenu;

    void Awake()
    {
        gameOverText.gameObject.SetActive(false);
        gameWinText.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLives = totalLives;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            TakeDamage();
        }
        if (other.CompareTag("winBox"))
        {
            Time.timeScale = 0f;

            if (gameWinText != null)
            {
                gameWinText.gameObject.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void TakeDamage()
    {
        currentLives--;
        livesText.text = "Lives: " + currentLives;

        Play_Player_Impact_Object_Body.Post(gameObject);

        if (currentLives <= 0)
        {
            pauseMenu.GameOver();
        }
    }
    //public void GameOver()
    //{
      //  pauseMenu.GameOver();

        //gameOverText.gameObject.SetActive(true);
        //Time.timeScale = 0f; 
 //   }
}
