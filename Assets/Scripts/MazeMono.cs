using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class MazeMono : MonoBehaviour
{
    public int2 MazeSize;
    public GameObject MazeCellPrefab;
}

internal class MazeBaker : Baker<MazeMono>
{
    public override void Bake(MazeMono authoring)
    {
        this.AddComponent<MazeComponent>(
            new MazeComponent
            {
                MazeSize = authoring.MazeSize,
                ent_MazeCell = GetEntity(authoring.MazeCellPrefab),
            }
        );
    }
}
