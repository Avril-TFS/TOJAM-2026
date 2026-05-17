using System;
using System.Collections;
using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float duration = 2f;
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private bool moveOnStart = false;

    [Header("Lifetime Settings")]
    [SerializeField] private bool useLifetime = true;
    [SerializeField] private float lifetime = 3f;

    private Coroutine _activeMovement;

    private void Start()
    {
        if (useLifetime)
            Destroy(gameObject, lifetime);

        if (moveOnStart)
            MoveToTarget();
    }

    public void MoveToTarget()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("[SmoothMover] No target object assigned.");
            return;
        }

        MoveTo(targetObject, duration);
    }

    public void MoveTo(GameObject target, float travelDuration, Action onComplete = null)
    {
        if (target == null)
        {
            Debug.LogWarning("[SmoothMover] Target object is null.");
            return;
        }

        if (travelDuration <= 0f)
        {
            Debug.LogWarning("[SmoothMover] Duration must be > 0. Snapping to destination.");
            transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
            onComplete?.Invoke();
            return;
        }

        StopMovement();
        _activeMovement = StartCoroutine(MoveRoutine(target, travelDuration, onComplete));
    }

    public void MoveTo(Vector3 destination, float travelDuration, Action onComplete = null)
    {
        if (travelDuration <= 0f)
        {
            Debug.LogWarning("[SmoothMover] Duration must be > 0. Snapping to destination.");
            transform.position = new Vector3(transform.position.x, destination.y, transform.position.z);
            onComplete?.Invoke();
            return;
        }

        StopMovement();
        _activeMovement = StartCoroutine(MoveRoutine(destination, travelDuration, onComplete));
    }

    public void StopMovement()
    {
        if (_activeMovement != null)
        {
            StopCoroutine(_activeMovement);
            _activeMovement = null;
        }
    }

    public bool IsMoving => _activeMovement != null;

    private IEnumerator MoveRoutine(GameObject target, float travelDuration, Action onComplete)
    {
        float originY = transform.position.y;
        float elapsed = 0f;

        while (elapsed < travelDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelDuration);
            float curved = easingCurve.Evaluate(t);

            float destinationY = target != null ? target.transform.position.y : transform.position.y;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.LerpUnclamped(originY, destinationY, curved),
                transform.position.z
            );

            yield return null;
        }

        if (target != null)
            transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);

        _activeMovement = null;
        onComplete?.Invoke();
    }

    private IEnumerator MoveRoutine(Vector3 destination, float travelDuration, Action onComplete)
    {
        float originY = transform.position.y;
        float elapsed = 0f;

        while (elapsed < travelDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelDuration);
            float curved = easingCurve.Evaluate(t);

            transform.position = new Vector3(
                transform.position.x,
                Mathf.LerpUnclamped(originY, destination.y, curved),
                transform.position.z
            );

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, destination.y, transform.position.z);
        _activeMovement = null;
        onComplete?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (targetObject == null) return;
        Gizmos.color = Color.cyan;

        Vector3 yOnlyTarget = new Vector3(transform.position.x, targetObject.transform.position.y, transform.position.z);
        Gizmos.DrawLine(transform.position, yOnlyTarget);
        Gizmos.DrawSphere(yOnlyTarget, 0.1f);
    }
#endif
}