using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStartTurnEvent", menuName = "DiscEvents/_Default/DefaultStartTurnEvent")]
public class DefaultStartTurnEvent : DiscEvent {

    public override void Execute(Disc target, object TurnDisc)
    {
        if (TurnDisc == target.gameObject)
        {
            Debug.Log("Taking my turn now");
            target.SendMessage("RefreshMotions");
        }
    }
}
