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
    [SerializeField] public GameObject RegSlime, BigSlime, PassiveSlime;
    [SerializeField] public GameObject lift;
    [Range(2,100)]
    [SerializeField] public int MinSlimes, MaxSlimes;
    [Range(1,100)]
    [SerializeField] public int MinBigSlimes, MaxBigSlimes;
    [Range(1,100)]
    [SerializeField] public int MinPassiveSlimes, MaxPassiveSlimes;
    [Range(1,10)]
    [SerializeField] public int MinKeys, MaxKeys;

    [SerializeField] private Tilemap Walls, Ground, Boundries;
    [SerializeField] private TileBase[] sprites;
    
    [Range(10,100)]
    [SerializeField] private int incr;

    [SerializeField] private GameObject fairy;

    private GridGraph graph;
    void Start()
    {
        init();
    }

    public void init(){
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

        creator.RegSlime = this.RegSlime;
        creator.MinSlimes = this.MinSlimes;
        creator.MaxSlimes = this.MaxSlimes;

        creator.BigSlime = this.BigSlime;
        creator.MinBigSlimes = this.MinBigSlimes;
        creator.MaxBigSlimes = this.MaxBigSlimes;

        creator.PassiveSlime = this.PassiveSlime;
        creator.MinPassiveSlimes = this.MinPassiveSlimes;
        creator.MaxPassiveSlimes = this.MaxPassiveSlimes;

        creator.Fairy = fairy;


        creator.lift = lift;
        
        creator.MinKeys = MinKeys;
        creator.MaxKeys = MaxKeys;

        creator.Walls = Walls;
        creator.Ground = Ground;
        creator.Boundries = Boundries;
        creator.sprites = sprites;
        creator.init();
        creator.generate();
        astar.Scan();
    }

    public void reinit(){
        AstarPath astar = GetComponent<AstarPath>();
        graph.SetDimensions(4*width, 4*height, 0.5f);
        creator.width = this.width;
        creator.height = this.height;
        creator.seed = this.seed;
        creator.useSeed = this.useSeed;
        
        int delta = Random.Range(1,10);
        creator.MinSlimes += delta;
        creator.MaxSlimes += delta;

        delta = Random.Range(1,10);
        creator.MinBigSlimes += delta;
        creator.MaxBigSlimes += delta;

        delta = Random.Range(1,10);
        creator.MinPassiveSlimes += delta;
        creator.MaxPassiveSlimes += delta;


        creator.lift = lift;

        creator.MaxKeys = MaxKeys;
        creator.reinit();
        creator.regenerate();
        astar.Scan();
    }

    public void fixedSizeIncrease(){
        width += incr;
        height += incr;
    }

    private float time = 0;
    private void FixedUpdate() {
        time += Time.deltaTime;
        if(time >= (float)Random.Range(45,75)){
            creator.placeNewSlimes();
            time = 0;
        }
    }
}
