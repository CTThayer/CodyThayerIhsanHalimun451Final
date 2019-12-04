using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindNode_P : MonoBehaviour
{
    public float MAX_R;
    public float MIN_R;
    public float Weighting;

    private float CurrentTheta = 0;
    private float StepTheta = 0;

    

    Quaternion OriginalRotation;
    Vector3 OriginalUp;
    
    public void SetOriginalRotation(Quaternion Q)
    {
        OriginalRotation = Q;
        OriginalUp = this.transform.up;
    }

    public Quaternion GetOriginalRotation()
    {
        return OriginalRotation;
    }

    public void ResetToOriginalRotation()
    {
        //this.transform.rotation = OriginalRotation;
        this.transform.localRotation = OriginalRotation;
    }

    // Simple clamping and total rotation tracking
    public float ForwardStep(float t)
    {
        if (CurrentTheta + t <= MAX_R)
        {
            CurrentTheta += t;
            return t;
        }
        else
        {
            float r = MAX_R - CurrentTheta;
            CurrentTheta = MAX_R;
            return r;
        }
          
    }

    // Simple clamping and total rotation tracking
    public float BackwardStep(float t)
    {
        if (CurrentTheta - t >= MIN_R)
        {
            CurrentTheta -= t;
            return t;
        }
        else
        {
            float r = CurrentTheta - t;
            CurrentTheta = MIN_R;
            return r;
        }
    }

    public Vector3 GetNodePlusWindDirection(Vector3 WindVector)
    {
        // Vector direction that the wind pushes the object towards
        Vector3 CombinedV = this.transform.up + WindVector;
        return CombinedV.normalized;
    }

    public float GetStepAngle()
    {
        return Weighting * StepTheta;
    }

    public Vector3 GetAxis(Vector3 vec)
    {
        return Vector3.Cross(vec, this.transform.up);
    }

    public void SetStepTheta(int steps)
    {
        StepTheta = MAX_R / steps;
    }

    public Vector3 GetReverseAxis()
    {
        Quaternion backwardR = Quaternion.FromToRotation(transform.up, OriginalUp);
        float angle;
        Vector3 axis;
        backwardR.ToAngleAxis(out angle, out axis);

        //return Vector3.Cross(this.transform.up, OriginalUp);

        return axis;
    }
}
