using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    [SerializeField] int lives;
    [SerializeField] private Image Life1;
    [SerializeField] private Image Life2;
    [SerializeField] private Image Life3;
    
    public void LoseLife(){
        lives--;
        if (lives == 2){
            Life3.gameObject.SetActive(false);
        }
        else if (lives == 1){
            Life2.gameObject.SetActive(false);
        }
        else if (lives == 0)        {
            Life1.gameObject.SetActive(false);
        }
    }

    public int GetLives(){
        return lives;
    }
}
