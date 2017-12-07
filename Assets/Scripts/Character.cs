using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public Disc disc;

    public int life = 5;
    public int maxAttacks = 2;
    public int maxMoves = 2;

    private int attacks;
    private int moves;

	// Use this for initialization
	void Start () {
        disc.StartTurnEvent += disc.DefaultStartTurn;
        disc.EndRoundEvent += disc.DefaultEndRound;
        disc.EndTurnEvent += disc.DefaultEndTurn;
        disc.OnGetAttacked += disc.DefaultOnGetAttacked;
        disc.OnCrash += disc.DefaultOnCrash;
        disc.OnFall += disc.DefaultOnFall;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
