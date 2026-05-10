using UnityEngine;

public class RopeController : MonoBehaviour
{
    public Transform playerA;
    public Transform playerB;
    public Transform playerC;

    public float pullSpeed = 5f;
    public float relaxSpeed = 3f;
    public float maxDistance = 8f;


    float restAB;
    float restAC;
    float restBC;

    void Start()
        {
            restAB = Vector3.Distance(playerA.position, playerB.position);
            restAC = Vector3.Distance(playerA.position, playerC.position);
            restBC = Vector3.Distance(playerB.position, playerC.position);
        }
    void Update()
    {
        HandleInput();
        Relax(playerA, playerB, restAB);
        Relax(playerA, playerC, restAC);
        Relax(playerB, playerC, restBC);
    }

    void HandleInput()
    {
        // Player A pulls
        if (Input.GetKey(KeyCode.A)) Pull(playerA, playerB);
        if (Input.GetKey(KeyCode.D)) Pull(playerA, playerC);

        // Player B pulls
        if (Input.GetKey(KeyCode.V)) Pull(playerB, playerA);
        if (Input.GetKey(KeyCode.B)) Pull(playerB, playerC);

        // Player C pulls
        if (Input.GetKey(KeyCode.J)) Pull(playerC, playerA);
        if (Input.GetKey(KeyCode.L)) Pull(playerC, playerB);
    }

    void Pull(Transform puller, Transform target)
    {
        Vector3 dir = (puller.position - target.position).normalized;
        target.position += dir * pullSpeed * Time.deltaTime;
    }

    void Relax(Transform p1, Transform p2, float restLength)
    {
        float dist = Vector3.Distance(p1.position, p2.position);

        if (Mathf.Abs(dist - restLength) < 1.5f) {
            Vector3 dir = (p2.position - p1.position).normalized;

            float move = relaxSpeed * Time.deltaTime;

            if(dist <= restLength)
            {
                p1.position += dir * move;
                p2.position -= dir * move;
            }
             if(dist > restLength)
            {
                p1.position -= dir * move;
                p2.position += dir * move;
            }
        }
        
    }
}