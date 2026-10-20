using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private CloseRangeAttack closeRangeAttackRange;
    [SerializeField] private LongRangeAttack longRangeAttackRange;

    private void Update()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        worldPoint.z = 0;
        Vector3 faceDirection = (transform.position - worldPoint).normalized;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            closeRangeAttackRange.Execute(faceDirection);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            longRangeAttackRange.Execute(faceDirection);
        }
    }
}
