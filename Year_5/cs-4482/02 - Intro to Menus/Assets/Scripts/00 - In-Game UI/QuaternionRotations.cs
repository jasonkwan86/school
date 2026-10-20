using UnityEngine;
using UnityEngine.UI; //Don't forget this!

public class QuaternionRotations : MonoBehaviour
{ 
    public float speed;

    private float angle;
    
    //Public reference to our toggles
    public Toggle xToggle;
    public Toggle yToggle;
    public Toggle zToggle;

    void SetAxis(ref bool axis, bool value)
    {
        axis = value;
    }
    
    void Update()
    {

        Quaternion currentRotation = transform.rotation;
        
        angle += Time.deltaTime * speed;

        Vector3 axis = Vector3.zero;
        
        //replace our booleans in the inspector with the boolean values on the toggle
        if (xToggle.isOn)
        {
            axis += new Vector3(1.0f, 0.0f, 0.0f);
        }

        if (yToggle.isOn)
        {
            axis += new Vector3(0.0f, 1.0f, 0.0f);
        }

        if (zToggle.isOn)
        {
            axis += new Vector3(0.0f, 0.0f, 1.0f);
        }
        
        currentRotation = Quaternion.AngleAxis(angle, axis);
        
        transform.rotation = currentRotation;
    }
}
