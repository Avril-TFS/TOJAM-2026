using UnityEngine;
using UnityEngine.InputSystem;

public class RopeController : MonoBehaviour
{
    public Transform playerA;
    public Transform playerB;
    public Transform playerC;
    public float pullSpeed = 5f;
    public float pushSpeed = 5f;
    public float relaxSpeed = 3f;
    public float maxDistance = 8f;

    float restAB, restAC, restBC;

    Gamepad padA, padB, padC;

    void Start()
    {
        restAB = Vector3.Distance(playerA.position, playerB.position);
        restAC = Vector3.Distance(playerA.position, playerC.position);
        restBC = Vector3.Distance(playerB.position, playerC.position);

        if (Gamepad.all.Count > 0) padA = Gamepad.all[0];
        if (Gamepad.all.Count > 1) padB = Gamepad.all[1];
        if (Gamepad.all.Count > 2) padC = Gamepad.all[2];
    }

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad &&
            (change == InputDeviceChange.Added || change == InputDeviceChange.Removed))
        {
            padA = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
            padB = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;
            padC = Gamepad.all.Count > 2 ? Gamepad.all[2] : null;
        }
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
        // ----------------------------------------------------------
        // Player A
        // Keyboard: A/D = pull, W/S = push
        // Gamepad:  left trigger = pull B, right trigger = pull C
        //           left shoulder = push B, right shoulder = push C
        // ----------------------------------------------------------
        if (Input.GetKey(KeyCode.A) || (padA != null && padA.leftTrigger.isPressed))
            Pull(playerA, playerB);

        if (Input.GetKey(KeyCode.D) || (padA != null && padA.rightTrigger.isPressed))
            Pull(playerA, playerC);

        if (Input.GetKey(KeyCode.W) || (padA != null && padA.leftShoulder.isPressed))
            Push(playerA, playerB);

        if (Input.GetKey(KeyCode.S) || (padA != null && padA.rightShoulder.isPressed))
            Push(playerA, playerC);

        // ----------------------------------------------------------
        // Player B
        // Keyboard: J/L = pull, I/K = push
        // Gamepad:  left trigger = pull A, right trigger = pull C
        //           left shoulder = push A, right shoulder = push C
        // ----------------------------------------------------------
        if (Input.GetKey(KeyCode.J) || (padB != null && padB.leftTrigger.isPressed))
            Pull(playerB, playerA);

        if (Input.GetKey(KeyCode.L) || (padB != null && padB.rightTrigger.isPressed))
            Pull(playerB, playerC);

        if (Input.GetKey(KeyCode.I) || (padB != null && padB.leftShoulder.isPressed))
            Push(playerB, playerA);

        if (Input.GetKey(KeyCode.K) || (padB != null && padB.rightShoulder.isPressed))
            Push(playerB, playerC);

        // ----------------------------------------------------------
        // Player C
        // Keyboard: left/right arrow = pull, up/down arrow = push
        // Gamepad:  left trigger = pull A, right trigger = pull B
        //           left shoulder = push A, right shoulder = push B
        // ----------------------------------------------------------
        if (Input.GetKey(KeyCode.LeftArrow) || (padC != null && padC.leftTrigger.isPressed))
            Pull(playerC, playerA);

        if (Input.GetKey(KeyCode.RightArrow) || (padC != null && padC.rightTrigger.isPressed))
            Pull(playerC, playerB);

        if (Input.GetKey(KeyCode.UpArrow) || (padC != null && padC.leftShoulder.isPressed))
            Push(playerC, playerA);

        if (Input.GetKey(KeyCode.DownArrow) || (padC != null && padC.rightShoulder.isPressed))
            Push(playerC, playerB);
    }

    void Pull(Transform puller, Transform target)
    {
        float dist = Vector3.Distance(puller.position, target.position);
        if (dist < maxDistance)
            puller.position = Vector3.MoveTowards(puller.position, target.position, pullSpeed * Time.deltaTime);
    }

    void Push(Transform pusher, Transform target)
    {
        pusher.position = Vector3.MoveTowards(pusher.position, target.position, -pushSpeed * Time.deltaTime);
    }

    void Relax(Transform a, Transform b, float restDistance)
    {
        float dist = Vector3.Distance(a.position, b.position);
        if (dist > restDistance)
        {
            Vector3 midpoint = (a.position + b.position) / 2f;
            a.position = Vector3.MoveTowards(a.position, midpoint, relaxSpeed * Time.deltaTime);
            b.position = Vector3.MoveTowards(b.position, midpoint, relaxSpeed * Time.deltaTime);
        }
    }
}