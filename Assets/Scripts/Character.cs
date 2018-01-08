using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public Disc disc;

    public int life = 5;
    public int maxStrikes = 2;
    public int maxMoves = 2;

    [System.NonSerialized]
    public int strikes;
    [System.NonSerialized]
    public int moves;
    
    public MotionType motion = MotionType.IDLE;
    public List<DiscSwipeOption> moveOptions = new List<DiscSwipeOption>();
    public List<DiscSwipeOption> strikeOptions = new List<DiscSwipeOption>();

    public Material playerMat;

    #region GUI variables
    //These variables let the GUI know what to display
    public bool ReadyForOption = false;
    public bool StrikePending = false;
    public bool MovePending = false;
    #endregion

    private Dictionary<DamageSource, bool> damageSources = new Dictionary<DamageSource, bool>()
    {
        {DamageSource.CRASH, true },
        {DamageSource.STRIKE, true },
        {DamageSource.FALL, true }
    };

    // Use this for initialization
    void Start () {
        disc = GetComponent<Disc>();

        disc.StartTurnEvent += DefaultStartTurn;
        disc.EndRoundEvent += DefaultEndRound;
        disc.EndTurnEvent += DefaultEndTurn;
        disc.OnDiscCollision += DefaultDiscCollision;
        disc.OnCrash += DefaultOnCrash;
        disc.OnFall += DefaultOnFall;

        moveOptions.Add(new DiscSwipeOption(DefaultBasicMove,1,0,OptionType.MOVE));
        strikeOptions.Add(new DiscSwipeOption(DefaultBasicStrike, 0, 1, OptionType.STRIKE));

        strikes = maxStrikes;
        moves = maxMoves;

        if (playerMat != null)
        {
            GetComponent<Renderer>().material = playerMat;
        }
	}

    public void RefreshMotions()
    {
        strikes = maxStrikes;
        moves = maxMoves;
    }

    public void TakeDamage(int amount, DamageSource source)
    {
        if (damageSources[source])
        {
            damageSources[source] = false;
            life -= amount;
            if (life == 0) disc.Die(false);
        }
    }

    public void UnlockDamageSources()
    {
        damageSources[DamageSource.CRASH] = true;
        damageSources[DamageSource.STRIKE] = true;
        damageSources[DamageSource.FALL] = true;
    }

    public void ExecuteOption(DiscSwipeOption opt)
    {
        SwipeManager.OnSwipeDetected += opt.swipeEvent;
        CameraController.DisableMouseControl();
        if (opt.opType == OptionType.MOVE  ) MovePending   = true;
        if (opt.opType == OptionType.STRIKE) StrikePending = true;
        ReadyForOption = false;
    }

    public void Ready()
    {
        ReadyForOption = true;
    }

    #region Default Hooks
    public void DefaultStartTurn()
    {
        RefreshMotions();
        ReadyForOption = true;
    }

    public void DefaultEndRound()
    {
        motion = MotionType.IDLE;
        disc.inMotion = false;
        if (moves == 0 && strikes == 0)
        {
            Debug.Log("ending turn");
            GameController.RequestEndTurn();
        }
        UnlockDamageSources();
    }

    public void DefaultEndTurn()
    {
        UnlockDamageSources();
    }


    public void DefaultOnCrash()
    {
        if (motion != MotionType.MOVE) //The only time terrain is safe is during movement
            TakeDamage(1, DamageSource.CRASH);
    }

    public void DefaultOnFall()
    {
        TakeDamage(1, DamageSource.FALL);
        if (life > 0) disc.Die(true);
        if (GameController.instance.activeDisc == disc)
        {
            GameController.RequestEndTurn();
        }
    }

    public void DefaultDiscCollision(Disc d)
    {

        //If we are idle or moving, the other disc is hitting us.
        if (motion != MotionType.STRIKE)
        {
            Debug.Log(gameObject.name + " has been attacked by " + d.gameObject.name);
            TakeDamage(1, DamageSource.STRIKE);
        }
        else //If we are attacking, we hit them. They're collision handler handles taking damage.
        {
            //By default, we don't do anything on strike. We're fine letting the opponent handle taking damage.
            //Some characters will do things when they attack people (sapping life, applying debuffs, etc.
            //So this hook is here for them.
        }
    }


    public void DefaultBasicMove(SwipeData swipe)
    {
        Vector2 swipeVector = swipe.GetSwipe();
        if (swipeVector != Vector2.zero)
        {
            Debug.Log("MOVING");
            motion = MotionType.MOVE;
            moves -= 1;
            Vector3 dir = new Vector3(swipeVector.x, 0.0f, swipeVector.y);
            disc.AddForce(dir);
            disc.inMotion = true;
            SwipeManager.OnSwipeDetected -= DefaultBasicMove;
            CameraController.EnableMouseControl(); //We can move the map again
        }
    }

    public void DefaultBasicStrike(SwipeData swipe)
    {
        Vector2 swipeVector = swipe.GetSwipe();
        if (swipeVector != Vector2.zero)
        {
            Debug.Log("STRIKING");
            motion = MotionType.STRIKE;
            strikes -= 1;
            Vector3 dir = new Vector3(swipeVector.x, 0.0f, swipeVector.y);
            disc.AddForce(dir);
            disc.inMotion = true;
            SwipeManager.OnSwipeDetected -= DefaultBasicStrike;
            CameraController.EnableMouseControl(); //We can move the map again
        }
    }
    #endregion

}

public class DiscSwipeOption
{
    public SwipeManager.OnSwipeDetectedHandler swipeEvent;
    public int moveCost;
    public int strikeCost;
    public OptionType opType;

    public DiscSwipeOption(SwipeManager.OnSwipeDetectedHandler ev, int move, int strike, OptionType opt)
    {
        swipeEvent = ev;
        moveCost = move;
        strikeCost = strike;
        opType = opt;
    }
}

public enum OptionType
{
    MOVE,
    STRIKE
}