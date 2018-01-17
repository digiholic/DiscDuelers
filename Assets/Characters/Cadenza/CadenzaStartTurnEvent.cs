using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CadenzaEvent",menuName ="DiscEvents/Cadenza/CadenzaEvent")]
public class CadenzaStartTurnEvent : DiscEvent {

    public override void Execute(Disc target, object source)
    {
        Debug.Log("Cadenza Online!");
    }
}
