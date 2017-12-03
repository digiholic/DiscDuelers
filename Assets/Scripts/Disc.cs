using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour {
    public int ownerPlayer;
    public int life = 5;
    public int attacks = 2, moves = 2;

    public float ratio = 0.1f;
    public bool attack_waiting, move_waiting;
    public ParticleSystem deathBlast;

    private Rigidbody rb;
    private Renderer rend;

    private bool attacking, moving;
    private bool doneMotion;
    private bool falling = false;
    private Vector3 lastMousePos;
    private Vector3 exitPosition;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        SwipeManager.OnSwipeDetected += OnSwipeDetected;
    }

    // Update is called once per frame
    void Update() {
        //Face the right direction
        if (rb.velocity.magnitude >= 0.01)
            transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(0, 90, 0);
        else
            transform.rotation = Quaternion.Euler(0, 90, 0);

        //If the done flag isn't set, but motion has stopped, set doneMotion
        if (!doneMotion && rb.velocity.magnitude < 0.01)
            EndMotion();

        //Fall animation
        if (falling)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 90.0f), Time.deltaTime * 5);
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SafeArea") //We've just fallen off stage
        {
            falling = true;
            exitPosition = transform.position-rb.velocity.normalized*5;
            exitPosition.y = 5;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Killbox")
        {
            Die();
        }
    }

    void OnSwipeDetected(SwipeData data)
    {
        if (move_waiting || attack_waiting)
        {
            Vector2 swipe = data.GetSwipe();
            if (swipe != Vector2.zero)
            {
                if (move_waiting)
                {
                    moving = true;
                    moves -= 1;
                }

                if (attack_waiting)
                {
                    attacking = true;
                    attacks -= 1;
                }


                Vector3 dir = new Vector3(swipe.x, 0.0f, swipe.y);
                rb.AddForce(dir, ForceMode.Impulse);

                attack_waiting = false;
                move_waiting = false;
            }
        }
    }

    private void StartTurn()
    {

    }

    private void EndTurn()
    {

    }

    private void Activate()
    {

    }

    private void Attack()
    {
        if (attacks > 0)
        {
            attack_waiting = true;
            doneMotion = false;
        }
    }

    private void Move()
    {
        if (moves > 0)
        {
            move_waiting = true;
            doneMotion = false;
        }
    }
    
    private void EndMotion()
    {
        attacking = false;
        moving = false;
        doneMotion = true;
        if (moves == 0 && attacks == 0)
            GameController.EndTurn();
    }

    private void Die()
    {
        ParticleSystem blast = Instantiate(deathBlast);
        blast.transform.position = transform.position;
        rend.enabled = false;
        IEnumerator respawn = Respawn();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        StartCoroutine(respawn);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        rend.enabled = true;
        transform.position = exitPosition;
        falling = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
    }
}
