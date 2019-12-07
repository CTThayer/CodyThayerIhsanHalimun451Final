using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNode : MonoBehaviour
{


    public float Rigidity;  // Modifier value that impacts how much this node 
                            // actually moves. 
                            // 0 = completely rigid, 1 = full rotation allowed

    public float MAX_R;    // Max foward rotation in degrees
    public float MIN_R;    // Min rotation (Max backward rotation)
    public float stepsize; // Degrees to rotate per step
    public bool forward;   // Is movement forward or backward with respect to orig loc
    public float currentR; // Current degrees rotated

    Vector3 OrigUp; // Original up vector


    // Start is called before the first frame update
    void Start()
    {
        OrigUp = this.transform.up;
    }

    public float GetCurrentR() {return currentR;}

    public void SetOriginalUpVector()
    {
        OrigUp = this.transform.up;
    }

    public Vector3 GetOriginalupVector()
    {
        return OrigUp;
    }

    public void ApplyWindMovement()
    {
        
        // Temp test code
        Quaternion cR = this.transform.localRotation;
        Vector3 vecR = cR.eulerAngles;

        if (forward)
        {
            vecR.x += stepsize * Rigidity;
            currentR += stepsize;
        }
        else
        {
            vecR.x += stepsize * Rigidity * -1.0f;
            currentR -= stepsize;
        }
        
        this.transform.localRotation = Quaternion.Euler(vecR.x, vecR.y, vecR.z);


    }

    public void ApplyWindMovementVector(Vector3 WindVector)
    {

        Quaternion result;
        float theta;
        Vector3 axis;

        if (forward)
        {
            axis = GetForwardAxis(WindVector);
            theta = stepsize * Rigidity;
            result = Quaternion.AngleAxis(theta, axis);
            currentR += stepsize;
        }
        else
        {
            axis = GetReverseVector();
            theta = stepsize * Rigidity;
            result = Quaternion.AngleAxis(theta, axis);
            currentR -= stepsize;
        }

        this.transform.localRotation = this.transform.localRotation * result;


    }

    private Vector3 GetForwardAxis(Vector3 WindVector)
    {
        Vector3 combined = this.transform.up + WindVector;
        Quaternion Q = Quaternion.FromToRotation(transform.up, combined.normalized);

        float DegToCombined;
        Vector3 axis;
        Q.ToAngleAxis(out DegToCombined, out axis);
        return axis;
    }

    private Vector3 GetReverseVector()
    {
        Quaternion Q = Quaternion.FromToRotation(transform.up, OrigUp);

        float DegToCombined;
        Vector3 axis;
        Q.ToAngleAxis(out DegToCombined, out axis);
        return axis;
    }

}
