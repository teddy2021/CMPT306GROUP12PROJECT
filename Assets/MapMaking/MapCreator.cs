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
    [Range(1,100)]
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

    [SerializeField] private Tilemap Walls, Ground;
    [SerializeField] private TileBase[] sprites;

    private Vector2 sampleRegionSize;


    public void init(){
        genny.width = this.width;
        genny.height = this.height;
        genny.useSeed = this.useSeed;
        genny.seed = this.seed;
        genny.rules_file_path = this.rules_file_path;
        genny.Walls = this.Walls;
        genny.Ground = this.Ground;
        genny.sprites = this.sprites;
        genny.init();
        sampleRegionSize = new Vector2(width, height);
    }

    void Start(){
        init();
        generate();
    }


    public void generate(){
        genny.Generate();
        List<Vector2> locations = ObjectPlacer.GeneratePoints(radius, new Vector2(width, height), maxSamples);
        placeObjects(locations);
    }

    public void regenerate(){
        genny.GenerateNewMap();
        List<Vector2> locations = ObjectPlacer.GeneratePoints(radius, new Vector2(width, height), maxSamples);
        placeObjects(locations);
    }

    private void placeObjects(List<Vector2> locations){
        if(locations.Count > 3){
            Vector2 position;

            int player_location = Random.Range(0, locations.Count);
            position = locations[player_location];
            player.transform.position = new Vector3(position.x, position.y, 0);
            locations.RemoveAt(player_location);
            if(position.x == -width/2){
                if(position.y == -height/2){
                    campfire.transform.position = new Vector3(position.x +2, position.y + 2, 0);
                }
                else{
                    campfire.transform.position = new Vector3(position.x + 2, position.y - 2, 0);
                }
            }
            else{
                if(position.y == -height/2){
                    campfire.transform.position = new Vector3(position.x - 2, position.y + 2, 0);
                }
                else{
                    campfire.transform.position = new Vector3(position.x - 2, position.y - 2, 0);
                }
            }

            for(int i = 0; i < Random.Range(1, MaxKeys + 1); i += 1){
                int index = Random.Range(0, locations.Count);
                position = locations[index];
                Instantiate(key, new Vector3(position.x, position.y, 0), Quaternion.identity);
                locations.RemoveAt(index);
            }

            for(int i = 0; i < Random.Range(1, MaxStartingEnemies + 1); i += 1){
                int index = Random.Range(0, locations.Count);
                position = locations[index];
                Instantiate(enemy, new Vector3(position.x, position.y, 0), Quaternion.identity);
                locations.RemoveAt(index);
            }
        }
    }
}
