using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public class GenerateMazeJob : IJob
{
    public int Length;
    public int Width;
    public int2 StartCell;
    public uint Seed;

    /// <summary>Indicates if a cell has been visited or not.</summary>
    /// <remarks>True for visited, false for not visited.</remarks>
    public NativeArray<bool> na_CellStates;
    public NativeArray<MazeWall> na_Walls;
    /// <summary>Indicates if a wall has been broken or not.</summary>
    /// <remarks>True for broken, false for not broken.</remarks>
    public NativeArray<bool> na_WallStates;

    public void Execute()
    {
        Random rand = Random.CreateFromIndex(this.Seed);

        NativeList<int2> na_cellStacks = new NativeList<int2>(this.Width, Allocator.Temp);
        NativeArray<int2> na_neighborDirections = new NativeArray<int2>(4, Allocator.Temp);
        // left
        na_neighborDirections[0] = new int2(-1, 0);
        // right
        na_neighborDirections[1] = new int2(1, 0);
        // top
        na_neighborDirections[2] = new int2(0, 1);
        // bottom
        na_neighborDirections[3] = new int2(0, -1);
        NativeArray<int2> na_unvisitedCells = new NativeArray<int2>(4, Allocator.Temp);

        int visitedCellCount = 0;
        int totalCellCount = this.Length * this.Width;

        // add start cell as the first element in the stack
        na_cellStacks.Add(in this.StartCell);
        // set start cell to visited
        this.na_CellStates[MazeUtil.FlattenIndex(this.StartCell, this.Length)] = true;

        while (visitedCellCount < totalCellCount)
        {
            int2 currCell = na_cellStacks.LastElement();

            // check for neighbors to see if there are any unvisited cells
            int unvisitedCellCount = 0;
            for (int n = 0; n < na_neighborDirections.Length; n++)
            {
                int2 neighborCell = currCell + na_neighborDirections[n];

                // ignore if out of bounds
                if (
                    neighborCell.x < 0 || neighborCell.y < 0 ||
                    neighborCell.x >= this.Length || neighborCell.y >= this.Length
                ) continue;

                // if not visisted, increment count and add to the array
                if (!this.na_CellStates[MazeUtil.FlattenIndex(neighborCell, this.Length)])
                {
                    na_unvisitedCells[unvisitedCellCount++] = neighborCell;
                }
            }

            // if there are neighbor neighbor cell, proceed to select a random neighbor cell
            if (unvisitedCellCount > 0)
            {
                int unvisitedCellIdx = rand.NextInt(0, unvisitedCellCount);
                int2 unvisitedCell = na_unvisitedCells[unvisitedCellIdx];
                na_cellStacks.Add(in unvisitedCell);

                this.na_CellStates[MazeUtil.FlattenIndex(unvisitedCell, this.Length)] = true;
                // TODO: break a wall here

                visitedCellCount++;
            } else // else, we back track
            {
                na_cellStacks.Pop();
            }
        }
    }
}
