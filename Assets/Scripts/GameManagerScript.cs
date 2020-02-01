using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    bool hasCrosshair = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasCrosshair(){
        return hasCrosshair;
    }

    public void AcquiredCrosshair(){
        hasCrosshair = true;
    }
}
