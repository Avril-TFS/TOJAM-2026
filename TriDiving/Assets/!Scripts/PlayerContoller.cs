using UnityEngine;
using TMPro;

public class PlayerContoller : MonoBehaviour
{
    public enum Player { PlayerA, PlayerB, PlayerC }

    [Header("Player")]
    public Player player;

    public Rope leftRope;
    public Rope rightRope;

    [Header("Lives")]
    [SerializeField] private int totalLives = 3;
    private int currentLives = 3;
    [SerializeField] public TMP_Text livesText;
    [SerializeField] public TMP_Text gameOverText;
    public PauseMenu pauseMenu;

    void Awake()
    {
        gameOverText.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLives = totalLives;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        KeyCode shortenL;
        KeyCode lengthenL;
        KeyCode shortenR;
        KeyCode lengthenR;

        switch (player)
        {       //Place holders until we set up controller support, but I have no idea how to do that
            case Player.PlayerA:
                shortenL = KeyCode.A;
                lengthenL = KeyCode.Q;
                shortenR = KeyCode.D;
                lengthenR = KeyCode.E;
                break;
            case Player.PlayerB:
                shortenL = KeyCode.J;
                lengthenL = KeyCode.U;
                shortenR = KeyCode.L;
                lengthenR = KeyCode.O;
                break;
            case Player.PlayerC:
                shortenL = KeyCode.LeftArrow;
                lengthenL = KeyCode.UpArrow;
                shortenR = KeyCode.DownArrow;
                lengthenR = KeyCode.RightArrow;
                break;
            default:
                shortenL = KeyCode.None;
                lengthenL = KeyCode.None;
                shortenR = KeyCode.None;
                lengthenR = KeyCode.None;
                break;
        }

        if (Input.GetKey(shortenL))
        {
            leftRope.Shorten();
            
        }
            else if (Input.GetKey(lengthenL))
            {
                leftRope.Lengthen();
                
            }
        if (Input.GetKey(shortenR))
        {
            rightRope.Shorten();
            }
        else if (Input.GetKey(lengthenR))
        {
            
            rightRope.Lengthen();
        }
        //Testing damage with space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            TakeDamage();
        }
    }
    public void TakeDamage()
    {
        currentLives--;
        livesText.text = "Lives: " + currentLives;

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
