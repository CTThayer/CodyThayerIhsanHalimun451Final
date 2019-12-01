using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodePrimitive : MonoBehaviour
{
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public Matrix4x4 TRS_matrix;

    // TODO: Add code to move colliders with the primitives to that they are
    // selectable and can be interacted with using the mouse. Then re-route the
    // selection back to the TreeNode that owns this primitive.

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    public void SetTransformMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        TRS_matrix = nodeMatrix * p * trs * invp;
    }

    public void ApplyWindMatrix(Matrix4x4 WindMatrix)
    {
        // TODO: The order of these might be incorrect. Check to see if this 
        // should be:
        //      TRS_matrix = WindMatrix * TRS_matrix;
        // instead of:
        TRS_matrix = TRS_matrix * WindMatrix;
    }

    public void LoadShaderMatrix()
    {
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", TRS_matrix);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    }

    public Vector3 GetNodePrimitiveScale()
    {
        return TRS_matrix.lossyScale;
    }
}
