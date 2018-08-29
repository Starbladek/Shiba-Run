using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 originPos;

    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {

    }
}