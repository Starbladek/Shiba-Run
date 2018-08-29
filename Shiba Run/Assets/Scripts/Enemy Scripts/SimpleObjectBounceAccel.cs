using UnityEngine;

[RequireComponent(typeof(SimpleObject))]
public class SimpleObjectBounceAccel : MonoBehaviour
{
    void OnEnable()
    {
        SimpleObject.OnBounce += IncreaseVelocity;
    }

    void OnDisable()
    {
        SimpleObject.OnBounce -= IncreaseVelocity;
    }

    void IncreaseVelocity(GameObject objectToAccel)
    {
        objectToAccel.GetComponent<SimpleObject>().velocity.x *= 2;
    }
}