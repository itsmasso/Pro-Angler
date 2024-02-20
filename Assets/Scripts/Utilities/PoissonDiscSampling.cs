using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//credit: Sebastion Lague, Procedural Object Placement using Poisson Disc Sampling Algorithm

/*
How it works:
- Creates a grid and list of spawn points inside various cells
- Each spawn point created will have a radius around a point (circle around a point)
- Each cell's diagonal length will be equal to a point's radius
    - Because of this, there is only enough room for ONE point per cell and we can determine the cell size

- The purpose is that a point's radius zone cannot exceed a 5x5 block of cells, so we only need to check whether a point lies in the block of cells or not
 */
public static class PoissonDiscSampling
{
    //radius indicates radius around a point
    //sampleRegionSize indicates size of grid in units
    //numSamplesBeforeRejection indicates num of times before we exit a loop
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        //getting size of cell through pythagorean theorem (r^2 = s^2 + s^2) we solve for s to get side lengths
        float cellSize = radius / Mathf.Sqrt(2);

        //creating grid using two dimensional array: int[,]
        //determines how many cells fit in given regionSize by dividing sampleRegionSize by cellSize.
        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];

        //list of points that will be used for object spawns
        List<Vector2> points = new List<Vector2>();

        //list of spawnpoints used for generating new potential spawnpoints around the spawnpoint
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);

        //In this loop, we will pick a spawnpoint from the spawnPoints list and create a candidate point
        //If the candidate is accepted, then we add it to the list and if not, we remove the spawnpoint that we spawn the candidate around
        //This keeps going until there is no space left, and since candidates keep failing, this gradually removes all spawnpoints which exits the loop
        while (spawnPoints.Count > 0)
        {
            //choosing a random spawnpoint from the spawnPoints list
            int spawnIndex = Random.Range(0, spawnPoints.Count);

            //the chosen spawnPoint
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                //determine a random direction
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

                //Random.Range(radius, 2 * radius); indicates that we spawn the candidate point outside the cell because the minimum is the radius
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    //mark a cell in the grid as occupied and store the index of that point on the grid
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }

        }

        return points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float dist = (candidate - points[pointIndex]).sqrMagnitude;
                        if (dist < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

}
