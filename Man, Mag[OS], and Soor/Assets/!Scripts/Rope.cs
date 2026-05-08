using UnityEngine;

public class Rope : MonoBehaviour
{
    public enum Player { PlayerA, PlayerB, PlayerC }

    public Rigidbody rgbdPlayerA;       
    public Rigidbody rgbdPlayerB;
    public Rigidbody rgbdPlayerC;

    public Player playerA;
    public Player playerB; 
    public Player playerC;

    [SerializeField] private float minLenght = 2f;
    [SerializeField] private float maxLenght = 10f;
    [SerializeField] private float pullSpeed = 5f;

    private SpringJoint joint;
    private LineRenderer line;

    private float targetLength;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        //joint = GetComponent<SpringJoint>();
        joint = rgbdPlayerA.gameObject.AddComponent<SpringJoint>();
        joint.connectedBody = rgbdPlayerB;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.spring = 40f;
        joint.damper = 5f;

        float startDist = Vector3.Distance(rgbdPlayerA.position, rgbdPlayerB.position);
        joint.maxDistance = startDist;
        targetLength = startDist;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(playerA);
        HandleInput(playerB);
        HandleInput(playerC);


        joint.maxDistance = Mathf.Lerp(joint.maxDistance, targetLength, Time.deltaTime * 8f);

        DrawRope();
    }

    void HandleInput(Player player)
    {
        KeyCode Shorten = KeyCode.None;
        KeyCode Lengthen = KeyCode.None;

        switch (player)
        {
            case player.PlayerA:
                Shorten = KeyCode.A;
                Lengthen = KeyCode.D;
                break;
            case player.PlayerB:
                Shorten = KeyCode.J;
                Lengthen = KeyCode.L;
                break;
            case player.PlayerC:
                Shorten = KeyCode.LeftArrow;
                Lengthen = KeyCode.RightArrow;
                break;

        }

        if (HandleInput.GetKey(shorten))
        {
            targetLength -= pullSpeed * Time.deltaTime;
        }
        if (HandleInput.GetKey(Lengthen))
        {
            targetLength += pullSpeed * Time.deltaTime;
        }

        targetLength = Mathf.Clamp(targetLength, minLenght, maxLenght);
    }

  /*  public void Shorten()
    {
        targetLength = Mathf.Max(minLenght, targetLength - pullSpeed * Time.deltaTime);
    }

    public void Lengthen()
    {
        targetLength = Mathf.Min(maxLenght, targetLength + pullSpeed * Time.deltaTime);
    }
*/
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
