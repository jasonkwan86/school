using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float speed = 0;
    
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent() will find the given component on the current object
        //Note: this can get expensive if done often, I suggest making these things public and dragging the component in manually to get the reference that way
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Notice how we set the input variables to be global such that they can be seen by the FixedUpdate Loop
        movementX = Input.GetAxis("Horizontal"); //Left/Right, A/D
        movementY = Input.GetAxis("Vertical"); //Up/Down, W/S
    }

    //FixedUpdate is like an Update() loop, however, it is normally reserved for physics calculations that occur on a fixed time step
    private void FixedUpdate()
    {
        //Notice we put movementY in the z component because our player moves on the x/z plane and not the x/y plane
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        //The rigidbody that our character is using is a physics-based component and thus any of it's function calls should be done in FixedUpdate
        //Note: we normally don't put player input here because we want to capture it every frame, therefore we will keep those in Update()
        rb.AddForce(movement * speed);
    }
}
