using UnityEngine;

public class FactoryMap : MonoBehaviour
{
    [SerializeField] private string mapName = "Abandoned Factory";
    [SerializeField] private Vector3 mapSize = new Vector3(200f, 50f, 200f);
    [SerializeField] private GameObject[] destructibleObjects;
    [SerializeField] private GameObject[] coverPoints;
    
    private void Start()
    {
        InitializeMap();
    }
    
    private void InitializeMap()
    {
        Debug.Log($"Loading map: {mapName}");
        SetupEnvironment();
        SetupCoverPoints();
    }
    
    private void SetupEnvironment()
    {
        // Setup factory environment
        // - Industrial containers
        // - Metal structures
        // - Destructible crates
        // - Dynamic obstacles
    }
    
    private void SetupCoverPoints()
    {
        if (coverPoints != null)
        {
            foreach (GameObject cover in coverPoints)
            {
                if (cover != null)
                {
                    cover.tag = "Cover";
                }
            }
        }
    }
    
    public Vector3 GetMapSize() => mapSize;
    public string GetMapName() => mapName;
}
