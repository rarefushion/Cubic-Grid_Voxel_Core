using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core.Math;

public static class ChunkMath<TDims> where TDims : IChunkDims
{
    public static int IndexByGlobalPos(Vector3D<int> pos) =>
        IndexByLocalPos(LocalPosByGlobalPos(pos));

    public static int IndexByLocalPos(Vector3D<int> pos) =>
        (pos.Z * TDims.Length + pos.Y) * TDims.Length + pos.X;

    public static Vector3D<int> LocalPosByGlobalPos(Vector3D<int> pos) => new
        (
            ((pos.X % TDims.Length) + TDims.Length) % TDims.Length,
            ((pos.Y % TDims.Length) + TDims.Length) % TDims.Length,
            ((pos.Z % TDims.Length) + TDims.Length) % TDims.Length
        );

    public static Vector3D<int> LocalPosByIndex(int index) =>
        new(index % TDims.Length, index / TDims.Length % TDims.Length, index / (TDims.Length * TDims.Length) % TDims.Length);

    public static bool PosLocal(Vector3D<int> pos) =>
        pos.X >= 0 && pos.X < TDims.Length &&
        pos.Y >= 0 && pos.Y < TDims.Length &&
        pos.Z >= 0 && pos.Z < TDims.Length;
}