using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {

    protected Matrix4x4 mCombinedParentXform;
    
    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;


	// Use this for initialization
	protected void Start () {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
	}


    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    public void AddSceneNode()
    {
        GameObject NewSN = new GameObject();
        Instantiate(NewSN);
        NewSN.AddComponent<SceneNode>();
        NewSN.transform.parent = gameObject.transform;
    }

    public void DeleteSceneNode(GameObject g)
    {
        foreach (Transform child in g.transform)
        {
            if (child.GetComponent<SceneNode>() != null)
            {
                child.GetComponent<SceneNode>().NodeOrigin = g.GetComponent<SceneNode>().NodeOrigin;
                child.transform.parent = g.transform.parent;
            }
        }
        Destroy(g);
    }

    // This must be called _BEFORE_ each draw!! 
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        
        mCombinedParentXform = parentXform * orgT * trs;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }
        
        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }

    }
}