using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindModel_P : MonoBehaviour
{
    public Vector3 WindVector;
    
    // Gust Interval: user specified Gust time length
    public float GustInterval;

    public WindNode_P Root;

    //Gust Controller variables
    private float IntervalEnd;
    private float IntervalMid;  // Unnecessary?
    bool newInterval;
    private int steps;

    // Start is called before the first frame update
    void Start()
    {
        IntervalEnd = 0;
        IntervalMid = 0;
        newInterval = true;
    }

    // Update is called once per frame
    void Update()
    {
        GustUpdate(Root);
    }

    // New Version
    public void GustUpdate(WindNode_P n)
    {
        if (newInterval == true)
        {
            InitializeInterval();
            int s = ApproximateStepsPerInterval();
            SetInitialsHelper(Root, s);
            newInterval = false;
        }
        else if (Time.time < IntervalMid)
        {
            // Rotate in "wind direction" 
            RunWindSim(Root, true);
            // TODO: Ideally add logic to handle if it hits Gust MAX_R, then rebound slightly
        }
        else if (Time.time >= IntervalMid && Time.time < IntervalEnd)
        {
            // Reverse rotation direction so that it goes back to initial
            RunWindSim(Root, false);
            // TODO: Ideally add logic to handle if it goes past original location
        }
        else if (Time.time >= IntervalEnd)
        {
            // TODO:  Do we need to ensure node has reset between intervals??

            // Start new interval
            newInterval = true;
        }
    }


    private void SetInitialsHelper(WindNode_P Wp, int s)
    {
        Wp.SetOriginalRotation(Wp.transform.rotation);
        Wp.SetStepTheta(s);
        foreach (Transform child in Wp.transform)
        {
            WindNode_P p = child.GetComponent<WindNode_P>();
            if (p != null)
            {
                SetInitialsHelper(p, s);
            }
        }
    }



    private void RunWindSim(WindNode_P Wp, bool forward)
    {
        Quaternion Q = forward ? RunWindSimHelper(Wp) : RunWindSimReverseHelper(Wp);
        Wp.transform.rotation = Q;
        foreach (Transform child in Wp.transform)
        {
            WindNode_P p = child.GetComponent<WindNode_P> ();
            if (p != null)
            {
                RunWindSim(p, forward);
            }
        }
    }

    private Quaternion RunWindSimHelper(WindNode_P W)
    {
        float t = W.GetStepAngle();
        float Theta = W.ForwardStep(t);
        Vector3 Axis = W.GetAxis(WindVector);
        Vector3 Direction = W.GetNodePlusWindDirection(WindVector);
        Quaternion R = Quaternion.AngleAxis(Theta, Axis);

        return R;
    }

    private Quaternion RunWindSimReverseHelper(WindNode_P W)
    {
        float t = W.GetStepAngle();
        float Theta = W.BackwardStep(t);
        Vector3 Axis = W.GetReverseAxis();
        Vector3 Direction = W.GetNodePlusWindDirection(WindVector);
        Quaternion R = Quaternion.AngleAxis(Theta, Axis);

        return R;
    }




    public void InitializeInterval()
    {
        // Uncomment to use variance
        //IntervalEnd = Time.time + GustInterval + (Random.Range(-1.0f, 1.0f) * Variance);

        IntervalEnd = Time.time + GustInterval;
        IntervalMid = IntervalEnd / 2;
    }

    public int ApproximateStepsPerInterval()
    {
        return (int)(GustInterval / Time.deltaTime);
    }




}
