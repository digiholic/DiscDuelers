using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public List<Disc> discs;
    public Queue<Disc> turnOrder = new Queue<Disc>();
    public Disc activeDisc;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        foreach (Disc d in discs)
            turnOrder.Enqueue(d);
        activeDisc = turnOrder.Dequeue();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndTurn()
    {
        Disc oldDisc = activeDisc;
        turnOrder.Enqueue(oldDisc); //Put this disc into the back of the turn order
        activeDisc = turnOrder.Dequeue();
        activeDisc.SendMessage("StartTurn");
    }

    public void MoveButtonPressed()
    {
        activeDisc.SendMessage("Move");
    }

    public void AttackButtonPressed()
    {
        activeDisc.SendMessage("Attack");
    }

    public static void BroadcastEndRound()
    {
        instance.BroadcastMessage("EndRound");
    }

    public static void BroadcastEndTurn()
    { 
        instance.BroadcastMessage("EndTurn");
    }
}

[System.Serializable]
public class Player
{
    public int playerNum;
    public List<Disc> discs;
}