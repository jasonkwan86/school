using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    //+/- will control direction
    public float speed;
    void Update()
    {
        //Even though we want this rotate around the x-axis, we do so along y instead because we have a 90-degree rotation on z
        //Doing so can also make this same script be recycled for the blue spinner object
        transform.Rotate(0, speed*Time.deltaTime, 0);
    }
}
