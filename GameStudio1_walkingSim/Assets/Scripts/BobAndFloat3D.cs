using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BobAndFloat3D : MonoBehaviour
{

    public bool isFloatingInX;
    public bool startRightForX = true;
    public float floatWidthForX;
    public float floatDurationForX;

    public bool isFloatingInZ;
    public bool startBackwardsForZ = true;
    public float floatWidthForZ;
    public float floatDurationForZ;

    public bool isBobbing;
    public bool startUp = true;
    public float bobHeight;
    public float bobDuration;


    Vector3 startPos;
    Transform transform;

    bool firstFloat = true;
    bool firstBob = true;

    private bool rotate = false;
    private bool hasPerformedAction = false;
    
    public float degreesPerSecond = 5f;

    public bool startBobbingOnAwake = false;

    // Use this for initialization
    void Start()
    {
        transform = GetComponent<Transform>();
        startPos = transform.localPosition;

        if (isFloatingInX)
        {
            if (startRightForX)
            {
                FloatRight();
            }
            else
            {
                FloatLeft();
            }
        }
        else if (isBobbing)
        {
            if (startBobbingOnAwake)
            {
                if (startUp)
                {
                    BobUp();
                }
                else
                {
                    BobDown();
                }
            }
        }

        degreesPerSecond *= Mathf.Sign(Random.value - 0.5f);
        degreesPerSecond *= Random.Range(0.85f, 1.15f);
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate)
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
    }

    public void PerformUndeformAction()
    {
        if(hasPerformedAction)
            return;
        hasPerformedAction = true;
        StartBobbing();
    }

    public void StartBobbing()
    {
        
        if (startUp)
        {
            BobUp();
        }
        else
        {
            BobDown();
        }
    }

    public void StopMovement()
    {
        DOTween.Kill(gameObject);
    }


    public void BobUp()
    {
        rotate = true;
        Hashtable moveArgs = new Hashtable();

        float newYpos;
        if (firstBob)
        {
            newYpos = (startPos + Vector3.up * (bobHeight / 2)).y;
            firstBob = false;
        }
        else
        {
            newYpos = (transform.localPosition + Vector3.up * bobHeight).y;
        }

//        moveArgs.Add("position", );
//        moveArgs.Add("time", bobDuration);
//        moveArgs.Add("easetype", iTween.EaseType.easeInOutSine);
//        moveArgs.Add("oncomplete", "BobDown");
//        moveArgs.Add("islocal", true);
//
//        iTween.MoveTo(gameObject, moveArgs);
        
        transform.DOLocalMove(new Vector3(transform.localPosition.x, newYpos, transform.localPosition.z), bobDuration)
            .SetEase(Ease.InOutSine).OnComplete(BobDown);
    }

    public void BobDown()
    {
        rotate = true;
        Hashtable moveArgs = new Hashtable();

        float newYpos;
        if (firstBob)
        {
            newYpos = (startPos + Vector3.down * (bobHeight / 2)).y;
            firstBob = false;
        }
        else
        {
            newYpos = (transform.localPosition + Vector3.down * bobHeight).y;
        }

//        moveArgs.Add("position", new Vector3(transform.localPosition.x, newYpos, transform.localPosition.z));
//        moveArgs.Add("time", bobDuration);
//        moveArgs.Add("easetype", iTween.EaseType.easeInOutSine);
//        moveArgs.Add("oncomplete", "BobUp");
//        moveArgs.Add("islocal", true);
//
//        iTween.MoveTo(gameObject, moveArgs);
        
        transform.DOLocalMove(new Vector3(transform.localPosition.x, newYpos, transform.localPosition.z), bobDuration)
            .SetEase(Ease.InOutSine).OnComplete(BobUp);
    }

    public void FloatLeft()
    {
        Hashtable moveArgs = new Hashtable();

        float newXpos;
        if (firstFloat)
        {
            newXpos = (startPos + Vector3.left * (floatWidthForX / 2)).x;
            firstFloat = false;
        }
        else
        {
            newXpos = (transform.localPosition + Vector3.left * floatWidthForX).x;
        }

//        moveArgs.Add("position", new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z));
//        moveArgs.Add("time", floatDurationForX);
//        moveArgs.Add("easetype", iTween.EaseType.easeInOutSine);
//        moveArgs.Add("oncomplete", "FloatRight");
//        moveArgs.Add("islocal", true);
//
//        iTween.MoveTo(gameObject, moveArgs);
        
        transform.DOLocalMove(new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z), floatDurationForX)
            .SetEase(Ease.InOutSine).OnComplete(FloatRight);
    }

    public void FloatRight()
    {
        Hashtable moveArgs = new Hashtable();

        float newXpos;
        if (firstFloat)
        {
            newXpos = (startPos + Vector3.right * (floatWidthForX / 2)).x;
            firstFloat = false;
        }
        else
        {
            newXpos = (transform.localPosition + Vector3.right * floatWidthForX).x;
        }

//        moveArgs.Add("position", new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z));
//        moveArgs.Add("time", floatDurationForX);
//        moveArgs.Add("easetype", iTween.EaseType.easeInOutSine);
//        moveArgs.Add("oncomplete", "FloatLeft");
//        moveArgs.Add("islocal", true);
//
//        iTween.MoveTo(gameObject, moveArgs);
        
        transform.DOLocalMove(new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z), floatDurationForX)
            .SetEase(Ease.InOutSine).OnComplete(FloatLeft);
    }

//    public void FloatForward()
//    {
//        Hashtable moveArgs = new Hashtable();
//
//        float newXpos;
//        if (firstFloat)
//        {
//            newXpos = (startPos + Vector3.left * (floatWidthForZ / 2)).x;
//            firstFloat = false;
//        }
//        else
//        {
//            newXpos = (transform.localPosition + Vector3.left * floatWidthForZ).x;
//        }
//
//        moveArgs.Add("position", new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z));
//        moveArgs.Add("time", floatDurationForZ);
//        moveArgs.Add("easetype", iTween.EaseType.easeInOutSine);
//        moveArgs.Add("oncomplete", "FloatRight");
//        moveArgs.Add("islocal", true);
//
//        iTween.MoveTo(gameObject, moveArgs);
//        
//        transform.DOLocalMove(new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z), floatDurationForZ)
//            .SetEase(Ease.InOutSine).OnComplete(FloatBackwards);
//    }
//
//    public void FloatBackwards()
//    {
//        Hashtable moveArgs = new Hashtable();
//
//        float newXpos;
//        if (firstFloat)
//        {
//            newXpos = (startPos + Vector3.right * (floatWidthForZ / 2)).x;
//            firstFloat = false;
//        }
//        else
//        {
//            newXpos = (transform.localPosition + Vector3.right * floatWidthForZ).x;
//        }
//
//        moveArgs.Add("position", new Vector3(newXpos, transform.localPosition.y, transform.localPosition.z));
//        moveArgs.Add("time", floatDurationForZ);
//        moveArgs.Add("easetype", iTween.EaseType.easeInOutSine);
//        moveArgs.Add("oncomplete", "FloatLeft");
//        moveArgs.Add("islocal", true);
//
//        iTween.MoveTo(gameObject, moveArgs);
//    }


    void UpdateXPos(float xpos)
    {

    }

    void UpdateYPos(float xpos)
    {

    }
}
