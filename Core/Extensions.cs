using System.Numerics;
using System.Runtime.CompilerServices;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core;

public static class Extensions
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> Floor(this Vector3 pos) => new((int)MathF.Floor(pos.X), (int)MathF.Floor(pos.Y), (int)MathF.Floor(pos.Z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> FloorDiv(this Vector3 pos, int divisor) => new
    (
        (int)MathF.Floor(pos.X / divisor),
        (int)MathF.Floor(pos.Y / divisor),
        (int)MathF.Floor(pos.Z / divisor)
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> FloorDiv(this Vector3D<int> pos, int divisor) => new
    (
        MathUtils.FloorDiv(pos.X, divisor),
        MathUtils.FloorDiv(pos.Y, divisor),
        MathUtils.FloorDiv(pos.Z, divisor)
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> FloorTo(this Vector3 pos, int multiple) => pos.FloorDiv(multiple) * multiple;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> FloorTo(this Vector3D<int> pos, int multiple) => pos.FloorDiv(multiple) * multiple;
}