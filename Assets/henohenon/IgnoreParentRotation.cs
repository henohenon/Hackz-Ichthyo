using UnityEngine;

public sealed class IgnoreParentRotation : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = initialRotation;
    }
}