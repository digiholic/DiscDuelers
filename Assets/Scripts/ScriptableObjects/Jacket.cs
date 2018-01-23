using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Jacket Class represents the behavior a disc will have when colliding with things.
/// For example, when moving, a disc will "put on a jacket" that protects them from terrain collisions,
/// while some characters might have custom jackets, like Karin inflicting a Jacket on the opponent that makes them
/// take extra damage from enemy discs.
/// </summary>
public abstract class Jacket : ScriptableObject {
    //If two jackets are placed on a fighter that have the same triggers, the one with higher priority takes effect.
    //If two jackets don't conflict with each other (like if one reduces terrain damage, and another only affects disc collisions), they will both fire.
    public int jacketPriority = 0;

    public abstract void OnCollideDisc(Disc source, GameObject other, Collision col);

    public abstract void OnCollideTerrain(Disc source, GameObject other, Collision col);
}
