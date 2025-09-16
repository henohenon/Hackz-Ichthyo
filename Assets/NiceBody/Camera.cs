using UnityEngine;

[RequireComponent(typeof(UnityEngine.Camera))]
public class Camera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new(0, 0, -10);

    private void LateUpdate()
    {
        if (target == null) 
            return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}