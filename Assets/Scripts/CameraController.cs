using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float camera_speed = 1.0f;
    public float zoom_speed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal")*camera_speed,Input.GetAxis("Vertical")*camera_speed, Input.GetAxis("Mouse ScrollWheel")*zoom_speed));

    }
}
