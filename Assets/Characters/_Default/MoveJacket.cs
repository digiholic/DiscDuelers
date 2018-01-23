using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MoveJacket", menuName = "Jackets/MoveJacket")]
public class MoveJacket : Jacket {

    public override void OnCollideDisc(Disc source, GameObject other, Collision col)
    {
        AttackData data = other.GetComponent<Disc>().Attack(source,col);
        source.GetAttacked(data);
    }

    public override void OnCollideTerrain(Disc source, GameObject other, Collision col)
    {
        throw new NotImplementedException();
    }
}
