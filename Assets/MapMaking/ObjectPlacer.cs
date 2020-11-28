using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPlacer{

    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int maxSamples = 30){
        float cellSize = radius/Mathf.Sqrt(radius);

        int width = Mathf.CeilToInt(sampleRegionSize.x/cellSize);
        int height = Mathf.CeilToInt(sampleRegionSize.y/cellSize);

        int[,] grid = new int[width, height];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize/2);

        while(spawnPoints.Count > 0){
            int index = Random.Range(0, spawnPoints.Count); // obtain index for new candidate
            Vector2 spawnCenter = spawnPoints[index]; // obtain central location for candidate
            bool candidateAccepted = false; 

            for(int i = 0; i < maxSamples; i += 1){
                float angle = Random.value * Mathf.PI * 2; 
                Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + direction * Random.Range(radius, 2*radius);

                if(IsValid(candidate, sampleRegionSize, cellSize, points, grid, radius)){
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x/cellSize), (int)(candidate.y/cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if(! candidateAccepted){
                spawnPoints.RemoveAt(index);
            }
        }
        translate(points, sampleRegionSize);
        return points;
    }



    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, List<Vector2> points, int[,] grid, float radius){
        if(candidate.x >= 0 & candidate.x < sampleRegionSize.x &
        candidate.y >= 0 & candidate.y < sampleRegionSize.y){

            int cellX = (int)(candidate.x/cellSize);
            int cellY = (int)(candidate.y/cellSize);
            for(int x = Mathf.Max(0, cellX - 2); x < Mathf.Min(grid.GetLength(0)-1, cellX + 2); x += 1){
            
                for(int y = Mathf.Max(0, cellY - 2); y < Mathf.Min(grid.GetLength(1)-1, cellY + 2); y += 1){
            
                    int pointIndex = grid[x,y] - 1;
            
                    if(pointIndex != -1){
            
                        float sqrdst = (candidate - points[pointIndex]).sqrMagnitude;
            
                        if(sqrdst < radius * radius){
            
                            return false;
                        }
                    }
                }
            }
            if(candidate.x/cellSize > grid.GetLength(0) -1 || candidate.y/cellSize > grid.GetLength(1) -1){
                return false;
            }
            return true;
        }
        return false;
    }


    static void translate(List<Vector2> points, Vector2 sampleRegionSize){
        for(int i = 0; i < points.Count; i += 1){
            Vector2 point = points[i];
            point.x -= sampleRegionSize.x/2;
            point.y -= sampleRegionSize.y/2;
            points[i] = point;
        }
     }

}
