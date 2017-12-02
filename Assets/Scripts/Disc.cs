using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour {
    private Rigidbody rb;
    public ForceMode force;
    public float divisor;

    private Vector3 LastMousePos;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            LastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 lPos = Camera.main.ScreenToWorldPoint(LastMousePos);
            lPos.y = 0;
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.y = 0;
            Debug.DrawLine(lPos, mPos, Color.black);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 diff = Input.mousePosition - LastMousePos;
            diff.z = diff.y;
            diff.y = 0;
            rb.AddForce(diff / divisor, force);
        }
	}
}
