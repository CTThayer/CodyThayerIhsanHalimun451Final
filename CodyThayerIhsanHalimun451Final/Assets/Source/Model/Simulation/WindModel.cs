using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindModel : MonoBehaviour
{
    public Vector3 WindVector;  // Direction and Magnitude of the wind
    public float Rc;            // Rigidity Coefficient for the whole tree
    public float Gi;            // Gust Interval: timeframe for "current gust"

    private const float Pc = 0.613f;    // "Standard constant" for wind pressure
                                        //  calculations in Newtons/m^2


    /************************** GetWindLoadOnTree **************************//**
    * Calculates the wind load on the tree (and it's individual components) 
    * using the helper methods: CalculateProjectedArea() and CalculateWindLoad()
    * 
    *    @param    tree     is the tree to calculate the wind load on
    *    @param    wind     is the wind vector
    *    @param    dragC    is the user specified drag coefficient for the tree
    *    
    ***************************************************************************/
    public float GetWindLoadOnTree(DynamicTree tree, Vector3 wind, float dragC)
    {
        float area = CalculateProjectedArea(tree, wind);
        return CalculateWindLoad(area, dragC, wind);
    }

    /*********************** CalculateProjectedArea ************************//**
    * Calculates the projection of the tree's geometry onto a plane that is
    * perpendicular to the wind vector. This projection is used to calculate the
    * force of the wind and to "weight" the effects of the wind on the 
    * individual parts of the tree.
    *    @param    tree     is the tree to calculate the wind load on
    *    @param    wind     is the wind vector
    *    
    ***************************************************************************/
    float CalculateProjectedArea(DynamicTree tree, Vector3 wind)
    {
        float area = 0;

        // TODO:    Calculate Projection Plane
        // TODO:    Verify Cross Product order is correct
        Vector3 pX = Vector3.Cross(wind, Vector3.up);


        // TODO:    Use leaf-to-root traversal to navigate through the tree. 
        //          CalculateProjected area at each node

        // TODO:    Implement helper method for calculating the projection of 
        //          individual geometry pieces onto the plane.

        return area;
    }

    /************************* CalculateWindLoad ***************************//**
     * Calculates wind load for the tree based on current wind settings
     * Uses the following simplified wind load model:
     *    F = A x P x Cd
     *      where:
     *    @param    F   is the force or wind load
     *    @param    A   is the projected area of the object (SA)
     *    @param    P   is the wind pressure
     *    @param    Cd  is the drag coefficient
     **************************************************************************/
    float CalculateWindLoad(float A, float Cd, Vector3 wind)
    {
        float W = wind.magnitude;
        float P = Pc * W * W;
        return A * P * Cd;
    }


}
