using System.Runtime.CompilerServices;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core.Math;

public static class RegionMath
{
    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="IndexByGlobalPos(Vector3D{int} pos, int length)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexByGlobalPos(Vector3D<int> pos, int length) =>
        IndexByLocalPos(LocalPosByGlobalPos(pos, length), length);

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="IndexByLocalPos(Vector3D{int} pos, int length)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexByLocalPos(Vector3D<int> pos, int length) =>
        (pos.Z * length + pos.Y) * length + pos.X;

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="LocalPosByGlobalPos(Vector3D{int} pos, int length)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByGlobalPos(Vector3D<int> pos, int length) => new
        (
            ((pos.X % length) + length) % length,
            ((pos.Y % length) + length) % length,
            ((pos.Z % length) + length) % length
        );

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="LocalPosByIndex(int index, int length)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByIndex(int index, int length) =>
        new(index % length, index / length % length, index / (length * length) % length);

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="PosLocal(Vector3D{int} pos, int length)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool PosLocal(Vector3D<int> pos, int length) =>
        pos.X >= 0 && pos.X < length &&
        pos.Y >= 0 && pos.Y < length &&
        pos.Z >= 0 && pos.Z < length;

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="IndexByGlobalPosMasked(Vector3D{int} pos, int mask)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexByGlobalPosMasked(Vector3D<int> pos, int mask) =>
        IndexByLocalPos(LocalPosByGlobalPosMasked(pos, mask), mask + 1);

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="LocalPosByGlobalPosMasked(Vector3D{int} pos, int mask)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByGlobalPosMasked(Vector3D<int> pos, int mask) =>
        new(pos.X & mask, pos.Y & mask, pos.Z & mask);

    /// <include file='RegionMath.xml' path='MyDocs/MyMembers[@name="LocalPosByIndexMaskedShifted(int index, int mask, int shift)"]/*' />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> LocalPosByIndexMaskedShifted(int index, int mask, int shift) =>
        new(index & mask, (index >> shift) & mask, (index >> (shift * 2)) & mask);
}