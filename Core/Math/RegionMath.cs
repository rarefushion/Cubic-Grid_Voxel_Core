using System.Runtime.CompilerServices;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core.Math;

public static class RegionMath
{
    /// <summary>
    /// Calculates the 1D index of a 3D global position.
    /// </summary>
    /// <param name="pos"> The 3D position. </param>
    /// <param name="length"> The side length of the cubic region. </param>
    /// <returns> A 0 based linear index. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexByGlobalPos(Vector3D<int> pos, int length) =>
        IndexByLocalPos(LocalPosByGlobalPos(pos, length), length);

    /// <summary>
    /// Calculates the 1D index of a 3D local position within a cubic region.
    /// </summary>
    /// <param name="pos"> The 3D position relative to the region's origin. </param>
    /// <param name="length"> The side length of the cubic region. </param>
    /// <returns> A 0 based linear index. </returns>
    /// <remarks>
    /// This method assumes <paramref name="pos"/> is within the bounds [0, <paramref name="length"/>)
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexByLocalPos(Vector3D<int> pos, int length) =>
        (pos.Z * length + pos.Y) * length + pos.X;

    /// <summary>
    /// Wraps a global position into a local position within the cubic region.
    /// </summary>
    /// <param name="pos"> The 3D position. </param>
    /// <param name="length"> The side length of the cubic region. </param>
    /// <returns> A 3D position inside a cubic region originated at 0,0,0 </returns>
    /// <remarks>
    /// Uses a double-masking pattern to correctly handle negative coordinates,
    /// ensuring the result is always a positive offset within the region.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByGlobalPos(Vector3D<int> pos, int length) => new
        (
            ((pos.X % length) + length) % length,
            ((pos.Y % length) + length) % length,
            ((pos.Z % length) + length) % length
        );

    /// <summary>
    /// Decodes a 1D linear index into a 3D local position inside a cubic region.
    /// </summary>
    /// <param name="index"> The 0 based 1D linear index of the cubic region. </param>
    /// <param name="length"> The side length of the cubic region. </param>
    /// <returns> A 3D position inside a cubic region originated at 0,0,0 </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByIndex(int index, int length) =>
        new(index % length, index / length % length, index / (length * length) % length);

    /// <summary>
    /// Determines if a 3D position is within a cubic region.
    /// </summary>
    /// <param name="pos"> The 3D position. </param>
    /// <param name="lengthMask"> The side length of the cubic region. </param>
    /// <returns> True when pos is within the cubic region. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool PosLocal(Vector3D<int> pos, int length) =>
        pos.X >= 0 && pos.X < length &&
        pos.Y >= 0 && pos.Y < length &&
        pos.Z >= 0 && pos.Z < length;

    /// <summary>
    /// Calculates the 1D index of a 3D global position.
    /// </summary>
    /// <param name="pos"> The 3D position. </param>
    /// <param name="mask"> The bitwise mask length of the cubic region. (e.g 7, 15, 31) </param>
    /// <returns> A 0 based linear index. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexByGlobalPosMasked(Vector3D<int> pos, int mask) =>
        IndexByLocalPos(LocalPosByGlobalPosMasked(pos, mask), mask + 1);

    /// <summary>
    /// Wraps a global position into a local position within the cubic region.
    /// </summary>
    /// <param name="pos"> The 3D position. </param>
    /// <param name="mask"> The bitwise mask length of the cubic region. (e.g 7, 15, 31) </param>
    /// <returns> A 3D position inside a cubic region originated at 0,0,0 </returns>
    /// <remarks>
    /// Uses a double-masking pattern to correctly handle negative coordinates,
    /// ensuring the result is always a positive offset within the region.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByGlobalPosMasked(Vector3D<int> pos, int mask) =>
        new(pos.X & mask, pos.Y & mask, pos.Z & mask);

    /// <summary>
    /// Decodes a 1D linear index into a 3D local position inside a cubic region.
    /// </summary>
    /// <param name="index"> The 0 based 1D linear index of the cubic region. </param>
    /// <param name="mask"> The bitwise mask length of the cubic region. (e.g 7, 15, 31) </param>
    /// <param name="shift"> The number of bits to shift for each dimension (e.g., 4 for a 16-length region, 5 for 32).</param>
    /// <returns> A 3D position inside a cubic region originated at 0,0,0 </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByIndexMaskedShifted(int index, int mask, int shift) =>
        new(index & mask, (index >> shift) & mask, (index >> (shift * 2)) & mask);
}