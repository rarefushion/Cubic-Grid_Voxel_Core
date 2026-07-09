using System.Numerics;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core.Math;

public static class ChunkMath<TDims> where TDims : IChunkDims
{
    public static readonly int mask;
    public static readonly int shift;

    public static int IndexByGlobalPos(Vector3D<int> pos) =>
        IndexByLocalPos(LocalPosByGlobalPos(pos));

    public static int IndexByLocalPos(Vector3D<int> pos) =>
        (pos.Z << shift | pos.Y) << shift | pos.X;

    public static Vector3D<int> LocalPosByGlobalPos(Vector3D<int> pos) => new
    (
        pos.X & mask,
        pos.Y & mask,
        pos.Z & mask
    );

    public static Vector3D<int> LocalPosByIndex(int index) =>
        new(index & mask, (index >> shift) & mask, (index >> (shift * 2)) & mask);

    public static bool PosLocal(Vector3D<int> pos) =>
        (pos.X & ~mask) == 0 &&
        (pos.Y & ~mask) == 0 &&
        (pos.Z & ~mask) == 0;

    public static Vector3D<int> FloorToChunk(Vector3D<int> pos) => new
    (
        pos.X & ~mask,
        pos.Y & ~mask,
        pos.Z & ~mask
    );

    static ChunkMath()
    {
        if (!BitOperations.IsPow2(TDims.Length))
            throw new Exception($"The IChunkDims.Length provided must be a power of 2. Currently: {TDims.Length}");
        mask = TDims.Length - 1;
        shift = BitOperations.TrailingZeroCount(TDims.Length);
    }
}
