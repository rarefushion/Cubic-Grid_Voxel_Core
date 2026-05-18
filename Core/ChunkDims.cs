using System.Numerics;

namespace GalensUnified.CubicGrid.Core;

public readonly struct ChunkDims(int length)
{
    /// <summary>The length of the chunk on one and every axis.</summary>
    public readonly int Length = length;
    /// <summary>The total size of the chunk.</summary>
    public readonly int Volume = length * length * length;
}