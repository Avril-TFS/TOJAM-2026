using UnityEngine;

public class DualPhaseLerpScaler : MonoBehaviour
{
    [Header("Lifetime")]
    [Min(0f)]
    public float lifetime = 6f;

    [Header("Phase 1 – Uniform Scale (X, Y, Z)")]
    public Vector3 phase1TargetScale = new Vector3(3f, 3f, 3f);
    [Min(0.001f)]
    public float phase1Duration = 2f;

    [Header("Phase 2 – Scale X and Z only (Y is locked)")]
    public Vector2 phase2TargetScaleXZ = new Vector2(5f, 5f);
    [Min(0.001f)]
    public float phase2Duration = 2f;

    private Vector3 _phase1StartScale;
    private Vector3 _phase2StartScale;
    private float _lockedY;          // Y scale captured at end of Phase 1
    private float _phaseElapsed;
    private bool _inPhase2;

    private void Awake()
    {
        _phase1StartScale = transform.localScale;
    }

    private void Start()
    {
        if (lifetime > 0f)
            Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!_inPhase2)
            RunPhase1();
        else
            RunPhase2();
    }

    private void RunPhase1()
    {
        _phaseElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_phaseElapsed / phase1Duration);

        transform.localScale = Vector3.Lerp(_phase1StartScale, phase1TargetScale, t);

        if (t >= 1f)
        {
            // Lock Y explicitly from the authoritative target, not from
            // whatever floating-point result Lerp happened to produce.
            _lockedY = phase1TargetScale.y;
            _phase2StartScale = transform.localScale;
            _phaseElapsed = 0f;
            _inPhase2 = true;
        }
    }

    private void RunPhase2()
    {
        _phaseElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_phaseElapsed / phase2Duration);

        // Build target with Y explicitly held at the locked value.
        Vector3 phase2Target = new Vector3(
            phase2TargetScaleXZ.x,
            _lockedY,
            phase2TargetScaleXZ.y
        );

        Vector3 next = Vector3.Lerp(_phase2StartScale, phase2Target, t);

        // Belt-and-suspenders: overwrite Y so no floating-point drift
        // can ever move it, even if _phase2StartScale.y != _lockedY.
        next.y = _lockedY;

        transform.localScale = next;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        float totalPhaseDuration = phase1Duration + phase2Duration;
        if (totalPhaseDuration > lifetime)
        {
            Debug.LogWarning(
                $"[DualPhaseLerpScaler] Combined phase duration ({totalPhaseDuration}s) exceeds " +
                $"Lifetime ({lifetime}s) on '{name}'. Phase 2 may not complete before destruction.",
                this
            );
        }
    }
#endif
}