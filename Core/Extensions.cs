using System.Numerics;
using System.Runtime.CompilerServices;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core;

public static class Extensions
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<int> Floor(this Vector3 pos) => new((int)MathF.Floor(pos.X), (int)MathF.Floor(pos.Y), (int)MathF.Floor(pos.Z));
}