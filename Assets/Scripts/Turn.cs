using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Turn data object lets us know whose turn it is, and holds a delegate for discs to register themselves to.
/// </summary>
public class Turn : MonoBehaviour {
    public Player currentPlayer;
    public Disc activeDisc;

    public delegate void EndTurnEvent(Disc turn_ending_disc);
    public event EndTurnEvent OnTurnEnd;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndTurn()
    {
        //TODO do all the turn ending busywork here, changing players, etc.
        OnTurnEnd(activeDisc);
    }
}
