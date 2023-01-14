using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

public class Test : MonoBehaviour
{
    public int Width;
    public int Height;

    void Start()
    {
        int cellCount = this.Width * this.Height;
        int wallCount = (this.Width - 1) * this.Height + this.Width * (this.Height - 1);
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

        na_cellStates.Dispose();
        na_wallStates.Dispose();
    }

    void Update()
    {
        
    }
}
