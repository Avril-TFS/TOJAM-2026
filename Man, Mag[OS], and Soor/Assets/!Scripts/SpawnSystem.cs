using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnSystem : MonoBehaviour
{

    [System.Serializable]
    public class SpawnPhase
    {
        public string label = "Phase";

        public float startTime = 0f;

        public int spawnCount = 1;

        
        public float spawnInterval = 2f;

       
        public float staggerDelay = 0.3f;

        public GameObject[] prefabs;
    }


    [Header("Spawn Phases")]
    [Tooltip("Add one entry per phase, sorted by ascending Start Time.")]
    [SerializeField]
    private SpawnPhase[] phases = new SpawnPhase[]
    {
        new SpawnPhase { label = "Phase 1 – Starter",  startTime =  0f, spawnCount = 1, spawnInterval = 3f },
        new SpawnPhase { label = "Phase 2 – Mid",      startTime = 15f, spawnCount = 2, spawnInterval = 2f },
        new SpawnPhase { label = "Phase 3 – Intense",  startTime = 30f, spawnCount = 3, spawnInterval = 1f },
    };

    [Header("Spawn Locations")]
    [Tooltip("One Transform is chosen at random per spawned object.")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Optional Parent")]
    [Tooltip("Spawned objects are parented here if assigned — keeps the Hierarchy tidy.")]
    [SerializeField] private Transform spawnedObjectContainer;


    private float _elapsedTime;
    private int _activePhaseIndex = -1;
    private bool _isRunning;

    private readonly List<GameObject> _activeObjects = new List<GameObject>();


    private void Start()
    {
        if (!ValidateSetup()) return;

        _elapsedTime = 0f;
        _activePhaseIndex = -1;
        _isRunning = true;

        UpdatePhase(); 
    }

    private void Update()
    {
        if (!_isRunning) return;

        _elapsedTime += Time.deltaTime;
        UpdatePhase();
    }


    private IEnumerator SpawnRoutine()
    {
        while (_isRunning)
        {
            float interval = phases[_activePhaseIndex].spawnInterval;   
            yield return new WaitForSeconds(interval);
            SpawnBatch(phases[_activePhaseIndex]);
        }
    }

    private void SpawnBatch(SpawnPhase phase)
    {
        for (int i = 0; i < phase.spawnCount; i++)
        {
            GameObject prefab = GetRandomPrefabFromPhase(phase);
            Transform spawnPoint = GetRandomSpawnPoint();

            if (prefab == null || spawnPoint == null) continue;

            GameObject spawned = Instantiate(
                prefab,
                spawnPoint.position,
                spawnPoint.rotation,
                spawnedObjectContainer
            );

            _activeObjects.Add(spawned);
            OnObjectSpawned(spawned, spawnPoint, phase);
        }

        Debug.Log($"[SpawnSystem] [{phase.label}] Spawned {phase.spawnCount} object(s) | Elapsed: {_elapsedTime:F1}s");
    }


    private void UpdatePhase()
    {
        int newIndex = 0;
        for (int i = 0; i < phases.Length; i++)
        {
            if (_elapsedTime >= phases[i].startTime)
                newIndex = i;
        }

        if (newIndex == _activePhaseIndex) return;

        int prevIndex = _activePhaseIndex;
        SpawnPhase previous = prevIndex >= 0 ? phases[prevIndex] : null;

        _activePhaseIndex = newIndex;
        SpawnPhase current = phases[_activePhaseIndex];

        Debug.Log($"[SpawnSystem] Entered [{current.label}] — " +
                  $"{current.spawnCount} object(s) every {current.spawnInterval}s | " +
                  $"Prefab pool: {current.prefabs?.Length ?? 0} prefab(s)");

        OnPhaseChanged(current, previous);

        
        if (_isRunning)
        {
            StopAllCoroutines();
            StartCoroutine(SpawnRoutine());
        }
    }


    private GameObject GetRandomPrefabFromPhase(SpawnPhase phase)
    {
        if (phase.prefabs == null || phase.prefabs.Length == 0)
        {
            Debug.LogWarning($"[SpawnSystem] [{phase.label}] has no prefabs assigned.");
            return null;
        }
        return phase.prefabs[Random.Range(0, phase.prefabs.Length)];
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[SpawnSystem] No spawn points assigned.");
            return null;
        }
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }


    // Called right after each object is instantiated. Override to run custom setup.
    protected virtual void OnObjectSpawned(GameObject spawned, Transform fromPoint, SpawnPhase phase) { }

    // Called once whenever the active phase changes. Override to react to transitions.
    protected virtual void OnPhaseChanged(SpawnPhase newPhase, SpawnPhase oldPhase) { }


    public void PauseSpawning()
    {
        _isRunning = false;
        StopAllCoroutines();
        Debug.Log("[SpawnSystem] Paused.");
    }

    public void ResumeSpawning()
    {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(SpawnRoutine());
        Debug.Log("[SpawnSystem] Resumed.");
    }

    public void ClearAllSpawnedObjects()
    {
        foreach (GameObject obj in _activeObjects)
            if (obj != null) Destroy(obj);

        _activeObjects.Clear();
        Debug.Log("[SpawnSystem] All spawned objects cleared.");
    }

    public IReadOnlyList<GameObject> ActiveObjects => _activeObjects.AsReadOnly();
    public SpawnPhase ActivePhase => _activePhaseIndex >= 0 ? phases[_activePhaseIndex] : null;


    private bool ValidateSetup()
    {
        bool valid = true;

        if (phases == null || phases.Length == 0)
        {
            Debug.LogError("[SpawnSystem] 'Phases' array is empty.", this);
            valid = false;
        }
        else
        {
            for (int i = 0; i < phases.Length; i++)
            {
                if (phases[i].prefabs == null || phases[i].prefabs.Length == 0)
                    Debug.LogWarning($"[SpawnSystem] Phase [{i}] \"{phases[i].label}\" has no prefabs.", this);

                if (phases[i].spawnCount < 1)
                {
                    Debug.LogError($"[SpawnSystem] Phase [{i}] spawnCount must be >= 1.", this);
                    valid = false;
                }

                if (phases[i].spawnInterval <= 0f)
                {
                    Debug.LogError($"[SpawnSystem] Phase [{i}] spawnInterval must be > 0.", this);
                    valid = false;
                }
            }
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[SpawnSystem] 'Spawn Points' is empty.", this);
            valid = false;
        }

        if (!valid) enabled = false;
        return valid;
    }


#if UNITY_EDITOR
    private static readonly Color[] PhaseGizmoColors =
    {
        new Color(0.20f, 0.90f, 0.40f, 0.85f),
        new Color(1.00f, 0.75f, 0.10f, 0.85f),
        new Color(1.00f, 0.25f, 0.25f, 0.85f),
        new Color(0.40f, 0.60f, 1.00f, 0.85f),
    };

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = _activePhaseIndex >= 0
            ? PhaseGizmoColors[_activePhaseIndex % PhaseGizmoColors.Length]
            : PhaseGizmoColors[0];

        foreach (Transform pt in spawnPoints)
        {
            if (pt == null) continue;
            Gizmos.DrawSphere(pt.position, 0.3f);
            Gizmos.DrawLine(pt.position, pt.position + pt.up);
        }
    }
#endif
}