using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionRig : MonoBehaviour {
    public GameObject anchor;
    public List<GameObject> options = new List<GameObject>();
    public float radius;

    public bool active = false;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (active)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        } else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
	}

    void OnGUI()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(anchor.transform.position);
        transform.position = pos;

        float angle = 360 / options.Count;

        for (int i = 0; i < options.Count; i++)
        {

            float x = Mathf.Sin(Mathf.Deg2Rad * angle*i) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle*i) * radius;

            options[i].transform.position = new Vector3(pos.x + x, pos.y + y, 0);
        }
    }
}
