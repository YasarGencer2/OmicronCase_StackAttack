using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] Transform target;

    void LateUpdate()
    {
        if (target != null)
        {
            var pos = target.position + offset;
            pos.x = 0;
            transform.position = pos;
        }
    }
} 
