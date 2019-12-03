using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    protected Matrix4x4 mCombinedParentXform;

    public Vector3 NodeOrigin = Vector3.zero;
    public List<TreeNodePrimitive> PrimitiveList;

    /**************************** Wind Sim Support *************************//**
     * Instance variables that are initialized and updated by the wind model 
     * based on WindModel calculations and parameters.
     * 
     **************************************************************************/
    // TODO: These are set to public for in-Unity testing purposes. We may want 
    // to set them back to private once everything is complete.
    public WindSimNode WSM;
    public float Rn;               // Node rigidity
    public float Wn;               // Wind load at this node
    public float MAX_Rotation;     // Maximum amount this node can rotate in Deg

    private Quaternion GustMaximum; // Stores the maximum rotation for this gust interval


    // TODO: Add code to support selection based on primitive(s) collider(s)

    // Use this for initialization
    protected void Start()
    {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
    }
    

    private void InitializeSceneNode()
    {
        // TODO: why is this not combined with Start() ???
        mCombinedParentXform = Matrix4x4.identity;
    }


    public void AddSceneNode()
    {
        // TODO: Likely needs logic to control how the node is added 
        // (i.e. add node inline on branch OR create new branch off of node)

        GameObject NewSN = new GameObject();
        Instantiate(NewSN);
        NewSN.AddComponent<SceneNode>();
        NewSN.transform.parent = gameObject.transform;
    }


    /***************************** Xform Matrix *******************************/
    /*                                                                        */

    /**************************** CompositeXform ***************************//**
     *   Called by World (or maybe Tree) in UPDATE to pass transform info before
     *   wind forces are calculated and applied. This is so that primitives are
     *   translated, scaled, and rotated before the wind calculation is made.
     *   
     *       @param parentXform    The Matrix4x4 for the transform passed down 
     *                             from the parent. (Concatenated from all
     *                             preceding parent nodes)
     *                             
     **************************************************************************/
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXform = parentXform * orgT * trs;

        // propagate to children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // disenminate to primitives
        foreach (TreeNodePrimitive p in PrimitiveList)
        {
            p.SetTransformMatrix(ref mCombinedParentXform);
            //LoadShaderMatrix() is now called after Wind Calculations
        }
    }

    /*                                                                        */
    /*************************** END Xform Matrix *****************************/



    /**************************** Wind Simulation *****************************/
    /*                                                                        */

    // TODO: Need a method for applying the Wind Model formula at this node

    // TODO: Should this go in WindModel and TreeNode just calls it by passing
    // the current node to the WindModel??

    /************************* CalculateWindRotation ***********************//**
     *   Calculates the maximum effect of the supplied wind vector at this node.
     *   This should be run at the BEGINNING of each gust interval.
     *   
     *       @param     windVector      Vector3 representing the wind direction
     *                                  and wind strength (magnitude of vector).
     *                             
     **************************************************************************/
    public Quaternion CalculateGustMaxima(Vector3 windVector)
    {
        GustMaximum = this.PrimitiveList[0].GetMaxGustRotationOnNode(windVector, MAX_Rotation);
        return GustMaximum;
    }
    
    /*                                                                        */
    /************************** END Wind Simulation ***************************/





    /************************ Tree Node Configuration *************************/
    /*                                                                        */

    // TODO: We might need a method for creating and maintaining line segment 
    // primitives that connect nodes. 

    /***************************** SetNodeRigidity *************************//**
     * Calculates and stores the rigidity at this node
     * 
     *      @param  d   distance from root
     *      @param  n   number of nodes from the root node to this node
     *      @param  Rc  rigidity coefficient for the whole tree
     *        
     **************************************************************************/
    public void SetNodeRigidity(float d, float n, float Rc)
    {
        // Preliminary formula
        // TODO: Adjust duning tuning and add improved clamping if needed
        float invD = 1 / d;
        float Bn = Mathf.Pow(n, (GetBranchThickness() - n));    // n^(t - n)
        Rn = invD * Bn * Rc;
        ClampNodeRigidity();
    }

    /************************** SetWindLoadAtNode **************************//**
     * Sets the value of Wn. 
     * Should generally only be used by the wind system to establish a wind load 
     * value based on the current wind vector, the tree's shape, and the way the 
     * wind hits the tree. This is all calculated by the wind system.
     *        
     **************************************************************************/
    public void SetWindLoadAtNode(float windLoad)
    {
        Wn = windLoad;
    }

    /**************************** SetMaxRotation ***************************//**
     * Sets the value of MAX_Rotation. 
     * Should generally only be used by the wind system to set a reasonable 
     * range of rotation based on node depth.
     *        
     ***************************************************************************/
    public void SetMaxRotation(float max)
    {
        MAX_Rotation = max;
    }

    /*************************** GetBranchThickness ************************//**
     * Gets an approximation of branch thickness at this node based on the 
     * average of the node's x-scale and y-scale
     *        
     **************************************************************************/
    public float GetBranchThickness()
    {
        Vector3 v = this.PrimitiveList[0].TRS_matrix.lossyScale;
        return (v.x + v.z) / 2;
    }

    /*************************** ClampNodeRigidity *************************//**
     * Clamps the value of Rn (Node Rigidity) to a range of 0 to 1 so that other
     * calculations that rely on it can safely assume its range.
     *        
     **************************************************************************/
    private void ClampNodeRigidity()
    {
        // TODO: Create improved method of clamping(ideally with some sort of
        // linear interpolation or scaling based on range of values.
        if (Rn > 1f)
            Rn = 1f;
        if (Rn < 0)
            Rn = 0f;
    }

    /*                                                                        */
    /********************** END Tree Node Configuration ***********************/

}
