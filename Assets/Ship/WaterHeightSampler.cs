using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterHeightSampler : MonoBehaviour
{
    public static WaterHeightSampler Instance { get; private set; }

    [SerializeField] private WaterSurface waterSurface;

    [Header("Cache")]
    [SerializeField] private float cacheLifetime = 0.025f;
    [SerializeField] private float gridSize = 0.3f;

    private readonly Dictionary<Vector2Int, CacheEntry> cache = new();

    private WaterSearchParameters searchParameters;
    private WaterSearchResult searchResult;

    private struct CacheEntry
    {
        public float height;
        public float time;
    }

    // ---------------- Singleton ----------------
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------------- Water ----------------


    public float GetWaterHeight(Vector3 worldPos)
    {
        Vector2Int key = Quantize(worldPos);

        if (cache.TryGetValue(key, out var entry))
        {
            if (Time.time - entry.time < cacheLifetime)
                return entry.height;
        }

        searchParameters.startPositionWS = worldPos;
        searchParameters.targetPositionWS = worldPos;
        searchParameters.error = 0.01f;
        searchParameters.maxIterations = 8;

        float height = worldPos.y;

        if (waterSurface.ProjectPointOnWaterSurface(searchParameters, out searchResult))
            height = searchResult.projectedPositionWS.y;

        cache[key] = new CacheEntry
        {
            height = height,
            time = Time.time
        };

        return height;
    }

    Vector2Int Quantize(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x / gridSize),
            Mathf.RoundToInt(pos.z / gridSize)
        );
    }
}
