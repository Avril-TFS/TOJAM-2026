using UnityEngine;

public class Rope : MonoBehaviour
{
    public enum Player { PlayerA, PlayerB, PlayerC }

    public Rigidbody rgbdPlayerA;       
    public Rigidbody rgbdPlayerB;
    //public Rigidbody rgbdPlayerC;

    /*public Player playerA;
    public Player playerB; 
    public Player playerC;
*/
   // [SerializeField] private float minLenght = 2f;
   // [SerializeField] private float maxLenght = 10f;
//    [SerializeField] private float pullSpeed = 5f;

    //private SpringJoint joint;
    private LineRenderer line;

    private float targetLength;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        DrawRope(); // probably just move this method into the update, and delete like all of these shitty commented out code

    
    }

    void DrawRope()
    {
        Vector3 midPoint = (rgbdPlayerA.position + rgbdPlayerB.position) / 2f;
        midPoint.y -= 0.5f;

        line.positionCount = 3;
        line.SetPosition(0, rgbdPlayerA.position);
        line.SetPosition(1, midPoint);
        line.SetPosition(2, rgbdPlayerB.position);
    }
}
