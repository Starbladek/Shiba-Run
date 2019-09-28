using UnityEngine;

public static class Vector2Extensions
{
    //With (change a single axis value on an object)
    public static Vector2 With(this Vector2 original, float? x = null, float? y = null)
    {
        return new Vector2(x ?? original.x, y ?? original.y);
    }
}