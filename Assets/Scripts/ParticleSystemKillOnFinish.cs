using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemKillOnFinish : MonoBehaviour {
    private ParticleSystem ps;

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule m = ps.main;
        m.loop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
