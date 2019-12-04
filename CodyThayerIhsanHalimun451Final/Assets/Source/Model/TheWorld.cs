using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour  {

    public SceneNode TheRoot;

    //
    //public delegate void ApplyWindToNode(SceneNode sn, Matrix4x4 m);
    //public delegate Matrix4x4 CalculateWindEffectAtNode(SceneNode sn);

    //public delegate SceneNode GetNode();

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);
    }

    /**************************** Traversals **********************************/

    // Leaves-to-Root Traversal
    //  - basically depth first search (recursive)
    //  - Node passed will be treated as the root (starting point)
    public void TraverseAll_LeafToRoot(SceneNode node)
    {
        SceneNode[] children = node.GetComponentsInChildren<SceneNode>();

        for (int n = 0; n < children.Length; ++n)
        {
            TraverseAll_LeafToRoot(children[n]);
            // TODO - add function to perform work here for last-to-first 
            //        leaf execution
        }
        return;
    }

    //Root-To-Leaf Traversal
    //  - basically pre-order traversal (recursive)
    //  - Node passed will be treated as the root (starting point)
    public void TraverseAll_RootToLeaf(SceneNode node)
    {
        SceneNode[] children = node.GetComponentsInChildren<SceneNode>();

        // TODO - add function to perform work at each node as this progresses
        //        from root towards leaves

        for (int n = 0; n < children.Length; ++n)
        {
            TraverseAll_LeafToRoot(children[n]);
        }
        return;
    }

    //Leaf-To-Root Traversal - Single Branch
    //  - basically pre-order traversal (recursive) of a single branch starting
    //    at the end of the branch.
    //  - Node passed will be treated as the leaf starting point
    public void TraverseSingleBranch_LeafToRoot(SceneNode node)
    {
        // TODO - add function to do work here

        SceneNode parent = node.transform.GetComponentInParent<SceneNode>();
        if (parent != null)
            TraverseSingleBranch_LeafToRoot(parent);
        else
            return;
    }

    //Root-To-Leaf Traversal - Single Branch
    //  - basically pre-order traversal (recursive) of a single branch starting
    //    at the root of the branch.
    //  - Node passed will be treated as the root starting point for the branch
    public void TraverseSingleBranch_RootToLeaf(SceneNode node)
    {
        // TODO - add function to do work here

        // Assumes first child in hierarchy is the continuation of this branch
        SceneNode child = node.transform.GetComponentInChildren<SceneNode>();
        if (child != null)
            TraverseSingleBranch_LeafToRoot(child);
        else
            return;
    }

    /*************************** END Traversals *******************************/


    /************************** Delegate Switch *******************************/
    // "Universal" switch function that execute a delegate function based on the
    // supplied int parameter. This is used with the traversal methods so that 
    // any number of different delegates can be called based on a supplied
    // parameter list
    void NodeAction(int delegateIndex, SceneNode s)
    {
        switch (delegateIndex)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;

        }
    }
}
