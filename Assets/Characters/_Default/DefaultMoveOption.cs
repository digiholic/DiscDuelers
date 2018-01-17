using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMoveOption : LaunchOption {

    public override void OnSwipe(Disc target, SwipeData swipe)
    {
        Vector2 swipeVector = swipe.GetSwipe();
        if (swipeVector != Vector2.zero)
        {
            Debug.Log("MOVING");
            //target.motion = MotionType.MOVE;
            //target.moves -= 1;
            Vector3 dir = new Vector3(swipeVector.x, 0.0f, swipeVector.y);
            target.AddForce(dir);
            target.inMotion = true;
            CameraController.EnableMouseControl(); //We can move the map again
        }
    }
}
