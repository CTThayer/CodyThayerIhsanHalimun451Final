﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive : MonoBehaviour {
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public Matrix4x4 TRS_matrix;

	// Use this for initialization
	void Start ()
    {

    }

    void Update()
    {
    }
  
	public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        TRS_matrix = nodeMatrix * p * trs * invp;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", TRS_matrix);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);

        Transform selectedCC = transform.GetChild(0);
       // selectedCC.localScale = TRS_matrix.lossyScale;
        selectedCC.rotation = TRS_matrix.rotation;
        selectedCC.position = TRS_matrix.GetColumn(3);
    }

    public Vector3 GetNodePrimitiveScale()
    {
        return TRS_matrix.lossyScale;
    }
}