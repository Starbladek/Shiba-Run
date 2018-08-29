using UnityEngine;

public static class MathEquations
{
    public static Vector2 CalculateLaunchVelocity(Vector2 startPos, Vector2 endPos, float gravity, float maxHeight)
    {
        if (startPos.y > maxHeight) maxHeight = startPos.y;//This makes sure the startPos.y is never higher than the maxHeight
        if (endPos.y > maxHeight) maxHeight = endPos.y;//This makes sure the endPos isn't higher than the maxHeight
        maxHeight -= startPos.y;    //why do we do this (it makes it so that maxHeight operates relative to zero?)

        float displacementX = endPos.x - startPos.x;
        float displacementY = endPos.y - startPos.y;

        float velocityX = displacementX / (Mathf.Sqrt(-2 * maxHeight / gravity) + Mathf.Sqrt(2 * (displacementY - maxHeight) / gravity));
        float velocityY = Mathf.Sqrt(-2 * gravity * maxHeight);

        return new Vector2(velocityX, velocityY);
    }
}