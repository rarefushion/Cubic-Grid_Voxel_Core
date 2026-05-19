using System.Numerics;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core;

public static partial class Raycasting
{
    /// <summary>
    /// Handles callbacks during a chunk marching traversal.
    /// Implement to define per-step logic and control when the march stops.
    /// </summary>
    /// <remarks>
    /// Callbacks are invoked in the following order:
    /// <list type="number">
    /// <item><see cref="OnInitialize"/> once before stepping to ensure readiness.</item>
    /// <item><see cref="OnBlockStep"/> on every step to a new block.</item>
    /// <item><see cref="OnChunkEntered"/> when we cross into a new chunk boundary.</item>
    /// <item><see cref="OnComplete"/> when any of the above return false, concluding the march.</item>
    /// </list>
    /// Returning false from any callback immediately halts the march and invokes <see cref="OnComplete"/>.
    /// </remarks>
    public interface IChunkMarchHandler
    {
        /// <summary>Called before the march begins.</summary>
        /// <param name="chunkPosition">The chunk world position the ray originates from.</param>
        /// <param name="blockPosition">The global block the ray originates from.</param>
        /// <returns>False to stop marching.</returns>
        bool OnInitialize(Vector3D<int> chunkPosition, Vector3D<int> blockPosition);
        /// <summary>Called on every step to a new block.</summary>
        /// <param name="blockPosition">The global block position the ray has stepped to.</param>
        /// <returns>False to stop marching.</returns>
        bool OnBlockStep(Vector3D<int> blockPosition);
        /// <summary>Called when crossing a chunk boundry.</summary>
        /// <param name="chunkPosition">The global chunk position the ray has entered.</param>
        /// <returns>False to stop marching.</returns>
        bool OnChunkEntered(Vector3D<int> chunkPosition);
        /// <summary>Callen when the march concludes, dumping information about the march.</summary>
        /// <param name="blockPosition">The global block position the ray had ended on.</param>
        /// <param name="enteredFace">The face through which the ray entered the final block.</param>
        /// <param name="Distance">The precise distance the ray travelled before it hit a block.</param>
        void OnComplete(Vector3D<int> blockPosition, Direction enteredFace, float distance);
    }

    /// <summary>
    /// Marches a ray through chunks and blocks, invoking <paramref name="handler"/> callbacks at each step.
    /// </summary>
    /// <remarks>
    /// If maximum performance is not a concern, use <see cref="March"/> for simplicity.<br/>
    /// <typeparamref name="THandler"/> is constrained to <see langword="struct"/> 
    /// so the JIT can eliminate virtual call overhead.
    /// </remarks>
    /// <param name="origin">The global position the ray originates from.</param>
    /// <param name="direction">The direction of the ray. Ensure it is normalized before invoking.</param>
    /// <param name="handler">The handler invoked at each step of the march. The mutated instance is returned.</param>
    /// <returns>The mutated <paramref name="handler"/> after the march has concluded.</returns>
    /// <typeparam name="THandler">Must be a <see langword="struct"/> implementing <see cref="IChunkMarchHandler"/>.</typeparam>
    public static THandler MarchChunks<THandler, TChunkDims>
    (
        Vector3 origin,
        Vector3 direction,
        THandler handler
    )
    where THandler : struct, IChunkMarchHandler
    where TChunkDims : IChunkDims
    {
        static THandler Complete(THandler responder, in Vector3D<int> block, in Direction stepInverse, in float distance)
        {
            responder.OnComplete(block, stepInverse, distance);
            return responder;
        }
        Vector3D<int> blockPos = origin.Floor();
        Vector3D<int> chunkPos = blockPos.FloorTo(TChunkDims.Length);
        if (!handler.OnInitialize(chunkPos, blockPos))
            return Complete(handler, blockPos, default, 0);
        Direction stepInverseX = direction.X < 0 ? Direction.Right : Direction.Left;
        Direction stepInverseY = direction.Y < 0 ? Direction.Top : Direction.Bottom;
        Direction stepInverseZ = direction.Z < 0 ? Direction.Front : Direction.Back;
        int stepX = direction.X < 0 ? -1 : 1;
        int stepY = direction.Y < 0 ? -1 : 1;
        int stepZ = direction.Z < 0 ? -1 : 1;
        int stepChunkX = stepX * TChunkDims.Length;
        int stepChunkY = stepY * TChunkDims.Length;
        int stepChunkZ = stepZ * TChunkDims.Length;
        int chunkExitX = stepX < 0 ? chunkPos.X - 1 : chunkPos.X + TChunkDims.Length;
        int chunkExitY = stepY < 0 ? chunkPos.Y - 1 : chunkPos.Y + TChunkDims.Length;
        int chunkExitZ = stepZ < 0 ? chunkPos.Z - 1 : chunkPos.Z + TChunkDims.Length;
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
                if (blockPos.X == chunkExitX)
                {
                    chunkPos.X += stepChunkX;
                    chunkExitX += stepChunkX;
                    if (!handler.OnChunkEntered(chunkPos))
                        return Complete(handler, default, stepInverseX, sideDistX - deltaDistX);
                }
                if (!handler.OnBlockStep(blockPos))
                    return Complete(handler, blockPos, stepInverseX, sideDistX - deltaDistX);
            }
            else if (sideDistY < sideDistZ)
            {
                sideDistY += deltaDistY;
                blockPos.Y += stepY;
                if (blockPos.Y == chunkExitY)
                {
                    chunkPos.Y += stepChunkY;
                    chunkExitY += stepChunkY;
                    if (!handler.OnChunkEntered(chunkPos))
                        return Complete(handler, default, stepInverseY, sideDistY - deltaDistY);
                }
                if (!handler.OnBlockStep(blockPos))
                    return Complete(handler, blockPos, stepInverseY, sideDistY - deltaDistY);
            }
            else
            {
                sideDistZ += deltaDistZ;
                blockPos.Z += stepZ;
                if (blockPos.Z == chunkExitZ)
                {
                    chunkPos.Z += stepChunkZ;
                    chunkExitZ += stepChunkZ;
                    if (!handler.OnChunkEntered(chunkPos))
                        return Complete(handler, default, stepInverseZ, sideDistZ - deltaDistZ);
                }
                if (!handler.OnBlockStep(blockPos))
                    return Complete(handler, blockPos, stepInverseZ, sideDistZ - deltaDistZ);
            }
        }
    }
}