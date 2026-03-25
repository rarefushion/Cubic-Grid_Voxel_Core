using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core.Math;

public static class CubicNeighborhood
{
    public static readonly Vector2D<int>[] MooreNeighborhood2D =
    [
        Vector2D<int>.UnitY,                        // N
        Vector2D<int>.UnitY + Vector2D<int>.UnitX,  // NE
        Vector2D<int>.UnitX,                        // E
        -Vector2D<int>.UnitY + Vector2D<int>.UnitX, // SE
        -Vector2D<int>.UnitY,                       // S
        -Vector2D<int>.UnitY - Vector2D<int>.UnitX, // SW
        -Vector2D<int>.UnitX,                       // W
        Vector2D<int>.UnitY - Vector2D<int>.UnitX   // NW
    ];

    public static readonly Vector3D<int>[] MooreNeighborhood =
    [
        // Faces
        -Vector3D<int>.UnitY,
        -Vector3D<int>.UnitX,
        -Vector3D<int>.UnitZ,
        Vector3D<int>.UnitY,
        Vector3D<int>.UnitX,
        Vector3D<int>.UnitZ,
        // Bottom Layer (Y: -1) Spiral
        -Vector3D<int>.UnitY - Vector3D<int>.UnitZ,                         // Back Edge
        -Vector3D<int>.One,                                                 // Back-Left Corner
        -Vector3D<int>.UnitY - Vector3D<int>.UnitX,                         // Left Edge
        -Vector3D<int>.UnitY - Vector3D<int>.UnitX + Vector3D<int>.UnitZ,   // Front-Left Corner
        -Vector3D<int>.UnitY + Vector3D<int>.UnitZ,                         // Front Edge
        -Vector3D<int>.UnitY + Vector3D<int>.UnitX + Vector3D<int>.UnitZ,   // Front-Right Corner
        -Vector3D<int>.UnitY + Vector3D<int>.UnitX,                         // Right Edge
        -Vector3D<int>.UnitY + Vector3D<int>.UnitX - Vector3D<int>.UnitZ,   // Back-Right Corner
        // Middle Layer (Y: 0) Spiral
        -Vector3D<int>.UnitX - Vector3D<int>.UnitZ,                         // Back-Left Edge
        -Vector3D<int>.UnitX + Vector3D<int>.UnitZ,                         // Front-Left Edge
         Vector3D<int>.UnitX + Vector3D<int>.UnitZ,                         // Front-Right Edge
         Vector3D<int>.UnitX - Vector3D<int>.UnitZ,                         // Back-Right Edge
        // Top Layer (Y: 1) Spiral
        Vector3D<int>.UnitY - Vector3D<int>.UnitZ,                          // Back Edge
        Vector3D<int>.UnitY - Vector3D<int>.UnitX - Vector3D<int>.UnitZ,    // Back-Left Corner
        Vector3D<int>.UnitY - Vector3D<int>.UnitX,                          // Left Edge
        Vector3D<int>.UnitY - Vector3D<int>.UnitX + Vector3D<int>.UnitZ,    // Front-Left Corner
        Vector3D<int>.UnitY + Vector3D<int>.UnitZ,                          // Front Edge
        Vector3D<int>.UnitY + Vector3D<int>.UnitX + Vector3D<int>.UnitZ,    // Front-Right Corner
        Vector3D<int>.UnitY + Vector3D<int>.UnitX,                          // Right Edge
        Vector3D<int>.UnitY + Vector3D<int>.UnitX - Vector3D<int>.UnitZ     // Back-Right Corner
    ];

    /// <summary>Calculates the positional overlapse for a cube growing in size.</summary>
    /// <param name="center">Center position of the cube.</param>
    /// <param name="halfBounds">Half-extents of the bounding region.</param>
    /// <param name="stride">Step size between positions.</param>
    /// <returns>Positions ordered from innermost to outermost cube, nearest the center first.</returns>
    public static IEnumerable<Vector3D<int>> ExpandingCubePositions(Vector3D<int> center, Vector3D<int> halfBounds, int stride)
    {
        Vector3D<int> coord = center / stride;
        Vector3D<int> halfSteps = halfBounds / stride;
        Vector3D<int> negRD = (-halfSteps + coord) * stride;
        Vector3D<int> posRD = (halfSteps + coord) * stride;
        int maxSteps = System.Math.Max(halfSteps.X, System.Math.Max(halfSteps.Y, halfSteps.Z));
        for (int i = 0; i <= maxSteps; i++)
        {
            Vector3D<int> tmpNeg = Vector3D<int>.Zero;
            Vector3D<int> tmpPos = Vector3D<int>.Zero;
            tmpNeg.X = System.Math.Max((-i + coord.X) * stride, negRD.X);
            tmpNeg.Y = System.Math.Max((-i + coord.Y) * stride, negRD.Y);
            tmpNeg.Z = System.Math.Max((-i + coord.Z) * stride, negRD.Z);
            tmpPos.X = System.Math.Min((i + coord.X) * stride, posRD.X);
            tmpPos.Y = System.Math.Min((i + coord.Y) * stride, posRD.Y);
            tmpPos.Z = System.Math.Min((i + coord.Z) * stride, posRD.Z);
            for (int x = tmpNeg.X; x <= tmpPos.X; x += stride)
            {
                if (x == tmpNeg.X || x == tmpPos.X)
                {
                    for (int y = tmpNeg.Y; y <= tmpPos.Y; y += stride)
                        for (int z = tmpNeg.Z; z <= tmpPos.Z; z += stride)
                            yield return new(x, y, z);
                }
                else
                {
                    for (int y = tmpNeg.Y; y <= tmpPos.Y; y += stride)
                        yield return new(x, y, tmpNeg.Z);
                    for (int y = tmpNeg.Y; y <= tmpPos.Y; y += stride)
                        yield return new(x, y, tmpPos.Z);

                    for (int z = tmpNeg.Z + stride; z <= tmpPos.Z - stride; z += stride)
                        yield return new(x, tmpNeg.Y, z);
                    for (int z = tmpNeg.Z + stride; z <= tmpPos.Z - stride; z += stride)
                        yield return new(x, tmpPos.Y, z);
                }
            }
        }
    }
}