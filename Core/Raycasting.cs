
using System.Numerics;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core;

public static class Raycasting
{
    /// <summary>The result of a raycast.</summary>
    /// <param name="Block">The block that was hit. 0 if no block was hit.</param>
    /// <param name="BlockPosition">The blocks global position.</param>
    /// <param name="Normal">The direction the block was hit from.</param>
    /// <param name="Distance">The distance the ray travelled before it hit a block.</param>
    /// <remarks>
    /// Extra values can be determined like so:<br/>
    /// The previous block can be calculated by using <paramref name="BlockPosition"/> + <paramref name="Normal"/>.<br/>
    /// The precise hit point can be calculated by remembering the ray start position and direction, start + direction * <paramref name="Distance"/>.
    /// </remarks>
    public record RaycastResult(ushort Block, Vector3D<int> BlockPosition, Vector3D<int> Normal, float Distance);

    /// <summary>Starts marching in a direction.</summary>
    /// <returns>The next cube position along the path.</returns>
    /// <remarks>
    /// This will run until you stop iterating.
    /// Always starts with the next block.
    /// Ensure <paramref name="direction"/> is normalized before starting.
    /// </remarks>
    public static IEnumerable<Vector3D<int>> March(Vector3 origin, Vector3 direction)
    {
        Vector3D<int> blockPos = origin.Floor();
        int stepX = direction.X < 0 ? -1 : 1;
        int stepY = direction.Y < 0 ? -1 : 1;
        int stepZ = direction.Z < 0 ? -1 : 1;
        float deltaDistX = MathF.Abs(1f / direction.X);
        float deltaDistY = MathF.Abs(1f / direction.Y);
        float deltaDistZ = MathF.Abs(1f / direction.Z);
        float sideDistX = direction.X < 0 ? (origin.X - blockPos.X) * deltaDistX : (blockPos.X + 1f - origin.X) * deltaDistX;
        float sideDistY = direction.Y < 0 ? (origin.Y - blockPos.Y) * deltaDistY : (blockPos.Y + 1f - origin.Y) * deltaDistY;
        float sideDistZ = direction.Z < 0 ? (origin.Z - blockPos.Z) * deltaDistZ : (blockPos.Z + 1f - origin.Z) * deltaDistZ;
        while (true)
        {
            // Step along the shortest sideDist
            if (sideDistX < sideDistY && sideDistX < sideDistZ)
            {
                sideDistX += deltaDistX;
                blockPos.X += stepX;
            }
            else if (sideDistY < sideDistZ)
            {
                sideDistY += deltaDistY;
                blockPos.Y += stepY;
            }
            else
            {
                sideDistZ += deltaDistZ;
                blockPos.Z += stepZ;
            }
            yield return blockPos;
        }
    }
}