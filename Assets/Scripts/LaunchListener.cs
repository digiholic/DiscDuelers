using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchListener : MonoBehaviour {
    public Disc disc;
    public Vector3 clickStartPos;

	// Use this for initialization
	void Start () {
        disc = GetComponent<Disc>();	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0))
        {
            clickStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 vec = clickStartPos - Input.mousePosition;
            vec.z = vec.y;
            vec.y = 0;
            float length = vec.magnitude;
            vec.Normalize();
            disc.AddForce(vec * 10);

        }
    }
}
