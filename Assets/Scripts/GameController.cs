using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public bool roundEndRequested= false;
    public bool turnEndRequested = false;

    public List<PlayerController> players;

    public List<Lock> RoundLock = new List<Lock>();
    public List<Lock> TurnLock = new List<Lock>();
    public Queue<Disc> turnOrder = new Queue<Disc>();

    public Disc activeDisc;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartRound();
    }

	// Update is called once per frame
	void Update () {
	    if (roundEndRequested && (RoundLock.Count == 0))
        {
            roundEndRequested = false;
            BroadcastMessage("EndRound");
            StartRound();
        }
        if (turnEndRequested && (TurnLock.Count == 0))
        {
            turnEndRequested = false;
            BroadcastMessage("EndTurn");
            Disc old = activeDisc;
            activeDisc = turnOrder.Dequeue();
            turnOrder.Enqueue(old);
            StartRound();
        }
    }

    void StartRound()
    {
        activeDisc.SendMessage("Ready");
    }

    /// <summary>
    /// This method is called when an object is requesting that the round doesn't end. For example, when playing an animation or still in motion.
    /// The object is added on to the list so that we can see what's holding up the turn order.
    /// </summary>
    /// <param name="go">The game object requesting the round be held</param>
    public static void LockRound(GameObject go, string message)
    {
        instance.RoundLock.Add(new Lock(go,message));
        Debug.Log(go.name + " is locking the round: " + message);
    }
    /// <summary>
    /// This method is called when an object that was previously requesting the round not end is now okay with it ending. This is called when whatever operation
    /// it was trying to do clears up.
    /// </summary>
    /// <param name="go">The game object that no longer requests the round stay open</param>
    public static void ReleaseRoundLock(GameObject go)
    {
        foreach (Lock l in instance.RoundLock)
        {
            if (l.lockSource == go)
            {
                instance.RoundLock.Remove(l);
                Debug.Log(go.name + " is allowing the round to end");
            }
        }
    }
    /// <summary>
    /// This method is called when an object is requesting that the turn doesn't end. For example, when playing an animation or still in motion.
    /// The object is added on to the list so that we can see what's holding up the turn order.
    /// </summary>
    /// <param name="go">The game object requesting the turn be held</param>
    public static void LockTurn(GameObject go, string message)
    {
        instance.TurnLock.Add(new Lock(go,message));
        Debug.Log(go.name + " is locking the turn: " + message);
    }
    /// <summary>
    /// This method is called when an object that was previously requesting the turn not end is now okay with it ending. This is called when whatever operation
    /// it was trying to do clears up.
    /// </summary>
    /// <param name="go">The game object that no longer requests the turn stay open</param>
    public static void ReleaseTurnLock(GameObject go)
    {
        foreach (Lock l in instance.TurnLock)
        {
            if (l.lockSource == go)
            {
                instance.TurnLock.Remove(l);
                Debug.Log(go.name + " is allowing the turn to end");
            }       
        }
    }

    /// <summary>
    /// Signal to the game controller that you want the round to end
    /// </summary>
    public static void RequestEndRound(GameObject requestor)
    {
        instance.roundEndRequested = true;
        Debug.Log(requestor.name + " is requesting the round ends");
    }
    /// <summary>
    /// Signal to the game controller that you want the turn to end
    /// </summary>
    public static void RequestEndTurn(GameObject requestor)
    {
        instance.turnEndRequested = true;
        Debug.Log(requestor.name + " is requesting the turn ends");
    }
}

[System.Serializable]
public class Player
{
    public int playerNum;
    public List<Disc> discs;
}

[System.Serializable]
public class Lock
{
    public GameObject lockSource;
    public string lockMessage;

    public Lock(GameObject _lockSource, string _lockMessage)
    {
        lockSource = _lockSource;
        lockMessage = _lockMessage;
    }
}