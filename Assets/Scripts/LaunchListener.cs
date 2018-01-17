using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaunchListener : MonoBehaviour {
    public Disc disc;
    public Vector3 clickStartPos;
    public DiscSwipeEvent testEvent;

    private SwipeData currentSwipe;

    void Awake()
    {
        if (testEvent == null)
            testEvent = new DiscSwipeEvent();
    }

    // Use this for initialization
    void Start () {
        disc = GetComponent<Disc>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0))
        {
            currentSwipe = new SwipeData(Input.mousePosition, Time.time);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentSwipe.FinishSwipe(Input.mousePosition, Time.time);
            testEvent.Invoke(currentSwipe);

        }
    }
}

[System.Serializable]
public class DiscSwipeEvent : UnityEvent<SwipeData> {

}

public class SwipeData
{
    public static float minSwipeLength = 5f;
    public static float maxSwipeLength = 100f;

    public Vector2 swipeStartPos;
    public Vector2 swipeEndPos;
    public float startTime;
    public float endTime;
    public bool done = false;

    public SwipeData(Vector2 startPos, float time)
    {
        swipeStartPos = startPos;
        startTime = time;
    }

    public void FinishSwipe(Vector2 endPos, float time)
    {
        swipeEndPos = endPos;
        endTime = time;
        done = true;
    }

    public Vector2 GetSwipe()
    {
        Vector2 swipe = (swipeEndPos - swipeStartPos) / 30;
        float time = (endTime - startTime);
        Vector2 result = swipe / time;
        if (result.magnitude < minSwipeLength) result = Vector2.zero; //If we're too small, make it zero
        if (result.magnitude > maxSwipeLength) result = result.normalized * maxSwipeLength; //If we're too big, make it maximum
        result *= SwipeManager.swipeRatio;
        return result;
    }

    public Vector2 GetSwipeNormalized()
    {
        return GetSwipe().normalized;
    }
}