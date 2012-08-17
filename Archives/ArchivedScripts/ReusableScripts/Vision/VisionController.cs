using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Reusable/VisionController")]
public class VisionController : MonoBehaviour {
    
    
    [SerializeField]
    private float goOut = 5;
    
    
    [SerializeField]
    private GameObject attachTo;
    
    
    /// <summary>
    /// Wake up in the morning and an invisible field of view trigger. It takes a cube and modifies it into a viewport
    /// </summary>
    void Awake(){
        
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = Camera.main.transform.position;
        Destroy(cube.collider);
        Mesh mesh = cube.GetComponent<MeshFilter>().mesh;
    
        Vector3[] vertices = new Vector3[24];
        
        vertices[0] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, goOut));
        vertices[1] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, goOut));
        vertices[2] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, goOut));
        vertices[3] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, goOut));
        
        vertices[4] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        vertices[5] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
        vertices[6] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, Camera.main.nearClipPlane));
        vertices[7] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.nearClipPlane));
        
        vertices[8] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, goOut));
        vertices[9] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, goOut));
        vertices[10] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        vertices[11] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
        
        vertices[12] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, Camera.main.nearClipPlane));
        vertices[13] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, goOut));
        vertices[14] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.nearClipPlane));
        vertices[15] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, goOut));
        
        vertices[16] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, goOut));
        vertices[17] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
        vertices[18] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.nearClipPlane));
        vertices[19] = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, goOut));
        
        vertices[20] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, Camera.main.nearClipPlane));
        vertices[21] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, goOut));
        vertices[22] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, goOut));
        vertices[23] = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        
        MeshCollider m2 = attachTo.AddComponent<MeshCollider>();
        Vision v = cube.AddComponent<Vision>();
        Rigidbody r = cube.AddComponent<Rigidbody>();
        r.useGravity = false;
        m2.isTrigger = true;

        //debug.log(attachTo.transform.position.x + " " + attachTo.transform.position.y);
        cube.transform.position = new Vector3(attachTo.transform.position.x, attachTo.transform.position.y, attachTo.transform.position.z);
    }
    
    void OnDrawGizmos() {
        Vector3 p = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.1F);
        
        Vector3 p2 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p2, 0.1F);
        
        Vector3 p3 = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p3, 0.1F);
        
        Vector3 p4 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p4, 0.1F);
        
        Vector3 p5 = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, goOut));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p5, 0.1F);
        
        Vector3 p6 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, goOut));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p6, 0.1F);
        
        Vector3 p7 = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, goOut));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p7, 0.1F);
        
        Vector3 p8 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, goOut));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p8, 0.1F);
        
    }
    
    
    
    
}