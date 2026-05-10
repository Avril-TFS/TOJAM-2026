using UnityEngine;

public class PhaseTimer : MonoBehaviour
{
    [SerializeField] private GameObject terrainObject;
    [SerializeField] private float delay = 80f;

    private void Start()
    {
        if (terrainObject != null)
            StartCoroutine(EnableTerrainAfterDelay());
    }

    private IEnumerator EnableTerrainAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        terrainObject.SetActive(true);
    }
}
