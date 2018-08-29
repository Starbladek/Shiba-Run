using UnityEngine;

public static class Vector3Extensions
{
    //With (change a single axis value on an object)
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        //"??" means it'll use the former value if it was given a non-null argument; otherwise, if that argument was NULL, it'll use the latter value
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }

    //Flat (force an object to y = 0)
    public static Vector3 Flat(this Vector3 original)
    {
        return new Vector3(original.x, 0, original.z);
    }

    //Calls TWO extension methods to get the normalized direction to another object
    //Without double extension: print(transform.position.DirectionTo(other.position));
    //With double extension:    print(transform.DirectionTo(other));
    public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
    {
        return Vector3.Normalize(destination - source);
    }
}