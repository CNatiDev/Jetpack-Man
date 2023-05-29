using UnityEngine;

public class PlayerLeg : MonoBehaviour
{
    private ConfigurableJoint joint;

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        joint.targetRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Sin(Time.time) * 30));
    }
}
