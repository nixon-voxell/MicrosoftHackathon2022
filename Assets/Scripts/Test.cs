using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

public class Test : MonoBehaviour
{
    public int Width;
    public int Height;

    public GameObject TilePrefab;
    public GameObject WallPrefab;

    // public GameObject[] Walls;
    // public GameObject[] Tiles;

    private void Start()
    {
        int cellCount = this.Width * this.Height;
        int wallCount = (this.Width - 1) * this.Height + this.Width * (this.Height - 1);
        int verticalWallStartIdx = (this.Width - 1) * this.Height;

        // clear memory to make sure that all initial data is false
        NativeArray<bool> na_cellStates = new NativeArray<bool>(cellCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);
        NativeArray<bool> na_wallStates = new NativeArray<bool>(wallCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);

        GenerateMazeJob generateMazeJob = new GenerateMazeJob
        {
            Width = this.Width,
            Height = this.Height,
            StartCell = int2.zero,
            Seed = 1,

            na_CellStates = na_cellStates,
            na_WallStates = na_wallStates,
        };

        JobHandle jobHandle = generateMazeJob.Schedule();
        jobHandle.Complete();

        // this.Tiles = new GameObject[cellCount];
        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                // int flattenIdx = MazeUtil.FlattenIndex(x, y, this.Width);
                // this.Tiles[flattenIdx] =
                Vector3 position = new Vector3(x, y, 0.1f);
                Quaternion rotation = Quaternion.identity;
                GameObject tile = Object.Instantiate(this.TilePrefab, position, rotation, this.transform);
            }
        }

        // horizontal walls
        for (int x = 0; x < this.Width - 1; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                int wallIdx = x + y * this.Width;

                if (na_wallStates[wallIdx] == false)
                {
                    Vector3 position = new Vector3(x + 0.5f, y, 0.0f);
                    Quaternion rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    GameObject wall = Object.Instantiate(this.WallPrefab, position, rotation, this.transform);
                }
            }
        }

        // vertical walls
        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height - 1; y++)
            {
                int wallIdx = x + y * this.Width;
                wallIdx += verticalWallStartIdx;

                if (na_wallStates[wallIdx] == false)
                {
                    Vector3 position = new Vector3(x, y + 0.5f, 0.0f);
                    Quaternion rotation = Quaternion.Euler(0.0f, 180.0f, 90.0f);
                    GameObject wall = Object.Instantiate(this.WallPrefab, position, rotation, this.transform);
                }
            }
        }

        na_cellStates.Dispose();
        na_wallStates.Dispose();
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        // destroy all instantiated prefabs
        // for (int t = 0; t < this.Tiles.Length; t++)
        // {
        //     Object.Destroy(this.Tiles[t]);
        // }
    }
}
