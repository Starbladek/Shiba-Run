using UnityEngine;

public static class TransformExtensions
{
    //Calling an extension method from another extension method ,':)
    public static Vector3 DirectionTo(this Transform source, Transform destination)
    {
        return source.position.DirectionTo(destination.position);
    }
}