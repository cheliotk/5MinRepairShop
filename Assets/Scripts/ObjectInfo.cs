using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    public string itemName;
    [TextArea(10,15)]
    public string description;
    public bool isPickable = false;
    public bool isBrokenObject = false;
    public bool isPartPlaceholder = false;

    private void OnDrawGizmos() {
        Renderer r = GetComponent<Renderer>();
        
        if(r != null){
            Bounds b = r.bounds;
            Gizmos.DrawWireCube(b.center, b.size);
        }

    }

}
