using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindModel : MonoBehaviour
{
    public Vector3 WindVector;  // Direction and Magnitude of the wind
    private bool isRunning = false;     // bool for tracking if simulation is running

    public float Rc;            // Rigidity Coefficient for the whole tree
    public float GustInterval;  // Gust Interval: user specified Gust time length
    public float Variance;      // Influence coefficient of randomization
    private const float Pc = 0.613f;    // "Standard constant" for wind pressure
                                        //  calculations in Newtons/m^2

    // NOTE: Wind should be disabled (temporarily) whenever the wind vector is 
    // changed because the recalculation of the WindLoad and re-weighting of
    // wind force on the nodes will be an expensive operation.  Failing to 
    // disable it will likely cause strange issues and/or have performance
    // impacts.

    public bool GetSimulationStatus()
    {
        return isRunning;
    }

    public void StartWindSimulation(TreeNode tn)    // Initial node should always be the root
    {
        isRunning = true;
        StartWSHelper(tn);
    }

    // Iterate through nodes and save current XForms so that we can revert to "pre-wind state"
    private void StartWSHelper(TreeNode tn) 
    {
        tn.WSN.StoreOriginalRotation(tn.transform.localRotation);
        foreach (Transform child in tn.transform)
        {
            TreeNode cn = child.GetComponent<TreeNode>();
            if (cn != null)
            {
                StartWSHelper(tn);
            }
        }
    }

    public void StopWindSimulation(TreeNode tn)    // Initial node should always be the root
    {
        isRunning = false;
        StopWSHelper(tn);
    }

    // Iterate through nodes and reset to save "pre-wind state"
    private void StopWSHelper(TreeNode tn)
    {
        tn.transform.localRotation = tn.WSN.GetOriginalRotation();
        foreach (Transform child in tn.transform)
        {
            TreeNode cn = child.GetComponent<TreeNode>();
            if (cn != null)
            {
                StopWSHelper(tn);
            }
        }
    }

    //Gust Controller variables
    private float IntervalEnd = 0;
    private float IntervalMid = 0;  // Unnecessary?
    bool newInterval = true;

    public void WindUpdate(TreeNode tn)
    {
        GustUpdate(tn);
        foreach (Transform child in tn.transform)
        {
            TreeNode cn = child.GetComponent<TreeNode>();
            if (cn != null)
            {
                WindUpdate(cn);
            }
        }
    }

    // New Version
    public void GustUpdate(TreeNode tn)
    {
        if (newInterval)
        {
            InitializeInterval();
            float s = ApproximateStepsPerInterval();
            tn.WSN.InitilializeThetaPerStep(s);
            // TODO: Calculate Gust MAX_R for this gust interval - Using manually set debug values for now, calculate for final version
            newInterval = false;
        }
        else if (Time.time < IntervalMid)
        {
            // Rotate in "wind direction" 
            Quaternion Q = tn.WSN.GetStepRotation(WindVector, true);
            tn.transform.localRotation = Q;
            // TODO: Ideally add logic to handle if it hits Gust MAX_R, then rebound slightly
        }
        else if (Time.time >= IntervalMid && Time.time < IntervalEnd)
        {
            // Reverse rotation direction so that it goes back to initial
            Quaternion Q = tn.WSN.GetStepRotation(WindVector, false);
            tn.transform.localRotation = Q;
            // TODO: Ideally add logic to handle if it goes past original location
        }
        else if (Time.time >= IntervalEnd)
        {
            // TODO:  Do we need to ensure node has reset between intervals??

            // Start new interval
            newInterval = true;
        }
    }

    public void InitializeInterval()
    {
        // Uncomment to use variance
        //IntervalEnd = Time.time + GustInterval + (Random.Range(-1.0f, 1.0f) * Variance);

        IntervalEnd = Time.time + GustInterval;
        //Debug.Log("Current Time: " + Time.time);
        //Debug.Log("Current Gust Interval End: " + IntervalEnd);
        IntervalMid = IntervalEnd / 2;
    }

    public float ApproximateStepsPerInterval()
    {
        return  GustInterval / Time.deltaTime;
    }


}
