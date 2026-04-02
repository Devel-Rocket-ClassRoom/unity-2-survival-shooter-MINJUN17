using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 4, -3);
    private void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
