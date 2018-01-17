using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    #region Inspector Variables

    [Tooltip("Min swipe distance (inches) to register as swipe")]
    [SerializeField]
    float minSwipeLength = 0.5f;
    public static float swipeRatio = 1.0f;
    #endregion

    public delegate void OnSwipeDetectedHandler(SwipeData swipe);

    static OnSwipeDetectedHandler _OnSwipeDetected;
    public static event OnSwipeDetectedHandler OnSwipeDetected
    {
        add
        {
            _OnSwipeDetected += value;
        }
        remove
        {
            _OnSwipeDetected -= value;
        }
    }

    public static Vector2 swipeVelocity;
    
    static bool swipeEnded;
    static SwipeManager instance;

    public static SwipeData currentSwipe;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        DetectSwipe();
    }

    /// <summary>
    /// Attempts to detect the current swipe direction.
    /// Should be called over multiple frames in an Update-like loop.
    /// </summary>
    static void DetectSwipe()
    {
        if (GetTouchInput() || GetMouseInput())
        {
            // Swipe already ended, don't detect until a new swipe has begun
            if (swipeEnded)
            {
                return;
            }
            swipeEnded = true;

            if (_OnSwipeDetected != null)
            {
                _OnSwipeDetected(currentSwipe);
            }
        }
    }

    #region Helper Functions

    static bool GetTouchInput()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            // Swipe/Touch started
            if (t.phase == TouchPhase.Began)
            {
                currentSwipe = new SwipeData(t.position, Time.time);
                swipeEnded = false;
                // Swipe/Touch ended
            }
            else if (t.phase == TouchPhase.Ended)
            {
                currentSwipe.FinishSwipe(t.position, Time.time);
                return true;
                // Still swiping/touching
            }
        }

        return false;
    }

    static bool GetMouseInput()
    {
        // Swipe/Click started
        if (Input.GetMouseButtonDown(0))
        {
            currentSwipe = new SwipeData(Input.mousePosition, Time.time);
            swipeEnded = false;
            // Swipe/Click ended
        }
        else if (Input.GetMouseButtonUp(0))
        { 
            currentSwipe.FinishSwipe(Input.mousePosition, Time.time);
            return true;
            // Still swiping/clicking
        }
        return false;
    }
    #endregion
}

