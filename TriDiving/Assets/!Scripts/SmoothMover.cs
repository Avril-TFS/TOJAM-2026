using UnityEngine;

public class SmoothMovers : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float targetY = 5f;

    [Header("Movement")]
    [SerializeField] private float moveTime = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float timer;
    private bool isMoving;

    private void Start()
    {
        startPosition = transform.position;

        targetPosition = new Vector3(
            transform.position.x,
            targetY,
            transform.position.z
        );

        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        timer += Time.deltaTime;

        float t = timer / moveTime;

        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }
}