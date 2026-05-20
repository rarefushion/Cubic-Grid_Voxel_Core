using System.Numerics;
using Silk.NET.Maths;

namespace GalensUnified.CubicGrid.Core.Math;

public static class DeterministicRandom
{
    public static uint seed;

    public static ushort Hash16(uint x)
    {
        x ^= seed;
        x ^= x >> 16;
        x *= 0x7feb352d;
        x ^= x >> 15;
        x *= 0x846ca68b;
        x ^= x >> 16;
        return (ushort)x;
    }

    public static float NormalizedRandom(uint x) => (float)Hash16(x) / (float)(ushort.MaxValue + 1);
    public static int RandomRange(uint x, int max) => (int)(Hash16(x) * (ulong)max >> 16);
    public static int RandomRange(uint x, int min, int max) => min + RandomRange(x, max - min);

    static uint Fold3(int x, int y, int z) => (uint)(x * 73856093 ^ y * 19349663 ^ z * 83492791);

    public static ushort Hash16(Vector3 vector) => Hash16(Fold3((int)vector.X, (int)vector.Y, (int)vector.Z));
    public static float NormalizedRandom(Vector3 vector) => (float)Hash16(vector) / (float)(ushort.MaxValue + 1);
    public static int RandomRange(Vector3 vector, int max) => (int)(Hash16(vector) * (ulong)max >> 16);
    public static int RandomRange(Vector3 vector, int min, int max) => min + RandomRange(vector, max - min);

    public static ushort Hash16(Vector3D<int> vector) => Hash16(Fold3(vector.X, vector.Y, vector.Z));
    public static float NormalizedRandom(Vector3D<int> vector) => (float)Hash16(vector) / (float)(ushort.MaxValue + 1);
    public static int RandomRange(Vector3D<int> vector, int max) => (int)(Hash16(vector) * (ulong)max >> 16);
    public static int RandomRange(Vector3D<int> vector, int min, int max) => min + RandomRange(vector, max - min);
}