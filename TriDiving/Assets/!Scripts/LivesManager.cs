using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LivesManager : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int lives;

    [SerializeField] private Image Life1;
    [SerializeField] private Image Life2;
    [SerializeField] private Image Life3;

    [Header("Camera Shake")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.15f;

    private Vector3 originalCamPos;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        originalCamPos = cameraTransform.localPosition;
    }

    public void LoseLife()
    {
        lives--;

        // Trigger camera shake
        if(lives!=0){
            StartCoroutine(CameraShake());
        }

        if (lives == 2)
        {
            Life3.gameObject.SetActive(false);
        }
        else if (lives == 1)
        {
            Life2.gameObject.SetActive(false);
        }
        else if (lives == 0)
        {
            Life1.gameObject.SetActive(false);
        }
    }

    private IEnumerator CameraShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = originalCamPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalCamPos;
    }

    public int GetLives()
    {
        return lives;
    }
}