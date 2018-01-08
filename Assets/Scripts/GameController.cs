using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public bool roundEndRequested= false;
    public bool turnEndRequested = false;

    public List<PlayerController> players;

    public List<GameObject> RoundLock = new List<GameObject>();
    public List<GameObject> TurnLock = new List<GameObject>();
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
    public static void LockRound(GameObject go)
    {
        instance.RoundLock.Add(go);
    }

    /// <summary>
    /// This method is called when an object that was previously requesting the round not end is now okay with it ending. This is called when whatever operation
    /// it was trying to do clears up.
    /// </summary>
    /// <param name="go">The game object that no longer requests the round stay open</param>
    public static void ReleaseRoundLock(GameObject go)
    {
        instance.RoundLock.Remove(go);
    }

    /// <summary>
    /// This method is called when an object is requesting that the turn doesn't end. For example, when playing an animation or still in motion.
    /// The object is added on to the list so that we can see what's holding up the turn order.
    /// </summary>
    /// <param name="go">The game object requesting the turn be held</param>
    public static void LockTurn(GameObject go)
    {
        instance.TurnLock.Add(go);
    }

    /// <summary>
    /// This method is called when an object that was previously requesting the turn not end is now okay with it ending. This is called when whatever operation
    /// it was trying to do clears up.
    /// </summary>
    /// <param name="go">The game object that no longer requests the turn stay open</param>
    public static void ReleaseTurnLock(GameObject go)
    {
        instance.TurnLock.Remove(go);
    }

    /// <summary>
    /// Signal to the game controller that you want the round to end
    /// </summary>
    public static void RequestEndRound()
    {
        instance.roundEndRequested = true;
    }

    /// <summary>
    /// Signal to the game controller that you want the turn to end
    /// </summary>
    public static void RequestEndTurn()
    {
        instance.turnEndRequested = true;
    }
}

[System.Serializable]
public class Player
{
    public int playerNum;
    public List<Disc> discs;
}