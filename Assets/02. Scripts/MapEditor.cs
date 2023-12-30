using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public GameObject[] monsterTypes;
    public int[] monsterCounts;
    public float gridSize = 1f;
    public Vector2Int gridDimensions = new Vector2Int(10, 10);
    
    public List<Vector3> monsterSpawnPoints = new List<Vector3>();

    private bool[,] spawnGrid;

    void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        spawnGrid = new bool[gridDimensions.x, gridDimensions.y];
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                // Initialize grid values
                spawnGrid[x, y] = false;
            }
        }
    }

    public void SetMonsterSpawnPoint(int x, int y, bool spawn)
    {
        /*
        if (x < 0 || y < 0 || x >= gridDimensions.x || y >= gridDimensions.y)
            return;

        spawnGrid[x, y] = spawn;
        */
        if (x < 0 || y < 0 || x >= gridDimensions.x || y >= gridDimensions.y)
            return;

        Vector3 spawnPoint = new Vector3(x * gridSize, 0, y * gridSize); // Assuming y is up-axis

        if (spawn && !monsterSpawnPoints.Contains(spawnPoint))
        {
            monsterSpawnPoints.Add(spawnPoint);
        }
        else if (!spawn)
        {
            monsterSpawnPoints.Remove(spawnPoint);
        }
    }
    
    void OnDrawGizmos()
    {
        DrawGrid();
    }
    
    private void DrawGrid()
    {
        Gizmos.color = Color.green; // Set the color of the grid lines

        for (int x = 0; x <= gridDimensions.x; x++)
        {
            for (int y = 0; y <= gridDimensions.y; y++)
            {
                // Draw the horizontal lines
                Vector3 start = transform.position + new Vector3(x * gridSize, 0, 0);
                Vector3 end = transform.position + new Vector3(x * gridSize, 0, gridDimensions.y * gridSize);
                Gizmos.DrawLine(start, end);

                // Draw the vertical lines
                start = transform.position + new Vector3(0, 0, y * gridSize);
                end = transform.position + new Vector3(gridDimensions.x * gridSize, 0, y * gridSize);
                Gizmos.DrawLine(start, end);
            }
        }

        // Draw spawn points if needed
        Gizmos.color = Color.red; // Set a different color for spawn points
        foreach (var point in monsterSpawnPoints)
        {
            Gizmos.DrawSphere(point, gridSize * 0.3f); // Adjust the size as needed
        }
    }
}
