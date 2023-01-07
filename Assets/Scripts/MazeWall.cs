using Unity.Mathematics;

public struct MazeWall : System.IEquatable<MazeWall>
{
    public int2 Cell0;
    public int2 Cell1;

    public MazeWall(int2 cell0, int2 cell1)
    {
        this.Cell0 = cell0;
        this.Cell1 = cell1;
    }

    public bool Equals(MazeWall otherWall)
    {
        return (
            this.Cell0.Equals(otherWall.Cell0) && this.Cell1.Equals(otherWall.Cell1) ||
            this.Cell0.Equals(otherWall.Cell1) && this.Cell1.Equals(otherWall.Cell0)
        );
    }

    public static bool operator ==(MazeWall wall0, MazeWall wall1)
    {
        return (
            wall0.Cell0.Equals(wall1.Cell0) && wall0.Cell1.Equals(wall1.Cell1) ||
            wall0.Cell0.Equals(wall1.Cell1) && wall0.Cell1.Equals(wall1.Cell0)
        );
    }

    public static bool operator !=(MazeWall wall0, MazeWall wall1)
    {
        return !(
            wall0.Cell0.Equals(wall1.Cell0) && wall0.Cell1.Equals(wall1.Cell1) ||
            wall0.Cell0.Equals(wall1.Cell1) && wall0.Cell1.Equals(wall1.Cell0)
        );
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(MazeWall)) return false;

        MazeWall otherWall = (MazeWall)obj;
        return (
            this.Cell0.Equals(otherWall.Cell0) && this.Cell1.Equals(otherWall.Cell1) ||
            this.Cell0.Equals(otherWall.Cell1) && this.Cell1.Equals(otherWall.Cell0)
        );
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
