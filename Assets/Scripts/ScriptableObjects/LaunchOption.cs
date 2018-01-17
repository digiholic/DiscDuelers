using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaunchOption : ScriptableObject {

    public Sprite icon;
    public int strikeCost;
    public int moveCost;

    public abstract void OnSwipe(Disc target, SwipeData swipe);
}
