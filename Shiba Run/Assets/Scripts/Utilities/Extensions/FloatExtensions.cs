using UnityEngine;

public static class FloatExtensions
{
    /// <summary>
    /// Add some random value between (-amount, amount) to the float.
    /// <para>
    /// This would be ideal to use over Random.Range() when we know exactly where the origin point of the object is,
    /// and then want to shift it away from that point
    /// </para>
    /// </summary>
    public static float AddRandVal(this float original, float min, float max)
    {
        return original + Random.Range(min, max);
    }
}