using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchingScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]public class Touching : UnityEvent { }

    public Touching touching;


    public TouchType touchType;

    Touch touch;

    #region UI_DOUBLE
    bool isClicked = false;
    public float doubleClickDelay = 0.3f;
    float touchDurationUI;
    float lastTouchTime = 0;
    #endregion

    #region SCREEN_DOUBLE
    float touchDuration;
    #endregion

    #region SCREEN_SWIPE
    public float swipeAmount = 50;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    #endregion

    #region SCREEN_HOLD
    public float pressTime = 0;
    private float pressedTime = 0;
    #endregion


    private void Awake()
    {
        Debug.Log(touchType.ToString());
    }
    private void Update()
    {
        /// TOUCHING TO SCREEN
        if (touchType.ToString().StartsWith("SCREEN"))
        {
            TouchScreen();
        }
        /// TOUCHING TO UI
        else
        {
            TouchUI();
        }
    }

    void TouchUI()
    {
        touchDurationUI += Time.deltaTime;
    }

    void TouchScreen()
    {
        // if there is any touching
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            touchDuration += Time.deltaTime;
            //making sure it only check the touch once && it was a short touch/tap and not a dragging.
            if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f)
                StartCoroutine("singleOrDouble");

            if (touchType == TouchType.SCREEN_SWIPE)
                swipeScreen();
            else if (touchType == TouchType.SCREEN_HOLD)
                holdScreen();

            

        }
        touchDuration = 0.0f;
    }

    private void holdScreen()
    {
        if (touch.phase == TouchPhase.Stationary)
        {
            pressedTime += Time.deltaTime;
            
            if(pressTime == 0)
            {
                transform.position += new Vector3(0f, 0.1f, 0f);
                holdingScreen();
            }

        }
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {

            if (pressTime != 0 && pressTime <= pressedTime)
            {
                holdForSeconds();
            }
            pressedTime = 0.0f;

        }

    }

    void swipeScreen()
    {
        if (touch.phase == TouchPhase.Began)
        {
            startSwipe = touch.position;
            Debug.Log("baþlama " + startSwipe);
        }
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            endSwipe = touch.position;
            Debug.Log("bitiþ " + endSwipe);

            if (startSwipe.x - endSwipe.x < -swipeAmount)
            {
                rightSwipe();
                transform.position += new Vector3(2f, 0f, 0f);
            }
            else if (startSwipe.x - endSwipe.x > swipeAmount)
            {
                leftSwipe();
                transform.position += new Vector3(-2f, 0f, 0f);
            }
            else if (startSwipe.y - endSwipe.y < -swipeAmount)
            {
                upSwipe();
                transform.position += new Vector3(0f, 2f, 0f);
            }
            else if (startSwipe.y - endSwipe.y > swipeAmount)
            {
                downSwipe();
                transform.position += new Vector3(0f, 8f, 0f);
            }


        }

    }

    IEnumerator singleOrDouble()
    {
        yield return new WaitForSeconds(0.3f);
        if (touch.tapCount == 1)
        {
            if (touchType == TouchType.SCREEN_SINGLE)
                singleTouch();
        }
        else if (touch.tapCount == 2)
        {
            if (touchType == TouchType.SCREEN_DOUBLE)
                doubleTouch();


            StopCoroutine("singleOrDouble");
        }
    }

    private void downSwipe()
    {
        Debug.Log("aþaðý swipe fonksiyonu");
    }

    private void upSwipe()
    {
        Debug.Log("yukarý swipe fonksiyonu");
        touching.Invoke();
    }

    private void leftSwipe()
    {
        Debug.Log("sola swipe fonksiyonu");
        touching.Invoke();
    }

    private void rightSwipe()
    {
        Debug.Log("saða swipe fonksiyonu");
        touching.Invoke();
    }

    void singleTouch()
    {
        Debug.Log("ekrana tek týklama fonksiyonu");
        touching.Invoke();
    }

    void doubleTouch()
    {
        Debug.Log("çift týklama fonksiyonu");
        touching.Invoke();

    }

    private void holdForSeconds()
    {
        Debug.Log(pressTime.ToString() + "saniye basýlý tutma fonksiyonu.");
        touching.Invoke();
    }

    void holdingScreen()
    {
        Debug.Log("basýlý tutarken çalýþan fonksiyon");
        touching.Invoke();
    }

    void doubleTouchUI()
    {
        Debug.Log("butona çift týklandý");
    }

    void singleTouchUI()
    {
        Debug.Log("butona tek týklandý");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isClicked)
            lastTouchTime = Time.time;
        isClicked = true;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(Time.time - lastTouchTime);
        if (Time.time-lastTouchTime <= doubleClickDelay)
        {
            doubleTouchUI();
            isClicked = false;
        }
        else
        {
            touchDurationUI = 0f;
        }
    }
    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        singleTouchUI();
    }*/

    public enum TouchType
    {
        SCREEN_SINGLE,
        SCREEN_DOUBLE,
        SCREEN_SWIPE,
        SCREEN_HOLD,
        UI_SINGLE,
        UI_DOUBLE,
        UI_HOLD,

    }
}
