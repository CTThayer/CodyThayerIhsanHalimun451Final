using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCamController : MonoBehaviour
{
    public Camera smallCam;
    public GameObject target;
    public float distance;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(smallCam != null);
        Debug.Assert(target != null);
        Debug.Assert(distance > 0);

        SetToTopView();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetToFrontView()
    {
        Quaternion Q = Quaternion.AngleAxis(angle, Vector3.right);
        Vector3 V = Q * Vector3.back;

        // Sets camera position
        smallCam.transform.position = target.transform.position + (V * distance);

        // Ensures camera looks at the target
        smallCam.transform.forward = (target.transform.position - smallCam.transform.position).normalized;
    }

    public void SetToBackView()
    {
        Quaternion Q = Quaternion.AngleAxis(angle, Vector3.left);
        Vector3 V = Q * Vector3.forward;

        // Sets camera position
        smallCam.transform.position = target.transform.position + (V * distance);

        // Ensures camera looks at the target
        smallCam.transform.forward = (target.transform.position - smallCam.transform.position).normalized;
    }

    public void SetToLeftView()
    {
        Quaternion Q = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 V = Q * Vector3.right;

        // Sets camera position
        smallCam.transform.position = target.transform.position + (V * distance);

        // Ensures camera looks at the target
        smallCam.transform.forward = (target.transform.position - smallCam.transform.position).normalized;
    }

    public void SetToRightView()
    {
        Quaternion Q = Quaternion.AngleAxis(angle, Vector3.back);
        Vector3 V = Q * Vector3.left;

        // Sets camera position
        smallCam.transform.position = target.transform.position + (V * distance);

        // Ensures camera looks at the target
        smallCam.transform.forward = (target.transform.position - smallCam.transform.position).normalized;
    }

    public void SetToTopView()
    {
        // Sets camera position
        smallCam.transform.position = target.transform.position + (Vector3.up * distance); // * 2.0f);

        // Ensures camera looks at the target
        smallCam.transform.forward = (target.transform.position - smallCam.transform.position).normalized;
    }

}
