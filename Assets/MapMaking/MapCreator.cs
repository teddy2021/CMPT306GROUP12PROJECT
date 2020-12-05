using System.Collections;
using System.Collections.Generic;

using static ObjectPlacer;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    public Generator genny;

    public int maxSamples;
    public float radius;

        // handed in values from unity
    public int width, height;
    public bool useSeed;
    public string seed;
    public string rules_file_path;
    
    
     public GameObject campfire;
     public GameObject player;
     public GameObject key;
     public GameObject enemy;
     public GameObject lift;
    
    public int MaxStartingEnemies;
    
    public int MaxKeys;

    public Tilemap Walls, Ground, Boundries;
    public TileBase[] sprites;

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
        regenerate();
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
        if(locations.Count >= MaxKeys + MaxStartingEnemies + 1){
            
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
            float dist = Vector3.Magnitude(new Vector3(position.x, position.y, 0) - player.transform.position);
            while( dist < 2){
                Debug.Log(dist);
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                dist = Vector3.Magnitude(new Vector3(position.x, position.y, 0) - player.transform.position);
            }
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
