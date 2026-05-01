using System.Numerics;

namespace GalensUnified.CubicGrid.Core;

public enum Direction { Back, Front, Top, Bottom, Left, Right }

public static class Directions
{
    public static readonly Vector3[] directions =
    [
        -Vector3.UnitZ,
         Vector3.UnitZ,
         Vector3.UnitY,
        -Vector3.UnitY,
        -Vector3.UnitX,
         Vector3.UnitX
    ];

    public static Vector3 ToVector(this Direction direction) => directions[(int)direction];
}