using UnityEngine;

public class RotatorComponent : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond = 0.5f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationsPerSecond * 360 * Time.deltaTime);
    }
}
