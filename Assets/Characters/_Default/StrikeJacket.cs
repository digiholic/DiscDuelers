using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StrikeJacket", menuName = "Jackets/StrikeJacket")]
public class StrikeJacket : Jacket {

    public override void OnCollideDisc(Disc source, GameObject other, Collision col)
    {
        Disc target = other.GetComponent<Disc>();
        AttackData data = source.Attack(target, col);
        target.GetAttacked(data);
    }

    public override void OnCollideTerrain(Disc source, GameObject other, Collision col)
    {
        throw new NotImplementedException();
    }
}
