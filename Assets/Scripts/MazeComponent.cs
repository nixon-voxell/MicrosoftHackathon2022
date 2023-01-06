using Unity.Mathematics;
using Unity.Entities;

public struct MazeComponent : IComponentData
{
    public int2 MazeSize;
    public Entity ent_MazeCell;
}
