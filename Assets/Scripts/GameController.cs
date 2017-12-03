using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public List<Player> players;
    public int currentPlayerTurn;

    public Disc activeDisc;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void EndTurn()
    {

    }

    public void MoveButtonPressed()
    {
        activeDisc.SendMessage("Move");
    }

    public void AttackButtonPressed()
    {
        activeDisc.SendMessage("Attack");
    }
}

[System.Serializable]
public class Player
{
    public int playerNum;
    public List<Disc> discs;
}