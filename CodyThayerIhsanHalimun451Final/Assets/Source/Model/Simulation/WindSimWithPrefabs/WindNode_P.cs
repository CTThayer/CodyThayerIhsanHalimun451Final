using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindNode_P : MonoBehaviour
{
    public float MAX_R;
    public float MIN_R;
    public float CurrentTheta;
    public float Weighting;

    Quaternion OriginalRotation;
    
    public void SetOriginalRotation(Quaternion Q)
    {
        OriginalRotation = Q;
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
    public void ForwardStep(float t)
    {
        if (CurrentTheta + t <= MAX_R)
            CurrentTheta += t;
        else
            CurrentTheta = MAX_R;
    }

    // Simple clamping and total rotation tracking
    public void BackwardStep(float t)
    {
        if (CurrentTheta - t >= MIN_R)
            CurrentTheta -= t;
        else
            CurrentTheta = MIN_R;
    }

}
