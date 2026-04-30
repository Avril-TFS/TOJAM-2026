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
    void Update()
    {
        KeyCode shorten;
        KeyCode lengthen;

        switch (player)
        {
            case Player.PlayerA:
                shorten = KeyCode.A;
                lengthen = KeyCode.D;
                break;
            case Player.PlayerB:
                shorten = KeyCode.J;
                lengthen = KeyCode.L;
                break;
            case Player.PlayerC:
                shorten = KeyCode.LeftArrow;
                lengthen = KeyCode.RightArrow;
                break;
            default:
                shorten = KeyCode.None;
                lengthen = KeyCode.None;
                break;
        }

        if (Input.GetKey(shorten))
        {
            leftRope.Shorten();
            rightRope.Shorten();
        }
        else if (Input.GetKey(lengthen))
        {
            leftRope.Lengthen();
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
            GameOver();
        }
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
