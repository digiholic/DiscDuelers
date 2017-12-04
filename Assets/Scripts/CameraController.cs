using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public float camera_speed = 1.0f;
    public float zoom_speed = 5.0f;

    public float pinch_zoom_speed = 0.5f;
    public float pan_speed = 0.5f;

    public float minZoom, maxZoom;
    public float minX, maxX;
    public float minZ, maxZ;

    public bool mouseMapControl = true;

	// Use this for initialization
	void Awake() {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        float x, y, z;
        x = Input.GetAxis("Horizontal")*camera_speed;
        y = Input.GetAxis("Vertical")*camera_speed;
        z = Input.GetAxis("Mouse ScrollWheel") * zoom_speed;

        if (mouseMapControl)
        {
            if (Input.touchCount == 1) //pan
            {
                Touch touchZero = Input.GetTouch(0);

                x = -(touchZero.deltaPosition.x * pan_speed);
                y = -(touchZero.deltaPosition.y * pan_speed);
            }

            if (Input.touchCount == 2) //zoom
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                z = -(deltaMagnitudeDiff * pinch_zoom_speed);
            }
        }

        transform.Translate(new Vector3(x,y,z));

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(transform.position.y, minZoom, maxZoom);
        clampedPosition.z = Mathf.Clamp(transform.position.z, minZ, maxZ);

        transform.position = clampedPosition;
    }

    public static void EnableMouseControl()
    {
        instance.mouseMapControl = true;
    }

    public static void DisableMouseControl()
    {
        instance.mouseMapControl = false;
    }
}
