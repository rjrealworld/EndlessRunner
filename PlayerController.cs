using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Touch initialTouch = new Touch();
    private float distance = 0;
    private bool hasSwiped = false;
    
    public bool jump = false;
    public bool slide = false;

    public Animator anim;

    public int laneNum = 2;
    public float horizVel = 0;
    public int zVel = 6;
    public float score = 0;
    public Text scoreText;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
        GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 0, zVel);

        if(Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        if(Input.GetMouseButtonUp(0))
        {
                //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        
                //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            
            //normalize the 2d vector
            currentSwipe.Normalize();
    
            //swipe upwards
            if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("up swipe");
                jump = true;
            } else {
                jump = false;
            }
            anim.SetBool("isJump", jump);

            if(jump == true)
            {
                GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 3.5f, zVel);
                StartCoroutine(stopJump());
            }
                
            //swipe down
            if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("down swipe");
                slide = true;
            } else {
                slide = false;
            }

            if(slide == true)
            {
                anim.SetBool("isSlide", slide);
                transform.Translate(0, 0, 0.1f);
                StartCoroutine(stopSlide());
            } else if(slide == false) {
                anim.SetBool("isSlide", slide);
            }
                
            //swipe left
            if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && (laneNum > 1) && (laneNum <= 3))
            {
                Debug.Log("left swipe");
                horizVel = -2;
                StartCoroutine(stopLaneChange());
                laneNum -= 1;
            }
            //swipe right
            if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && (laneNum >= 1) && (laneNum < 3))
            {
                Debug.Log("right swipe");
                horizVel = 2;
                StartCoroutine(stopLaneChange());
                laneNum += 1;
            }
        }

        scoreText.text = score.ToString();
        if(score > 15)
        {   
            zVel = 8;
            GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 0, zVel);
        }
        else if(score > 30)
        {
            zVel = 12;
            GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 0, zVel);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 0, zVel);
        }
    }
    
    IEnumerator stopLaneChange()
    {
        yield return new WaitForSeconds(.5f);
        horizVel = 0;
    }

    IEnumerator stopJump()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, -6.5f, zVel + 4);
        yield return new WaitForSeconds(.1f);
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, 0, zVel);
        anim.SetBool("isJump", false);
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, 0, zVel);
        anim.SetBool("isSlide", false);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "lethal")
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);
            score += 1f;
        }
    }

    public void MobSwipe()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if(t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x,t.position.y);
            }
            if(t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x,t.position.y);
                            
                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                
                //normalize the 2d vector
                currentSwipe.Normalize();
    
                //swipe upwards
                if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("up swipe");
                    jump = true;
                } else {
                    jump = false;
                }

                if(jump == true)
                {
                    anim.SetBool("isJump", jump);
                    GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 3.5f, zVel);
                    StartCoroutine(stopJump());
                } else if(jump == false) {
                    anim.SetBool("isJump", jump);
                }
                
                //swipe down
                if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                    slide = true;
                } else {
                    slide = false;
                }

                if(slide == true)
                {
                    anim.SetBool("isSlide", slide);
                    transform.Translate(0, 0, 0.1f);
                } else if(slide == false) {
                    anim.SetBool("isSlide", slide);
                }
                
                //swipe left
                if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                }
                //swipe right
                if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                }
            }
        }
    }

}
