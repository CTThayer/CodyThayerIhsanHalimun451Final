using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindModel : MonoBehaviour
{
    public Vector3 WindVector;  // Direction and Magnitude of the wind
    private bool isRunning;     // bool for tracking if simulation is running

    public float Rc;            // Rigidity Coefficient for the whole tree
    public float GustInterval;  // Gust Interval: timeframe for "current gust"
    public float Variance;      // Influence coefficient of randomization
    private const float Pc = 0.613f;    // "Standard constant" for wind pressure
                                        //  calculations in Newtons/m^2


    public bool GetSimulationStatus()
    {
        return isRunning;
    }

    public void SetSimulationStatus(bool b)
    {
        isRunning = b;
    }

    public void ToggleSimulationStatus()
    {
        isRunning = !isRunning;
    }

    // TODO: Add code for turning wind simulation on/off
    // NOTE: Wind should be disabled (temporarily) whenever the wind vector is 
    // changed because the recalculation of the WindLoad and re-weighting of
    // wind force on the nodes will be an expensive operation.  Failing to 
    // disable it will likely cause strange issues and/or have performance
    // impacts.

    //Gust Controller variables
    private float IntervalEnd = 0;
    private float IntervalMid = 0;  // Unnecessary?

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

    public void GustUpdate(TreeNode tn)
    {
        if (Time.time >= IntervalEnd)
        {
            InitializeInterval();
            tn.WSM.InitializeStepping(WindVector, IntervalEnd);
        }
        else if (Time.time < IntervalMid)
        {
            Matrix4x4 WM = tn.WSM.Step(true, WindVector);
            foreach(TreeNodePrimitive tnp in tn.PrimitiveList)
            {
                tnp.LoadShaderMatrix(WM);
            }
        }
        else if (Time.time >= IntervalMid && Time.time < IntervalEnd)
        {
            Matrix4x4 WM = tn.WSM.Step(false, WindVector);
            foreach (TreeNodePrimitive tnp in tn.PrimitiveList)
            {
                tnp.LoadShaderMatrix(WM);
            }
        }
    }

    public void InitializeInterval()
    {
        IntervalEnd = Time.time + GustInterval + (Random.Range(-1.0f, 1.0f) * Variance);
        Debug.Log("Current Time: " + Time.time);
        Debug.Log("Current Gust Interval End: " + IntervalEnd);
        IntervalMid = IntervalEnd / 2;
    }


    /************************** GetWindLoadOnTree **************************//**
    * Calculates the wind load on the tree (and it's individual components) 
    * using the helper methods: CalculateProjectedArea() and CalculateWindLoad()
    * 
    *    @param    tree     is the tree to calculate the wind load on
    *    @param    wind     is the wind vector
    *    @param    dragC    is the user specified drag coefficient for the tree
    *    
    ***************************************************************************/
    public float GetWindLoadOnTree(TreeNode treeNode, Vector3 wind, float dragC)
    {
        float area = CalculateProjectedArea(treeNode, wind);
        return CalculateWindLoad(area, dragC, wind);
    }

    /*********************** CalculateProjectedArea ************************//**
    * Calculates the projection of the tree's geometry onto a plane that is
    * perpendicular to the wind vector. This projection is used to calculate the
    * force of the wind and to "weight" the effects of the wind on the 
    * individual parts of the tree.
    *    @param    tree     is the tree to calculate the wind load on
    *    @param    wind     is the wind vector
    *    
    ***************************************************************************/
    private float CalculateProjectedArea(TreeNode treeNode, Vector3 wind)
    {
        float area = 0;

        // TODO:    Calculate Projection Plane
        // TODO:    Verify Cross Product order is correct
        Vector3 pX = Vector3.Cross(wind, Vector3.up);


        // TODO:    Use leaf-to-root traversal to navigate through the tree. 
        //          CalculateProjected area at each node

        // TODO:    Implement helper method for calculating the projection of 
        //          individual geometry pieces onto the plane.

        return area;
    }

    /************************* CalculateWindLoad ***************************//**
     * Calculates wind load for the tree based on current wind settings
     * Uses the following simplified wind load model:
     *    F = A x P x Cd
     *      where:
     *    @param    F   is the force or wind load
     *    @param    A   is the projected area of the object (SA)
     *    @param    P   is the wind pressure
     *    @param    Cd  is the drag coefficient
     **************************************************************************/
    private float CalculateWindLoad(float A, float Cd, Vector3 wind)
    {
        float W = wind.magnitude;
        float P = Pc * W * W;
        return A * P * Cd;
    }


}
