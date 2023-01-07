using UnityEngine;
using Unity.Mathematics;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MazeWall mazeWall0 = new MazeWall(new int2(1, 1), new int2(2, 2));
        MazeWall mazeWall1 = new MazeWall(new int2(2, 2), new int2(1, 1));

        Debug.Log(mazeWall0 == mazeWall1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
