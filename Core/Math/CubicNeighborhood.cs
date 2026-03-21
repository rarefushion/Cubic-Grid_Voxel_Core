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
    
    /// <summary>
    /// Returns an enumerator that iterates through the Moore neighborhood of a given position up to a specified distance.
    /// </summary>
    /// <param name="position">The central starting coordinates.</param>
    /// <param name="distance">The maximum radius of the neighborhood to explore.</param>
    /// <param name="scale">The multiplier applied to the step size; defaults to 1.</param>
    /// <returns>An enumeration of <see cref="Vector3D{int}"/> points surrounding the center.</returns>
    public static IEnumerator<Vector3D<int>> GetNeighborhood(Vector3D<int> position, int distance, int scale = 1)
    {
        for (int d = 1; d <= distance; d++)
        for (int i = 0; i < 26; i++)
            yield return MooreNeighborhood[i] * d * scale + position;
    }
}