﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodePrimitive : MonoBehaviour
{
    public GameObject primCollider;

    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public Matrix4x4 TRS_matrix;

    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        TRS_matrix = nodeMatrix * p * trs * invp;
        GetComponent<Renderer>().material.SetMatrix("MyXformMat", TRS_matrix);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);

        // Transform Collider Object
        primCollider.transform.position = TRS_matrix.GetColumn(3);
        primCollider.transform.rotation = TRS_matrix.rotation;
        primCollider.transform.localScale = TRS_matrix.lossyScale;
    }

    public Vector3 GetNodeUpVector()
    {
        Vector3 A = TRS_matrix.MultiplyPoint(Vector3.zero);
        Vector3 B = TRS_matrix.MultiplyPoint(Vector3.up);
        Vector3 UP = B - A;
        return UP.normalized;
    }

}
