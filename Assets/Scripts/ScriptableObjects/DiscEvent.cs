using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiscEvent : ScriptableObject
{
    public abstract void Execute(Disc target, object source);
}