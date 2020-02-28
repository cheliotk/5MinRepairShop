using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _testingScript : MonoBehaviour
{

    Renderer r;
    Collider c;
    Mesh m;
    // Start is called before the first frame update
    void Start()
    {
        r = this.GetComponent<Renderer>();
        c = this.GetComponent<Collider>();
        m = this.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        m.RecalculateBounds();
        Bounds b = r.bounds;

        print(b.center + " | " + b.extents);
    }
    
    private void OnDrawGizmos() {
        m.RecalculateBounds();
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(r.bounds.center, m.bounds.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(r.bounds.center, r.bounds.size);
    }
}
