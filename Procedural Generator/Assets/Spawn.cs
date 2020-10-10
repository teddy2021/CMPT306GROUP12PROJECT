// CMPT 306
// ASSIGNMENT 1, PROCEDURALLY GENERATED WORLD
// KODY MANASTYRSKI
// KOM607
// 11223681

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour{
    
    [Range(0,100)]
    public int randomFillPercent;

    public int width;
    public int height;

    public bool useRandomSeed;
    public string seed;

    public GameObject alive;
    public GameObject dead;

    GameObject[,] tiles;
    int[,] map;

    void Start() {
        GenerateMap();
        RandomFillMap();
        for(int i = 0; i < 5; i += 1) {
            SmootMap();
        }
    }

    void GenerateMap() {
        map = new int[width, height];
        tiles = new GameObject[width, height];
    }

    void RandomFillMap() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }
        System.Random rand = new System.Random(seed.GetHashCode());
        
        for (int i = 0; i < width; i += 1) {
            for (int j = 0; j < height; j += 1) {
                map[i, j] = (rand.Next(0, 101) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    void SmootMap() {
        for(int i = 1; i < width - 1; i += 1) {
            for (int j = 1; j < height - 1; j += 1) {
                int surroundings = GetSurroundingCount(i, j);
                if(surroundings > 4) {
                    map[i, j] = 1;
                }
                else if(surroundings < 4) {
                    map[i, j] = 0;
                }
            }
        }
    }

    int GetSurroundingCount(int x, int y) {
        int surround = 0;
        for(int nx = x - 1; nx <= x + 1; nx += 1) {
            for(int ny = y - 1; ny <= y +1; ny += 1) {
                if(nx != x || ny != y) {
                    surround += map[nx, ny];
                }
            }
        }
        return surround;
    }

    void Update() {
        if(map != null) {
            for (int i = 0; i < width; i += 1) {
                for(int j = 0; j < height; j += 1) {
                    Vector4 pos = new Vector4((((float)i/(float)width) - 0.5f)*10.0f, (((float)j / (float)height) - 0.5f)* 10.0f, 1.0f, 1.0f);
                    Matrix4x4 view = Camera.main.worldToCameraMatrix;
                    if(tiles[i, j] != null) {
                        Destroy(tiles[i, j]);
                    }
                    tiles[i, j] = Instantiate(((map[i, j] == 0) ? alive: dead), view * pos, Quaternion.identity);
                }
            }
        }
    }

}
