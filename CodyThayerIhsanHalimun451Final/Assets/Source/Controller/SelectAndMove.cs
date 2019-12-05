using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndMove : MonoBehaviour
{

    public Camera cam;
    public GameObject manipulator;


    public float translateSpeed;

    GameObject SelectedObject;
    GameObject ControlledObject;
    Vector3 LastMousePosition;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            HandleMouseEvents();
        }
        if (ControlledObject != null)
        {
            Debug.Log(ControlledObject.transform.parent.parent.name);
            ControlledObject.transform.parent.parent.localRotation = manipulator.transform.localRotation;
        }
    }

        GameObject GetSelection()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit!");
            return hit.collider.gameObject;
        }
        else
        {
            Debug.Log("No Hit!");
            return null;
        }
    }

    void HandleMouseEvents()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click!");

            GameObject NewSelection = GetSelection();
            // New Object Selected
            if (NewSelection != null
                && NewSelection != SelectedObject
                && NewSelection.transform.parent != manipulator
                && NewSelection.tag == "mController")
            {
                SelectedObject = NewSelection;
                manipulator.transform.position = SelectedObject.transform.position;
               // manipulator.transform.rotation = SelectedObject.transform.rotation;
            }
            //if (CurrentSelection.transform.parent == manipulator)

            if(NewSelection.tag == "X-Manipulator"
               || NewSelection.tag == "Y-Manipulator"
               || NewSelection.tag == "Z-Manipulator")
            {
                // If last selection was NOT a manipulator
                if (SelectedObject.tag != "X-Manipulator"
                   && SelectedObject.tag != "Y-Manipulator"
                   && SelectedObject.tag != "Z-Manipulator")
                {
                    ControlledObject = SelectedObject;  // Store previous selection in 
                    SelectedObject = NewSelection;
                    LastMousePosition = Input.mousePosition;
                }
                else  // If last selection WAS a maipulator only change selection
                {
                    SelectedObject = NewSelection;
                    LastMousePosition = Input.mousePosition;
                }
            }
        }
        if (Input.GetMouseButton(0) && SelectedObject != null)
        {
            float rotDelta = 0;

            if (SelectedObject.tag == "X-Manipulator")
            {
                Vector3 delta = Input.mousePosition - LastMousePosition;

                rotDelta = Quaternion.ToEulerAngles(manipulator.transform.localRotation).x;
                rotDelta += delta.x*.02f;
                manipulator.transform.localRotation = Quaternion.AngleAxis(rotDelta, manipulator.transform.right);
              //  ControlledObject.transform.parent.localRotation *= manipulator.transform.localRotation;

                LastMousePosition = Input.mousePosition;

            }
            else if (SelectedObject.tag == "Y-Manipulator")
            {
                Vector3 delta = Input.mousePosition - LastMousePosition;

                rotDelta = Quaternion.ToEulerAngles(manipulator.transform.localRotation).y;
                rotDelta += delta.y * .02f;
                manipulator.transform.localRotation = Quaternion.AngleAxis(rotDelta, manipulator.transform.up);
               // ControlledObject.transform.parent.localRotation *= manipulator.transform.localRotation;
                Debug.Log(SelectedObject.transform.parent.name);
                LastMousePosition = Input.mousePosition;

            }
            else if (SelectedObject.tag == "Z-Manipulator")
            {
                Vector3 delta = Input.mousePosition - LastMousePosition;

                rotDelta = Quaternion.ToEulerAngles(manipulator.transform.localRotation).z;
                rotDelta += delta.x * .02f;
                manipulator.transform.localRotation = Quaternion.AngleAxis(rotDelta, manipulator.transform.forward);
              //  ControlledObject.transform.parent.localRotation *= manipulator.transform.localRotation;

                LastMousePosition = Input.mousePosition;

            }
        }
    }
}