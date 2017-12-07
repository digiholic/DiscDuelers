using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour {
    private static int STOP_FRAMES = 5;

    public int ownerPlayer;
    
    public float ratio = 0.1f;
    public bool attack_waiting, move_waiting;
    public ParticleSystem deathBlast;

    private Rigidbody rb;
    private Renderer rend;

    private int stillFrames = 0;
    private bool attacking, moving;
    private bool doneMotion;
    private bool falling = false;
    private Vector3 lastMousePos;
    private Vector3 exitPosition;


    public int life = 5;
    public int maxAttacks = 2, maxMoves = 2;
    public int attacks, moves;


    private Dictionary<DamageSource, bool> damageSources = new Dictionary<DamageSource, bool>()
    {
        {DamageSource.CRASH, true },
        {DamageSource.STRIKE, true },
        {DamageSource.FALL, true }
    };

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        SwipeManager.OnSwipeDetected += OnSwipeDetected;
        attacks = maxAttacks;
        moves = maxMoves;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //Face the right direction
        if (rb.velocity.magnitude >= 0.01)
            transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(0, 90, 0);
        else
            transform.rotation = Quaternion.Euler(0, 90, 0);

        //If the done flag isn't set, but motion has stopped, set doneMotion
        if (!doneMotion && rb.velocity.magnitude < 0.01)
        {
            stillFrames += 1;
            if (stillFrames > STOP_FRAMES)
            {
                GameController.BroadcastEndRound();
                EndMotion();
            }
        }

        //Fall animation
        if (falling)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 90.0f), Time.deltaTime * 5);
	}

    public void TakeDamage(int amount, DamageSource source)
    {
        if (damageSources[source])
        {
            damageSources[source] = false;
            life -= amount;
            if (life == 0) Die(false);
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
                doneMotion = false;
                stillFrames = 0;
                CameraController.EnableMouseControl(); //We can move the map again
            }
        }
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
            IEnumerator respawnMethod = Respawn();
            StartCoroutine(respawnMethod);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        rend.enabled = true;
        transform.position = exitPosition;
        falling = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    #region Function Hooks
    public delegate void VoidEvent();
    public event VoidEvent StartTurnEvent;
    public event VoidEvent EndRoundEvent;
    public event VoidEvent EndTurnEvent;

    public delegate void ReadSwipe(SwipeData swipe);
    public event ReadSwipe BasicAttackEvent;
    public event ReadSwipe BasicMoveEvent;

    public delegate void DiscEvent(Disc d);
    public event DiscEvent OnAttackHit;
    public event DiscEvent OnGetAttacked;

    public event VoidEvent OnCrash;
    public event VoidEvent OnFall;

    private void Attack()
    {
        CameraController.DisableMouseControl(); //Since we're listening for a swipe, disable map control
        if (attacks > 0)
        {
            attack_waiting = true;
        }
    }

    private void Move()
    {
        CameraController.DisableMouseControl(); //Since we're listening for a swipe, disable map control
        if (moves > 0)
        {
            move_waiting = true;
        }
    }

    private void Activate()
    {

    }
    
    #endregion

    private void EndMotion()
    {
        Debug.Log("Ending Motion");
        attacking = false;
        moving = false;
        doneMotion = true;
        if (moves == 0 && attacks == 0)
            GameController.BroadcastEndTurn();
    }

    public void UnlockDamageSources()
    {
        damageSources[DamageSource.CRASH] = true;
        damageSources[DamageSource.STRIKE] = true;
        damageSources[DamageSource.FALL] = true;
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
            if (!moving) //The only time terrain is safe is during movement
                OnCrashWithTerrain();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Discs"))
        {
            Disc other = collision.gameObject.GetComponent<Disc>();
            if (moving)
            {
                OnGetAttacked(other);
            }
            if (attacking)
            { 
                other.SendMessage("OnGetAttacked", this);
                OnAttackWithDisc(other);
            }
        }
    }

    #endregion


    public void DefaultStartTurn()
    {
        attacks = maxAttacks;
        moves = maxMoves;
    }

    public void DefaultEndRound()
    {
        UnlockDamageSources();
    }

    public void DefaultEndTurn()
    {
        UnlockDamageSources();
    }

    public void DefaultOnGetAttacked(Disc d)
    {
        Debug.Log(gameObject.name + " has been attacked by " + d.gameObject.name);
        TakeDamage(1, DamageSource.STRIKE);
    }

    public void DefaultOnCrash()
    {
        TakeDamage(1, DamageSource.CRASH);
    }

    public void DefaultOnFall()
    {
        TakeDamage(1, DamageSource.FALL);
        if (life > 0) Die(true);
        if (GameController.instance.activeDisc == this)
        {
            GameController.BroadcastEndTurn();
        }
    }
}

public enum DamageSource
{
    CRASH,
    STRIKE,
    FALL
}
