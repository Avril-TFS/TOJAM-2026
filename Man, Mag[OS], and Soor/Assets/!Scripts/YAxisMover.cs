using UnityEngine;


public class YAxisMover : MonoBehaviour
{
    [Header("Movement Settings")]

    public float speed = 3f;

    void Update()
    {
        
        float deltaY = speed * Time.deltaTime;

        transform.Translate(0f, deltaY, 0f);
    }
}