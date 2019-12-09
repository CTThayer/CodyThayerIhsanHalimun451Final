using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public Camera cam;
    //public GameObject manipulator_S;
    public GameObject manipulator_R;
    public float inputDampening;

    public ScaleController scaleController;

    GameObject Selection;
    GameObject ControlledObject;
    Vector3 LastMousePosition;

    void Start()
    {
        Debug.Assert(cam != null);
        //Debug.Assert(manipulator_S != null);
        Debug.Assert(manipulator_R != null);
        Debug.Assert(inputDampening > 0);
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            HandleMouseEvents();
        }
    }

    GameObject GetSelection()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    void HandleMouseEvents()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject NewSelection = GetSelection();

            LastMousePosition = Input.mousePosition;

            // If not null and not the same as previous selection
            if (NewSelection != null && NewSelection != Selection)
            {
                Selection = NewSelection;

                // If NewSelection is not a manipulator
                if (NewSelection.tag != "X-Manipulator"
                    && NewSelection.tag != "Y-Manipulator"
                    && NewSelection.tag != "Z-Manipulator")
                {
                    ControlledObject = NewSelection;
                    manipulator_R.transform.position = Selection.transform.position;
                    manipulator_R.transform.rotation = Selection.transform.rotation;
                    Debug.Log("ControlledObject: " + ControlledObject.name);
                }
            }
            // If NewSelection is Null reset selector location and clear selection
            else if (NewSelection == null)
            {
                manipulator_R.transform.position = new Vector3(0, 0, 10000);
                Selection = NewSelection;
                ControlledObject = NewSelection;
            }

            
            // Set ScaleController UI
            if (ControlledObject != null)
                scaleController.SetSelectedObject(ControlledObject.transform);
            else
                scaleController.SetSelectedObject(null);
        }
        if (Input.GetMouseButton(0) && Selection != null)
        {
            HandleRotation();
        }
    }

    void HandleRotation()
    {
        if (ControlledObject != null)
        {
            float d = GetDeltaDistance();
            float rotDelta = 0;
            Quaternion Q = Quaternion.identity;
            // If manipulator selected, calculate rotation
            if (Selection.tag == "X-Manipulator")
            {
                Vector3 mR = manipulator_R.transform.localRotation.eulerAngles;
                rotDelta = mR.x + (d * inputDampening);
                Q = Quaternion.Euler(rotDelta, mR.y, mR.z);
            }
            else if (Selection.tag == "Y-Manipulator")
            {
                Vector3 mR = manipulator_R.transform.localRotation.eulerAngles;
                rotDelta = mR.y + (d * inputDampening);
                Q = Quaternion.Euler(mR.x, rotDelta, mR.z);
            }
            else if (Selection.tag == "Z-Manipulator")
            {
                Vector3 mR = manipulator_R.transform.localRotation.eulerAngles;
                rotDelta = mR.z + (d * inputDampening);
                Q = Quaternion.Euler(mR.x, mR.y, rotDelta);
            }
            // If manipulator selected, assign rotation
            if (Selection.tag == "X-Manipulator" || Selection.tag == "Y-Manipulator" || Selection.tag == "Z-Manipulator")
            {
                manipulator_R.transform.localRotation = Q;
                ControlledObject.transform.localRotation = Q;
                NodeRef nr = ControlledObject.GetComponent<NodeRef>();
                if (nr != null)
                    nr.treeNode.transform.localRotation = Q;
            }
            LastMousePosition = Input.mousePosition;
        }
    }

    private float GetDeltaDistance()
    {
        Vector3 delta = Input.mousePosition - LastMousePosition;
        float d = delta.magnitude;
        float dir = (delta.x + delta.y) > 0 ? 1 : -1;
        return d *= dir;
    }

}
