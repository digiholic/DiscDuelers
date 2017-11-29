using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour {
    private Rigidbody rb;
    public ForceMode force;
    public float launchforce;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            launchforce += 0.1f;
        }
	    if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForce(new Vector3(launchforce,0,0),force);
            launchforce = 0.0f;
        }	
	}
}
