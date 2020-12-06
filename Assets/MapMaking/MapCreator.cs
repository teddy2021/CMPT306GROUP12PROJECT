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
     public GameObject RegSlime, BigSlime, PassiveSlime;
     public int MinSlimes, MaxSlimes;
     public int MinBigSlimes, MaxBigSlimes;
     public int MinPassiveSlimes, MaxPassiveSlimes;
     public GameObject lift;
    
    
    public int MinKeys, MaxKeys;

    public Tilemap Walls, Ground, Boundries;
    public TileBase[] sprites;

    private Vector2 sampleRegionSize;
    private List<GameObject> spawnedItems;

    public GameObject Fairy;

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
        if(locations.Count >= MaxKeys + 3){
            
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


            for(int x = -2; x <= 2; x += 1){ // iterate from 1 to the left, and 1 up to 1 right and 1 down
                for(int y = -2; y <= 2; y += 1){
                    Vector3 point = new Vector3(
                        world_position.x + x, // 
                        world_position.y + y, // 
                        world_position.z
                    );
                    Vector3Int cell_point = Walls.WorldToCell(point);
                    Walls.SetTile(cell_point, null);
                }
                Walls.SetTile(
                    Walls.WorldToCell(
                    new Vector3(
                        world_position.x - 3, 
                        world_position.y + x, 
                        world_position.z)), 
                    sprites[0]
                );
                Walls.SetTile(
                    Walls.WorldToCell(
                    new Vector3(
                        world_position.x + 3, 
                        world_position.y + x, 
                        world_position.z)), 
                    sprites[0]
                );
                Walls.SetTile(
                    Walls.WorldToCell(
                    new Vector3(
                        world_position.x + x, 
                        world_position.y - 2, 
                        world_position.z)), 
                    sprites[0]
                );
                Walls.SetTile(
                    Walls.WorldToCell(
                    new Vector3(
                        world_position.x + x, 
                        world_position.y + 3, 
                        world_position.z)), 
                    sprites[0]
                );

            }
            Walls.RefreshAllTiles();
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

            for(int i = 0; i < Random.Range(MinKeys, MaxKeys + 1); i += 1){
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                GameObject obj = Instantiate(key, new Vector3(position.x, position.y, 0), Quaternion.identity);
                spawnedItems.Add(obj);
                locations.RemoveAt(index);
            }
        }
        placeSlimes();
    }


    void clear(){
        while(spawnedItems.Count > 0){
            Destroy(spawnedItems[0]);
            spawnedItems.RemoveAt(0);
        }
        Boundries.ClearAllTiles();
        Walls.ClearAllTiles();
        Ground.ClearAllTiles();
    }


    public void placeSlimes(){
        List<Vector2> locations = ObjectPlacer.GeneratePoints(radius, sampleRegionSize, maxSamples);
        locations = VerifyLocations(locations);
        if(MinSlimes + MinBigSlimes + MinPassiveSlimes < locations.Count &
        locations.Count >= MaxSlimes + MaxBigSlimes + MaxPassiveSlimes + 1){
            // Place Regular Slimes
            int index;
            Vector2 position;
            for(int i = 0; i < Random.Range(MinSlimes, MaxSlimes); i += 1){
                index = Random.Range(1, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(RegSlime, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }
            
            for(int i = 0; i < Random.Range(MinBigSlimes, MaxBigSlimes); i += 1){
                index = Random.Range(1, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(BigSlime, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }

            
            for(int i = 0; i < Random.Range(MinPassiveSlimes, MaxPassiveSlimes); i += 1){
                index = Random.Range(1, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(PassiveSlime, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }
        }
    }

    public void placeNewSlimes(){
        
        List<Vector2> locations = ObjectPlacer.GeneratePoints(radius, sampleRegionSize, maxSamples);
        locations = VerifyLocations(locations);
        
        int min_reg = Random.Range(0, MinSlimes);
        int max_reg = Random.Range(min_reg, MaxSlimes);

        int min_big = Random.Range(0, MinBigSlimes);
        int max_big = Random.Range(min_big, MaxBigSlimes);

        int min_pas = Random.Range(0, MinPassiveSlimes);
        int max_pas = Random.Range(min_pas, MaxPassiveSlimes);

        if(min_reg + min_big + min_pas + 1 < locations.Count &
        locations.Count <= max_reg + max_big + max_pas + 1){
            // Place Regular Slimes
            int index;
            Vector2 position;
            for(int i = 0; i < Random.Range(min_reg, max_reg); i += 1){
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(RegSlime, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }
            
            for(int i = 0; i < Random.Range(min_big, max_big); i += 1){
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(BigSlime, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }

            
            for(int i = 0; i < Random.Range(min_pas, max_pas); i += 1){
                index = Random.Range(0, locations.Count - 1);
                position = locations[index];
                spawnedItems.Add(Instantiate(PassiveSlime, new Vector3(position.x, position.y, 0), Quaternion.identity));
                locations.RemoveAt(index);
            }
        }        
    }

}
