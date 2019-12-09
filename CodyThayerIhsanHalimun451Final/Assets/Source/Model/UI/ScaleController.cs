using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleController : MonoBehaviour
{
    public SliderWithEcho X, Y, Z;
    public Text ObjectName;

    private Transform mSelected;
    private Vector3 mPreviousSliderValues = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);

        SetToScaling(true);
    }

    //---------------------------------------------------------------------------------
    // Initialize slider bars to specific function
    void SetToScaling(bool v)
    {
        Vector3 s = ReadObjectXfrom();
        mPreviousSliderValues = s;
        X.InitSliderRange(0.1f, 20, s.x);
        Y.InitSliderRange(0.1f, 20, s.y);
        Z.InitSliderRange(0.1f, 20, s.z);
    }

    //---------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------
    // resopond to sldier bar value changes
    void XValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        // if not in rotation, next two lines of work would be wasted
        float dx = v - mPreviousSliderValues.x;
        mPreviousSliderValues.x = v;
        Quaternion q = Quaternion.AngleAxis(dx, Vector3.right);
        p.x = v;
        UISetObjectXform(ref p, ref q);
    }

    void YValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        // if not in rotation, next two lines of work would be wasted
        float dy = v - mPreviousSliderValues.y;
        mPreviousSliderValues.y = v;
        Quaternion q = Quaternion.AngleAxis(dy, Vector3.up);
        p.y = v;
        UISetObjectXform(ref p, ref q);
    }

    void ZValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        // if not in rotation, next two lines of work would be wasterd
        float dz = v - mPreviousSliderValues.z;
        mPreviousSliderValues.z = v;
        Quaternion q = Quaternion.AngleAxis(dz, Vector3.forward);
        p.z = v;
        UISetObjectXform(ref p, ref q);
    }
    //---------------------------------------------------------------------------------

    // new object selected
    public void SetSelectedObject(Transform xform)
    {
        mSelected = xform;
        mPreviousSliderValues = Vector3.zero;
        if (xform != null)
            ObjectName.text = "Selected:" + xform.name;
        else
            ObjectName.text = "Selected: none";
        ObjectSetUI();
    }

    public void ObjectSetUI()
    {
        Vector3 p = ReadObjectXfrom();
        X.SetSliderValue(p.x);  // do not need to call back for this comes from the object
        Y.SetSliderValue(p.y);
        Z.SetSliderValue(p.z);
    }

    private Vector3 ReadObjectXfrom()
    {
        Vector3 p;

        if (mSelected != null)
            p = mSelected.localScale;
        else
            p = Vector3.one;

        return p;
    }

    private void UISetObjectXform(ref Vector3 p, ref Quaternion q)
    {
        if (mSelected == null)
            return;

        mSelected.localScale = p;
        NodeRef nr = mSelected.GetComponent<NodeRef>();
        if (nr != null)
        {
            foreach (TreeNodePrimitive tnp in nr.treeNode.PrimitiveList)
            {
                tnp.transform.localScale = p;
            }
        }
    }

}
