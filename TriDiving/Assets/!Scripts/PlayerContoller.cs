using UnityEngine;
using UnityEngine.UI; 
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
    [SerializeField] LivesManager livesManager;
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
        livesManager.LoseLife();
        // livesText.text = "Lives: " + livesManager.GetLives();

        Play_Player_Impact_Object_Body.Post(gameObject);

        if(livesManager.GetLives() == 0){
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
