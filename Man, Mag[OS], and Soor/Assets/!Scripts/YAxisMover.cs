using UnityEngine;


public class YAxisMover : MonoBehaviour
{
   
    public float speed = 3f;

    public float lifetime = 5f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void Update()
    {
        
        float deltaY = speed * Time.deltaTime;

        transform.Translate(0f, deltaY, 0f);
    }
}