using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public static bool RequestEndRound = false;
    public static bool RequestEndTurn = false;

    public List<Player> players;
    public List<GameObject> RoundLock = new List<GameObject>();
    public List<GameObject> TurnLock = new List<GameObject>();

    public Disc activeDisc;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	    if (RequestEndRound && (RoundLock.Count == 0))
        {
            RequestEndRound = false;
            //Fire end round message
        }
        if (RequestEndTurn && (TurnLock.Count == 0))
        {
            RequestEndTurn = false;
            //Fire end turn message
        }
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
}

[System.Serializable]
public class Player
{
    public int playerNum;
    public List<Disc> discs;
}