using UnityEngine;

public class DualPhaseScaler : MonoBehaviour
{
    [Header("Lifetime")]
    [Min(0f)]
    public float lifetime = 5f;

    [Header("Phase 1 – Uniform Scaling (X, Y, Z)")]
    [Min(0f)]
    public float phase1Duration = 2f;
    public float phase1ScaleSpeed = 1f;

    [Header("Phase 2 – Planar Scaling (X, Z only)")]
    public float phase2ScaleSpeed = 1.5f;

    [Header("Initial Scale (optional override)")]
    public Vector3 initialScale = Vector3.one;

    private float _elapsedTime;
    private bool _inPhase2;

    private void Awake()
    {
        transform.localScale = initialScale;
    }

    private void Start()
    {
        if (lifetime > 0f)
            Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (!_inPhase2 && _elapsedTime >= phase1Duration)
            EnterPhase2();

        if (_inPhase2)
            ApplyPhase2Scaling();
        else
            ApplyPhase1Scaling();
    }

    private void ApplyPhase1Scaling()
    {
        float delta = phase1ScaleSpeed * Time.deltaTime;
        transform.localScale += new Vector3(delta, delta, delta);
    }

    private void EnterPhase2()
    {
        _inPhase2 = true;
    }

    private void ApplyPhase2Scaling()
    {
        float delta = phase2ScaleSpeed * Time.deltaTime;
        Vector3 currentScale = transform.localScale;

        transform.localScale = new Vector3(
            currentScale.x + delta,
            currentScale.y,
            currentScale.z + delta
        );
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (phase1Duration > lifetime)
        {
            Debug.LogWarning(
                $"[DualPhaseScaler] 'Phase 1 Duration' ({phase1Duration}s) exceeds " +
                $"'Lifetime' ({lifetime}s) on '{name}'. Phase 2 will never be reached.",
                this
            );
        }
    }
#endif
}