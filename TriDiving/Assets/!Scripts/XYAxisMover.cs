using UnityEngine;

public class XYZAxisMover : MonoBehaviour
{
    public enum HorizontalAxis { X, Z }

    public HorizontalAxis horizontalAxis = HorizontalAxis.X;

    public float horizontalSpeed = 3f;
    public float verticalSpeed = 3f;

    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float deltaH = horizontalSpeed * Time.deltaTime;
        float deltaY = verticalSpeed * Time.deltaTime;

        if (horizontalAxis == HorizontalAxis.X)
            transform.Translate(deltaH, deltaY, 0f, Space.World);
        else
            transform.Translate(0f, deltaY, deltaH, Space.World);
    }
}