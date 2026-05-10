using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject terrainObject;
    public enum PrefabSpawnMode
    {
        Random,   // Pick one prefab at random
        All       // Spawn every prefab in the array
    }

    [System.Serializable]
    public class ObjectSpawnEntry
    {
        public string label = "Object";
        public GameObject[] prefabs;
        public Transform[] spawnPoints;

        [Tooltip("Random = spawn one prefab picked at random.\nAll = spawn every prefab in the array.")]
        public PrefabSpawnMode spawnMode = PrefabSpawnMode.Random;
    }

    [System.Serializable]
    public class SpawnPhase
    {
        public string label = "Phase";
        public float startTime = 0f;
        public float endTime = 15f;
        [Min(0.1f)] public float spawnInterval = 2f;
        [Min(1)] public int spawnsPerCycle = 1;
        public ObjectSpawnEntry[] objects;
    }

    [Header("Spawn Phases")]
    [SerializeField]
    private SpawnPhase[] phases = new SpawnPhase[]
    {
        new SpawnPhase { label = "Phase 1 - Starter",  startTime =  0f, endTime = 15f, spawnInterval = 3f, spawnsPerCycle = 1 },
        new SpawnPhase { label = "Phase 2 - Mid",      startTime = 15f, endTime = 30f, spawnInterval = 2f, spawnsPerCycle = 2 },
        new SpawnPhase { label = "Phase 3 - Intense",  startTime = 30f, endTime = 60f, spawnInterval = 1f, spawnsPerCycle = 3 },
    };

    [Header("Optional Parent")]
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
            yield return new WaitForSeconds(phases[_activePhaseIndex].spawnInterval);
            SpawnBatch(phases[_activePhaseIndex]);
        }
    }

    private void SpawnBatch(SpawnPhase phase)
    {
        if (phase.objects == null || phase.objects.Length == 0) return;

        List<ObjectSpawnEntry> validEntries = new List<ObjectSpawnEntry>();
        foreach (ObjectSpawnEntry entry in phase.objects)
        {
            if (entry.prefabs != null && entry.prefabs.Length > 0 &&
                entry.spawnPoints != null && entry.spawnPoints.Length > 0)
                validEntries.Add(entry);
            else
                Debug.LogWarning($"[SpawnSystem] [{phase.label}] Entry \"{entry.label}\" is missing prefabs or spawn points.");
        }

        if (validEntries.Count == 0) return;

        int count = Mathf.Min(phase.spawnsPerCycle, validEntries.Count);

        ShuffleList(validEntries);

        int totalSpawned = 0;

        for (int i = 0; i < count; i++)
        {
            ObjectSpawnEntry entry = validEntries[i];
            Transform spawnPoint = entry.spawnPoints[Random.Range(0, entry.spawnPoints.Length)];

            if (entry.spawnMode == PrefabSpawnMode.All)
            {
                // Spawn every prefab in the array at the chosen spawn point
                foreach (GameObject prefab in entry.prefabs)
                {
                    if (prefab == null) continue;

                    GameObject spawned = Instantiate(
                        prefab,
                        spawnPoint.position,
                        spawnPoint.rotation,
                        spawnedObjectContainer
                    );

                    _activeObjects.Add(spawned);
                    OnObjectSpawned(spawned, spawnPoint, entry, phase);
                    totalSpawned++;
                }
            }
            else // PrefabSpawnMode.Random
            {
                // Pick one prefab at random
                GameObject prefab = entry.prefabs[Random.Range(0, entry.prefabs.Length)];

                GameObject spawned = Instantiate(
                    prefab,
                    spawnPoint.position,
                    spawnPoint.rotation,
                    spawnedObjectContainer
                );

                _activeObjects.Add(spawned);
                OnObjectSpawned(spawned, spawnPoint, entry, phase);
                totalSpawned++;
            }
        }

        Debug.Log($"[SpawnSystem] [{phase.label}] Spawned {totalSpawned} object(s) across {count}/{validEntries.Count} entries | Elapsed: {_elapsedTime:F1}s");
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void UpdatePhase()
    {
        int newIndex = _activePhaseIndex < 0 ? 0 : _activePhaseIndex;

        if (_activePhaseIndex >= 0)
        {
            SpawnPhase current = phases[_activePhaseIndex];

            if (_elapsedTime >= current.endTime)
            {
                int next = _activePhaseIndex + 1;

                if (next >= phases.Length)
                {
                    StopAllCoroutines();
                    _isRunning = false;
                    Debug.Log("[SpawnSystem] All phases complete. Spawning stopped.");
                    return;
                }

                newIndex = next;
            }
            else
            {
                return;
            }
        }

        if (newIndex == _activePhaseIndex) return;

        int prevIndex = _activePhaseIndex;
        SpawnPhase previous = prevIndex >= 0 ? phases[prevIndex] : null;

        _activePhaseIndex = newIndex;
        SpawnPhase entered = phases[_activePhaseIndex];

        Debug.Log($"[SpawnSystem] Entered [{entered.label}] - " +
                  $"Interval: {entered.spawnInterval}s | " +
                  $"Ends at: {entered.endTime}s | " +
                  $"Spawns per cycle: {entered.spawnsPerCycle} | " +
                  $"Object types: {entered.objects?.Length ?? 0}");

        OnPhaseChanged(entered, previous);

        if (_isRunning)
        {
            StopAllCoroutines();
            StartCoroutine(SpawnRoutine());
        }
    }

    protected virtual void OnObjectSpawned(GameObject spawned, Transform fromPoint, ObjectSpawnEntry entry, SpawnPhase phase) { }

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
            return false;
        }

        for (int i = 0; i < phases.Length; i++)
        {
            SpawnPhase p = phases[i];

            if (p.spawnInterval <= 0f)
            {
                Debug.LogError($"[SpawnSystem] Phase [{i}] spawnInterval must be > 0.", this);
                valid = false;
            }

            if (p.endTime <= p.startTime)
            {
                Debug.LogError($"[SpawnSystem] Phase [{i}] endTime must be greater than startTime.", this);
                valid = false;
            }

            if (p.spawnsPerCycle < 1)
            {
                Debug.LogError($"[SpawnSystem] Phase [{i}] spawnsPerCycle must be >= 1.", this);
                valid = false;
            }

            if (p.objects == null || p.objects.Length == 0)
            {
                Debug.LogWarning($"[SpawnSystem] Phase [{i}] \"{p.label}\" has no object entries.", this);
                continue;
            }

            for (int j = 0; j < p.objects.Length; j++)
            {
                ObjectSpawnEntry entry = p.objects[j];

                if (entry.prefabs == null || entry.prefabs.Length == 0)
                    Debug.LogWarning($"[SpawnSystem] Phase [{i}] Entry [{j}] \"{entry.label}\" has no prefabs.", this);

                if (entry.spawnPoints == null || entry.spawnPoints.Length == 0)
                    Debug.LogWarning($"[SpawnSystem] Phase [{i}] Entry [{j}] \"{entry.label}\" has no spawn points.", this);
            }
        }

        if (!valid) enabled = false;
        return valid;
    }

#if UNITY_EDITOR
    private static readonly Color[] ObjectGizmoColors =
    {
        new Color(0.20f, 0.90f, 0.40f, 0.85f),
        new Color(1.00f, 0.75f, 0.10f, 0.85f),
        new Color(1.00f, 0.25f, 0.25f, 0.85f),
        new Color(0.40f, 0.60f, 1.00f, 0.85f),
        new Color(1.00f, 0.80f, 0.60f, 0.90f),
        new Color(0.90f, 0.60f, 1.00f, 0.90f),
    };

    private void OnDrawGizmosSelected()
    {
        if (phases == null) return;

        for (int i = 0; i < phases.Length; i++)
        {
            SpawnPhase phase = phases[i];
            if (phase.objects == null) continue;

            bool isActivePhase = (i == _activePhaseIndex);
            float phaseAlpha = isActivePhase ? 1f : 0.25f;
            float phaseRadius = isActivePhase ? 0.35f : 0.20f;

            for (int j = 0; j < phase.objects.Length; j++)
            {
                ObjectSpawnEntry entry = phase.objects[j];
                if (entry.spawnPoints == null) continue;

                Color objColor = ObjectGizmoColors[j % ObjectGizmoColors.Length];
                Gizmos.color = new Color(objColor.r, objColor.g, objColor.b, phaseAlpha);

                foreach (Transform pt in entry.spawnPoints)
                {
                    if (pt == null) continue;
                    Gizmos.DrawSphere(pt.position, phaseRadius);
                    Gizmos.DrawLine(pt.position, pt.position + pt.up);
                }
            }
        }
    }
#endif
}