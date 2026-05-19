using System.Numerics;

namespace GalensUnified.CubicGrid.Core;

public interface IChunkDims
{
    /// <summary>The length of the chunk on one and every axis.</summary>
    static abstract int Length { get; }
    /// <summary>The total size of the chunk.</summary>
    static abstract int Volume { get; }
}

public readonly struct HalfChunkDims : IChunkDims
{
    public static int Length => 8;
    public static int Volume => 8 * 8 * 8;
}

public struct ChunkDims : IChunkDims
{
    public static int Length => 16;
    public static int Volume => 16 * 16 * 16;
}

public readonly struct DoubleChunkDims : IChunkDims
{
    public static int Length => 32;
    public static int Volume => 32 * 32 * 32;
}

public readonly struct QuadChunkDims : IChunkDims
{
    public static int Length => 64;
    public static int Volume => 64 * 64 * 64;
}

public readonly struct OctupleChunkDims : IChunkDims
{
    public static int Length => 128;
    public static int Volume => 128 * 128 * 128;
}