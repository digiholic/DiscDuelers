using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscControlRig : MonoBehaviour {
    public int playerNum;
    public Text attackCount;
    public Text moveCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.instance.activeDisc.ownerPlayer == playerNum)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
    }

    void OnGUI() {
        attackCount.text = GameController.instance.activeDisc.attacks.ToString();
        moveCount.text = GameController.instance.activeDisc.moves.ToString();
    }
}
