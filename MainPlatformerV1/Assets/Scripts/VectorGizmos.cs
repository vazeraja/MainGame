using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class VectorGizmos : MonoBehaviour {
    [Range(0f, 10f)] public float radius = 5f;
    [Range(0f, 10f)] public float offset = 5f;

    public GameObject player;

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        Vector2 origin = transform.position;
        Vector2 obj = player.transform.position;

        Vector2 objToOrigin = origin - obj;
        Vector2 objToOriginDir = objToOrigin.normalized;
        Gizmos.DrawLine(obj, (obj + objToOriginDir*5));
        Vector2 offsetVec = objToOriginDir * offset;
        Gizmos.DrawSphere(obj + offsetVec, .1f);
        
        // Vector2 disp = obj - origin;
        // float distSq = disp.sqrMagnitude;
        // bool isInside = distSq < radius * radius;
        
        Vector2 disp = (obj + offsetVec) - origin; // The vector going from the offsetvector point to the origin
        float distSq = disp.sqrMagnitude; // (x^2 + y^2), also called the sqrMagnitude
        bool isInside = distSq < radius * radius; // checks if inside radius

        Gizmos.color = isInside ? Color.green : Color.red;
        Gizmos.DrawWireSphere(origin, radius);
    }
    #endif
}