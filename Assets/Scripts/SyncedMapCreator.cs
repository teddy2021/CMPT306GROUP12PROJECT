using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class SyncedMapCreator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MapCreator creator;

        [SerializeField] private Generator genny;

    [Range(1,100)]
    [SerializeField] private int maxSamples;
    [Range(0.01f,8.0f)]
    [SerializeField] private float radius;

        // handed in values from unity
    [Range(10,500)]
    [SerializeField] private int width, height;
    [SerializeField] private bool useSeed;
    [SerializeField] private string seed;
    [SerializeField] private string rules_file_path;
    
    
    [SerializeField] public GameObject campfire;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject key;
    [SerializeField] public GameObject enemy;
    [SerializeField] public GameObject lift;
    [Range(2,100)]
    [SerializeField] private int MaxStartingEnemies;
    [Range(1,10)]
    [SerializeField] private int MaxKeys;

    [SerializeField] private Tilemap Walls, Ground, Boundries;
    [SerializeField] private TileBase[] sprites;

    private GridGraph graph;
    void Start()
    {
        init();
    }

    void init(){
        AstarPath astar = GetComponent<AstarPath>();
        graph = AstarPath.active.astarData.gridGraph;
       // graph.
        graph.center = new Vector3(-0.5f, -0.5f, 0);
        graph.SetDimensions(4*width, 4*height, 0.5f);
        creator.genny = genny;
        creator.maxSamples = maxSamples;
        creator.radius = radius;
        creator.width = width;
        creator.height = height;
        creator.useSeed = useSeed;
        creator.seed = seed;
        creator.rules_file_path = rules_file_path;
        creator.campfire = campfire;
        creator.player = player;
        creator.key = key;
        creator.enemy = enemy;
        creator.lift = lift;
        creator.MaxStartingEnemies = MaxStartingEnemies;
        creator.MaxKeys = MaxKeys;
        creator.Walls = Walls;
        creator.Ground = Ground;
        creator.Boundries = Boundries;
        creator.sprites = sprites;
        creator.init();
        creator.generate();
        astar.Scan();
    }

    void reinit(){
        AstarPath astar = GetComponent<AstarPath>();
        graph.SetDimensions(4*width, 4*height, 0.5f);
        creator.width = width;
        creator.height = height;
        creator.seed = seed;
        creator.useSeed = useSeed;
        creator.MaxStartingEnemies = MaxStartingEnemies;
        creator.MaxKeys = MaxKeys;
        creator.reinit();
        creator.regenerate();
        astar.Scan();
    }
}
