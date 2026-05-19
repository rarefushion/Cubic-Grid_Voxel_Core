using System.Runtime.CompilerServices;

namespace GalensUnified.CubicGrid.Core;

public static class MathUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FloorDiv(int a, int b)
    {
        int q = a / b;
        return q - (((a ^ b) < 0 && q * b != a) ? 1 : 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FloorTo(int value, int multiple) => FloorDiv(value, multiple) * multiple;
}