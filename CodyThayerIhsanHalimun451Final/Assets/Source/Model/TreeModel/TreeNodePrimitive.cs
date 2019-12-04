using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodePrimitive : MonoBehaviour
{
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public Matrix4x4 TRS_matrix;

    // TODO: Add code to move colliders with the primitives so that they are
    // selectable and can be interacted with using the mouse. Then re-route the
    // selection back to the TreeNode that owns this primitive.

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    /************************** SetTransformMatrix *************************//**
    *   Separated from LoadShaderMatrix() because this information needs to be
    *   run before the wind simulation and the result needs to be stored so that
    *   when the wind force is calculated/applied, the transform information is
    *   available. (This is similar to the behavior that the Unity Transform
    *   class provides but is still lacking in many areas)
    *   
    *       @param  nodeMatrix  Matrix4x4 reference that is passed down from the
    *                           parent TreeNode(s) that own this primitive
    *                             
    **************************************************************************/
    public void SetTransformMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        TRS_matrix = nodeMatrix * p * trs * invp;
    }

    /*************************** LoadShaderMatrix **************************//**
    *   Loads the TRS_matrix and MyColor that are stored in the instance 
    *   variables in to the shader when this method is called.
    *                              
    **************************************************************************/
    //New - Applies outside transform matrix M to the TRS_matrix before it is
    //      sent to the shader.
    public void LoadShaderMatrix(Matrix4x4 M)
    {
        Matrix4x4 updatedMatrix = TRS_matrix * M;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", updatedMatrix);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);

        // Moves Collider to match primitive
        Transform selectedCC = transform.GetChild(0);
        selectedCC.position = TRS_matrix.GetColumn(3);
        selectedCC.rotation = TRS_matrix.rotation;
        selectedCC.localScale = TRS_matrix.lossyScale;
    }
    //Original
    //public void LoadShaderMatrix()
    //{
    //    GetComponent<Renderer>().material.SetMatrix("MyXformMat", TRS_matrix);
    //    GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    //}


    public Vector3 GetNodeUpVector()
    {
        Vector3 A = TRS_matrix.MultiplyPoint(Vector3.zero);
        Vector3 B = TRS_matrix.MultiplyPoint(Vector3.up);
        Vector3 UP = B - A;
        return UP.normalized;
    }

    /************************** Wind System Support ***************************/
    /*                                                                        */

    /****************************** ApplyWind ******************************//**
    *   Public method used to apply wind forces to nodes primitives.
    *   NOTE: The system that applies the wind force updates is responsible for 
    *   getting the initial max rotation quaternion (using the other methods
    *   supplied here) then calculating the appropriate quaternion to pass to
    *   this method for each time step.
    *   NOTE: This method does NOT validate the supplied quaternion before 
    *   applying it.
    *   
    *       @param      Q       Quaternion for the rotation that should be
    *                           applied to the primitve
    *                             
    **************************************************************************/
    public void ApplyWind(Quaternion Q)
    {
        // TODO: TEST ALL OF THIS!!!
        Matrix4x4 WindMatrix = GetWindMatrix(Q);

        // TODO: The order of these might be incorrect.  
        //       Check to see if this should be:
        //           TRS_matrix = WindMatrix * TRS_matrix;
        //       instead of:
        TRS_matrix = TRS_matrix * WindMatrix;
    }

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
        Quaternion R = TRS_matrix.rotation;                                         // Get a quaternion from the xform matrix
        Quaternion origR = this.transform.rotation;                                 // Store original rotation
        this.transform.rotation = R;                                                // Set prim's xform rotation to R. Has to be rotation NOT localRotation because this is from the composite xform matrix
        Vector3 VplusW = this.transform.up + windVector;                            // Combine wind vector and prim's up (up after rotation)
        Quaternion Q = Quaternion.FromToRotation(transform.up, VplusW.normalized);  // Get rotation between tranformed prim's up and the normalized combined vector VPlusW
        Q = ClampWindRotation(Q, MAX_R);                                            // Clamps the rotation to be less than or equal to MAX_R degrees
        this.transform.rotation = origR;                                            // Reset primitive's xform so that other code still functions correctly
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
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(Vector3.zero, Q, Vector3.one);                // TODO: Does this need to be Vector3.one for T param??
        return p * trs * invp;
    }

    /*                                                                        */
    /************************ END Wind System Support *************************/

}
