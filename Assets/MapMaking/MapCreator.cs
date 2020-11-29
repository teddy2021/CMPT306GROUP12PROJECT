using System.Collections;
using System.Collections.Generic;

using static ObjectPlacer;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
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

    private Vector2 sampleRegionSize;
    private List<GameObject> spawnedItems;

    public void init(){
        genny.width = this.width;
        genny.height = this.height;
        genny.useSeed = this.useSeed;
        genny.seed = this.seed;
        genny.rules_file_path = this.rules_file_path;
        genny.Walls = this.Walls;
        genny.Ground = this.Ground;
        genny.Boundries = this.Boundries;
        genny.sprites = this.sprites;
        genny.init();
        sampleRegionSize = new Vector2(width, height);
        spawnedItems = new List<GameObject>();
    }

    void Start(){
        init();
        generate();
    }


    public void reinit(){
        genny.width = this.width;
        genny.height = this.height;
        genny.useSeed = this.useSeed;
        genny.seed = this.seed;
        sampleRegionSize = new Vector2(width, height);
    }

    private void OnApplicationQuit() {
        clear();
    }

    public void generate(){
        genny.Generate();
        List<Vector2> locations = ObjectPlacer.GeneratePoints(radius, new Vector2(width, height), maxSamples);
        locations = VerifyLocations(locations);
        placeObjects(locations);
    }

    public void regenerate(){
        clear();
        genny.GenerateNewMap();
        List<Vector2> locations = ObjectPlacer.GeneratePoints(radius, new Vector2(width, height), maxSamples);
        locations = VerifyLocations(locations);
        placeObjects(locations);
    }



    public List<Vector2> VerifyLocations(List<Vector2> locations){
        List<Vector2> newLocations = new List<Vector2>();
        foreach(Vector2 point in locations){
            Vector3Int cellPoint = Walls.WorldToCell(new Vector3(point.x, point.y, 0));
            if(!Walls.HasTile(cellPoint)){
                newLocations.Add(point);
            }
        }
        return newLocations;
    }

    private Vector3 FindGoodLocation(Vector3 center){
        for(int i = -1; i <= 1; i += 1){
            for(int j = -1; j <= 1; j += 1){
                Vector3 translation = new Vector3(i, j, 0);
                Vector3Int cellPosition = Walls.WorldToCell(center + translation);
                if(!Walls.HasTile(cellPosition) && center + translation != center){
                    return center + translation;
                }
            }
        }
        return center;
    }

    private void placeObjects(List<Vector2> locations){
        if(locations.Count > 4){
            
            int player_location = 0;
            Vector2 position = locations[player_location];
            Vector3 world_position = new Vector3(position.x, position.y, 0);
            Vector3 campfire_location = FindGoodLocation(world_position);
            while(campfire_location == world_position){
                player_location = Mathf.Min(player_location + 1, locations.Count - 1);
                world_position = new Vector3(position.x, position.y, 0);
                campfire_location = FindGoodLocation(world_position);
                if(locations.Count - 1 == player_location){
                    break;
                }
            }
            player.transform.position = world_position;
            campfire.transform.position = campfire_location;
            locations.RemoveAt(player_location);

            int index = Random.Range(0, locations.Count - 1);
            position = locations[index];
            lift.transform.position = new Vector3(position.x, position.y, 0); 
            locations.RemoveAt(index);

            for(int i = 0; i < Random.Range(1, MaxKeys + 1); i += 1){
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                GameObject obj = Instantiate(key, new Vector3(position.x, position.y, 0), Quaternion.identity);
                spawnedItems.Add(obj);
                locations.RemoveAt(index);
            }

            for(int i = 0; i < Random.Range(1, MaxStartingEnemies + 1); i += 1){
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(enemy, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }


        }
    }


    void clear(){
        while(spawnedItems.Count > 0){
            Destroy(spawnedItems[0]);
            spawnedItems.RemoveAt(0);
        }
        Walls.ClearAllTiles();
        Ground.ClearAllTiles();
    }

}
