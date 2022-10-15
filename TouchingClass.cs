using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchingClass : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    Touch touch;
    public Camera mainCamera;

    public float pressTime = 0;
    public float endPressTime = 0;

    //for double click
    float touchDuration;
    bool isSmall = false;

    //for swipe
    public Vector2 startSwipe;
    public Vector2 endSwipe;
    public float swipedPixel = 0.01f;
    bool right = false;
    bool left = false;
    bool up=false;
    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    private Vector3 position;
    private float width;
    private float height;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2"));
    }

    void Update()
    {
        //Touching a game object
        


        // Handle screen touches.
        if (Input.touchCount > 0 )
        {
            touch = Input.GetTouch(0);

            //for double tap
            touchDuration += Time.deltaTime;
            if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
                StartCoroutine("singleOrDouble");
/*
            /// TOUCHING A GAME OBJECT
            RaycastHit touchedObject;

            if(Physics.Raycast(mainCamera.ScreenPointToRay(touch.position), out touchedObject))
            {
                if(touchedObject.collider.gameObject.name == "Cube")
                {
                    Debug.Log("küpe dokundun.");
                }
            }
            /// this is working but onmousedown,up,drag much efficient. this functions must writed in the game object script.
            
*/

            /*

            /// SWIPE
            
            Debug.Log("swiped:" + touch.deltaPosition);
            
            if(touch.deltaPosition.x > swipedPixel) //right swipe
            {
                right = true;
                
            }
            else if (touch.deltaPosition.x < -swipedPixel) //left swipe
            {
                left = true;
                
            }
            else if (touch.deltaPosition.y > swipedPixel) //up swipe
            {
                up = true;
                
            }


            /*

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 moved = touch.deltaPosition;
                Vector3 newPosition = transform.position + new Vector3(moved.x * 0.01f, moved.y * 0.01f, 0f);

                if(newPosition.y >= 0.5)
                {
                    transform.position = newPosition;
                }


                /* Vector2 pos = touch.position;
                    pos.x = (pos.x - width) / width;
                    pos.y = (pos.y - height) / height;
                    position = new Vector3(pos.x, pos.y, 0.0f);

                    // Position the cube.
                    transform.position = position; 


            }
          

            if (Input.touchCount == 2) // iki parmakla dokunulduðunda
            {
                touch = Input.GetTouch(1); // iki parmakla dokunulduðunda

                if (touch.phase == TouchPhase.Began)
                {
                    pressTime = 0;
                    // Halve the size of the cube.
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Restore the regular size of the cube.
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }

            

            if (touch.phase == TouchPhase.Stationary)
            {
                pressTime += Time.deltaTime;
                transform.position += new Vector3(0f, 0.1f, 0f);
                Debug.Log("basýlý tutulýuyor");
            }
*/
            if (touch.phase == TouchPhase.Began)
            {
                startSwipe = touch.position;
                Debug.Log("baþlama " + startSwipe);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase ==TouchPhase.Canceled)
            {
                endSwipe = touch.position;
                Debug.Log("bitiþ " + endSwipe);

                if (startSwipe.x - endSwipe.x < -50)
                    transform.position += new Vector3(2f, 0f, 0f);
                else if (startSwipe.x - endSwipe.x > 50)
                    transform.position += new Vector3(-2f, 0f, 0f);
                else if (startSwipe.y - endSwipe.y < -50)
                    transform.position += new Vector3(0f, 2f, 0f);

                endPressTime = Time.time - pressTime;

                if (endPressTime < 0.5f)
                {
                    //Do something;

                    
                }
                endPressTime = 0;
                right = false;
                left = false;
                up = false;
            }

            

        }
        else
            touchDuration = 0.0f;
    }


    IEnumerator singleOrDouble()
    {
        yield return new WaitForSeconds(0.3f);
        if (touch.tapCount == 1)
            Debug.Log("Single");
        else if (touch.tapCount == 2)
        {
            if (isSmall)
            {
                isSmall=false;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }

            else
            {
                isSmall= true;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
             
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");
            Debug.Log("Double");
        }
    }


  

}
