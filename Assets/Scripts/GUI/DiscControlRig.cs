using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscControlRig : MonoBehaviour {
    public int playerNum;
    public DiscButton strikeButton;
    public DiscButton moveButtons;
    public Character chara;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (chara.ReadyForOption)
        {
            ShowChildren();
        }
        else
        {
            HideChildren();
        }
        //strikeButton.ChangeText(chara.moves.ToString());
        //strikeButton.ChangeText(chara.strikes.ToString());
    }
    
    public void MoveClicked()
    {
        chara.ExecuteOption(chara.moveOptions[0]);
    }

    public void StrikeClicked()
    {
        chara.ExecuteOption(chara.strikeOptions[0]);
    }

    void HideChildren()
    {
        strikeButton.gameObject.SetActive(false);
        moveButtons.gameObject.SetActive(false);
    }

    void ShowChildren()
    {
        strikeButton.gameObject.SetActive(true);
        moveButtons.gameObject.SetActive(true);
    }
}
