using UnityEngine;

public class Rope : MonoBehaviour
{
    public Rigidbody playerA;
    public Rigidbody playerB;

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
        joint = playerA.gameObject.AddComponent<SpringJoint>();
        joint.connectedBody = playerB;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.spring = 40f;
        joint.damper = 5f;

        float startDist = Vector3.Distance(playerA.position, playerB.position);
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
        joint.maxDistance = Mathf.Lerp(joint.maxDistance, targetLength, Time.deltaTime * 8f);

        DrawRope();
    }

    public void Shorten()
    {
        targetLength = Mathf.Max(minLenght, targetLength - pullSpeed * Time.deltaTime);
    }

    public void Lengthen()
    {
        targetLength = Mathf.Min(maxLenght, targetLength + pullSpeed * Time.deltaTime);
    }

    void DrawRope()
    {
        Vector3 midPoint = (playerA.position + playerB.position) / 2f;
        midPoint.y -= 0.5f;

        line.positionCount = 3;
        line.SetPosition(0, playerA.position);
        line.SetPosition(1, midPoint);
        line.SetPosition(2, playerB.position);
    }
}
