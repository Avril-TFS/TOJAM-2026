using UnityEngine;

public class RopeContoller : MonoBehaviour
{
    public enum Player { PlayerA, PlayerB, PlayerC }
    public Player player;

    public Rope leftRope;
    public Rope rightRope;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.A))
        {
            leftRope.Shorten();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            leftRope.Lengthen();
        }
        if (Input.GetKey(KeyCode.J))
        {
            rightRope.Shorten();
        }
        else if (Input.GetKey(KeyCode.L))
        {
            rightRope.Lengthen();
        }*/

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
    }
}
