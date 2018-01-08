﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour {
    #region Component Accessors
    [SerializeField]
    private ParticleSystem deathBlast;
    private Rigidbody rb;
    private Renderer rend;
    #endregion

    #region Function Hooks
    public delegate void VoidEvent();
    public event VoidEvent StartTurnEvent;
    public event VoidEvent EndRoundEvent;
    public event VoidEvent EndTurnEvent;

    public delegate void DiscEvent(Disc d);
    public event DiscEvent OnStrikeHit;
    public event DiscEvent OnGetAttacked;
    public event DiscEvent OnDiscCollision;

    public event VoidEvent OnCrash;
    public event VoidEvent OnFall;
    #endregion

    public int ownerPlayer;

    [System.NonSerialized]
    public bool inMotion;
    private int stillFrames = 0;
    private bool falling = false;
    private Vector3 lastMousePos;
    private Vector3 exitPosition;

    public bool active;
    public Vector3 clickStartPos;
    public GameObject slingMarker;

    private static int STOP_FRAMES = 2;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Quaternion uprightDirection = Quaternion.Euler(0, 180, 0);

        //If our Velocity is not zero
        if (rb.velocity.magnitude >= 0.01)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity) * uprightDirection;    //Rotate to face the right direction
            GameController.LockRound(gameObject);                                            //The round can't end while we're moving
            stillFrames = 0;                                                                 //If we're moving, reset the stillFrames counter
        }
        else
        {
            //transform.rotation = uprightDirection;                                         //Rotate back to default direction
            GameController.ReleaseRoundLock(gameObject);                                     //It is now safe to end the round
            
            //If the motion flag is still on, we need to check if it's safe to request the end of the round.
            //Since there will be brief periods of zero motion during bounces and collisions, we have to make sure we hit a threshold.
            //The threshold is defined as a static variable above.
            if (inMotion)
            {
                stillFrames += 1;
                if (stillFrames > STOP_FRAMES)
                {
                    GameController.RequestEndRound();
                }
            }
        }

        //Fall animation
        if (falling)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 90.0f), Time.deltaTime * 5);
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void Die(bool respawn)
    {
        ParticleSystem blast = Instantiate(deathBlast);
        blast.transform.position = transform.position;
        rend.enabled = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        if (respawn)
        {
            GameController.LockRound(gameObject);
            GameController.LockTurn(gameObject);
            IEnumerator respawnMethod = Respawn();
            StartCoroutine(respawnMethod);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        GameController.ReleaseRoundLock(gameObject);
        GameController.ReleaseTurnLock(gameObject);
        rend.enabled = true;
        transform.position = exitPosition;
        falling = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    void EndRound()
    {
        Debug.Log("Round Ending");
        EndRoundEvent();
    }

    #region Collision Functions
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SafeArea") //We've just fallen off stage
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation; //Unfreeze y position
            falling = true;
            exitPosition = transform.position - rb.velocity.normalized * 5;
            exitPosition.y = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Killbox")
        {
            OnFall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        { 
            OnCrash();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Discs"))
        {
            Disc other = collision.gameObject.GetComponent<Disc>();
            OnDiscCollision(other);
        }
    }
    #endregion
}

public enum DamageSource
{
    CRASH,
    STRIKE,
    FALL
}

public enum MotionType
{
    IDLE,
    STRIKE,
    MOVE
}
