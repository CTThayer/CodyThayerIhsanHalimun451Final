using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour
{

    public Transform LookAtPosition = null;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(LookAtPosition != null);

        transform.parent.transform.forward = LookAtPosition.localPosition - transform.parent.transform.localPosition;

    }
    Vector3 delta = Vector3.zero;
    Vector3 mouseDownPos = Vector3.zero;


    // Update is called once per frame
    void Update()
    {

        int travel = 0;
        int scrollSpeed = 3;
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            var d = Input.GetAxis("Mouse ScrollWheel");

            if (d > 0f && travel > -30)
            {
                travel = travel - scrollSpeed;
                transform.parent.transform.Translate(0, 0, 1 * scrollSpeed, Space.Self);
                transform.parent.transform.forward = LookAtPosition.localPosition - transform.parent.transform.localPosition;

            }
            else if (d < 0f && travel < 100)
            {
                travel = travel + scrollSpeed;
                transform.parent.transform.Translate(0, 0, -1 * scrollSpeed, Space.Self);
                transform.parent.transform.forward = LookAtPosition.localPosition - transform.parent.transform.localPosition;

            }


            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = Input.mousePosition;
                delta = Vector3.zero;
            }
            if (Input.GetMouseButton(0))
            {
                delta = mouseDownPos - Input.mousePosition;
                mouseDownPos = Input.mousePosition;
                ProcesssZoom(delta);
            }
        }


    }

    public void ProcesssZoom(Vector3 delta)
    {


        Vector3 v = LookAtPosition.localPosition - transform.parent.transform.localPosition;
        float dist = v.magnitude;
        transform.parent.transform.localPosition += delta.normalized * 3;
        transform.parent.transform.forward = LookAtPosition.localPosition - transform.parent.transform.localPosition;
        transform.parent.transform.localPosition = LookAtPosition.localPosition - (LookAtPosition.localPosition - transform.parent.transform.localPosition).normalized * dist;
        //Debug.Log(transform.parent.localRotation);

    }
}
