using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSimNode : MonoBehaviour
{
    public TreeNode treeNode;
    public TreeNodePrimitive primitive;

    private float StepRotation = 0;
    //private Vector3 RotationAxis;   // Unnecessary?

    void Start()
    {
        //May have to remove this if instantiation becomes a problem.
        Debug.Assert(treeNode != null);
        Debug.Assert(primitive != null);
    }

    public void InitializeStepping(Vector3 WindVector, float end)
    {
        //Quaternion intervalMaxR = treeNode.CalculateGustMaxima(WindVector);

        Quaternion intervalMaxR = GetMaxGustRotationOnNode(WindVector, treeNode.MAX_Rotation);
        float steps = (end - Time.time) / Time.deltaTime;
        float theta;
        Vector3 RotationAxis;
        intervalMaxR.ToAngleAxis(out theta, out RotationAxis);
        StepRotation = theta / steps;
    }

    public Matrix4x4 Step(bool forward, Vector3 WindVector)
    {
        Quaternion Q = CalculateStepRotation(forward, WindVector);
        return GetWindMatrix(Q);
    }

    //TODO: DOES THIS WORK???
    // WIP Code
    private Quaternion CalculateStepRotation(bool forward, Vector3 WindVector)
    {
        // Attempt 2 - Uses code from below (reformatted)

        // Store original rotation
        Quaternion origR = primitive.transform.rotation;

        // Get a quaternion from the xform matrix
        Quaternion R = primitive.TRS_matrix.rotation;

        // Set prim's xform rotation to R. Has to be rotation NOT localRotation 
        // because this is from the composite xform matrix
        primitive.transform.rotation = R;

        // Combine wind vector and prim's up (up after rotation)
        Vector3 VplusW = primitive.transform.up + WindVector;

        // Get rotation between tranformed prim's up and the normalized combined vector VPlusW
        Quaternion Q = Quaternion.FromToRotation(transform.up, VplusW.normalized);

        // Reset primitive's xform so that other code still functions correctly
        primitive.transform.rotation = origR;

        // Getting axis from wind vector rotation Q
        float t;
        Vector3 axis;
        Q.ToAngleAxis(out t, out axis);

        // Getting angle theta from current rotation R
        float theta;
        Vector3 a;
        R.ToAngleAxis(out theta, out a);

        // If forward, add step, else subtract step
        theta += forward ? StepRotation : -StepRotation;
        Q = Quaternion.AngleAxis(theta, axis);

        // Clamp and return
        Q = ClampWindRotation(Q, treeNode.MAX_Rotation);
        return Q;

    }

    private Vector3 GetWindRotationAxis(Vector3 WindVector)
    {
        Vector3 UP = primitive.GetNodeUpVector();           // Do we need to account for WindMatrix Transform here???
        
        // Cross Product to get the axis of rotation
        Vector3 Axis = Vector3.Cross(UP, WindVector);       // This might be backwards
        return Axis.normalized;
    }
    
    // Returns max rotation in degrees
    //private float GetMaxGustRotation(Vector3 WindVector, float intervalLength)
    //{
    //    // Combine UP direction and WindVector, then normalize to get resulting direction
    //    Vector3 resultDir = UP + WindVector;
    //    resultDir = resultDir.normalized;

    //    // Get rotation (quaternion) from current UP to resultDir
    //    Quaternion R = Quaternion.FromToRotation(UP, resultDir);
    //}

    /*********************** GetMaxGustRotationOnNode **********************//**
    *   Public method used calculate the maximum rotation that the supplied
    *   wind vector can cause. Uses ClampWindRotation() to ensure that the
    *   TreeNodePrimitive doesn't rotate more than the specified MAX_Rotation.
    *   
    *       @param     windVector  Vector3 representing the wind direction and
    *                              magnitude. This should be supplied by the 
    *                              TreeNode that owns this primitive.
    *       @param     MAX_R       float representing the maximum rotation this
    *                              node can apply. This is based on the wind
    *                              model's calculations and supplied by the
    *                              TreeNode that owns this primitive.
    *                             
    **************************************************************************/
    public Quaternion GetMaxGustRotationOnNode(Vector3 windVector, float MAX_R)
    {
        // TODO: TEST ALL OF THIS!!!
        Quaternion origR = primitive.transform.rotation;                            // Store original rotation
        Quaternion R = primitive.TRS_matrix.rotation;                               // Get a quaternion from the xform matrix
        primitive.transform.rotation = R;                                           // Set prim's xform rotation to R. Has to be rotation NOT localRotation because this is from the composite xform matrix
        Vector3 VplusW = primitive.transform.up + windVector;                       // Combine wind vector and prim's up (up after rotation)
        Quaternion Q = Quaternion.FromToRotation(transform.up, VplusW.normalized);  // Get rotation between tranformed prim's up and the normalized combined vector VPlusW
        Q = ClampWindRotation(Q, MAX_R);                                            // Clamps the rotation to be less than or equal to MAX_R degrees
        primitive.transform.rotation = origR;                                       // Reset primitive's xform so that other code still functions correctly
        return Q;
    }

    /************************** ClampWindRotation **************************//**
    *   Private utility method for clamping the rotation to a specified number
    *   of degrees. This relies on the ToAngleAxis and AngleAxis methods owned
    *   by Quaternion.
    *   
    *       @param      windVector  Vector3 representing the wind direction and 
    *                               magnitude. (Should be supplied by the 
    *                               TreeNode that owns this TreeeNodePrimitive)
    *       @param      MAX_R       float representing maximum angle in degrees
    *                               that this node/primitive can rotate.
    *                               (Should be supplied by the TreeNode that 
    *                               owns this TreeNodePrimitive)
    *                             
    **************************************************************************/
    // TODO: Should this clamp to negative MAX_R in addition to positive MAX_R?
    private Quaternion ClampWindRotation(Quaternion Q, float MAX_R)
    {
        float theta;
        Vector3 axis;
        Q.ToAngleAxis(out theta, out axis);
        if (theta <= MAX_R)
            return Q;
        else
            return Quaternion.AngleAxis(MAX_R, axis);
    }


    /**************************** GetWindMatrix ****************************//**
    *   Private utility method for setting up the matrix that applies the wind
    *   rotation. Calculates and returns the matrix for the wind rotation, 
    *   while also taking into account the pivot.
    *   Assumes that the supplied quaternion is within the correct range. 
    *   Does NOT check for clamping.
    *   
    *       @param      Q       Quaternion representing the correct rotation
    *                             
    **************************************************************************/
    private Matrix4x4 GetWindMatrix(Quaternion Q)
    {
        Matrix4x4 p = Matrix4x4.TRS(primitive.Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-primitive.Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(Vector3.zero, Q, Vector3.one);                // TODO: Does this need to be Vector3.one for T param??
        return p * trs * invp;
    }



}
