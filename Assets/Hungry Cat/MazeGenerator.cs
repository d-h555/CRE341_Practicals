using System;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 20, height = 20;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    private int[,] maze;
    private System.Random random = new System.Random();

    void Start()
    {
        maze = new int[width, height];
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze()
    {
        // Initialize maze with walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1; // 1 represents a wall
            }
        }

        // Start maze generation from a random odd cell
        int startX = random.Next(width / 2) * 2 + 1;
        int startY = random.Next(height / 2) * 2 + 1;
        maze[startX, startY] = 0; // 0 represents a path

        CarveMaze(startX, startY);
    }

    void CarveMaze(int x, int y)
    {
        int[] directions = { 0, 1, 2, 3 };
        ShuffleArray(directions);

        foreach (int dir in directions)
        {
            int dx = 0, dy = 0;

            switch (dir)
            {
                case 0: dy = -2; break; // Up
                case 1: dy = 2; break;  // Down
                case 2: dx = -2; break; // Left
                case 3: dx = 2; break;  // Right
            }

            int nx = x + dx;
            int ny = y + dy;

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 1)
            {
                maze[nx - dx / 2, ny - dy / 2] = 0; // Remove wall between
                maze[nx, ny] = 0;
                CarveMaze(nx, ny);
            }
        }
    }

    void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                if (maze[x, y] == 1)
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else
                {
                    Instantiate(floorPrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
