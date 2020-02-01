using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectScript : MonoBehaviour
{
    public List<Part> requiredParts;
    List<Vector3> partsCorrectPosition;
    List<Quaternion> partsCorrectRotation;
    public List<GameObject> locationsOnShelf;

    public float distanceThresholdForCorrectPlacement = 0.1f;
    public float angleThresholdForCurrentPlacement = 5f;
    // Start is called before the first frame update
    void Start()
    {
        partsCorrectPosition = new List<Vector3>();
        partsCorrectRotation = new List<Quaternion>();
        foreach(Part p in requiredParts){
            partsCorrectPosition.Add(p.transform.localPosition);
            partsCorrectRotation.Add(p.transform.localRotation);
        }

        PlacePartsOnShelf();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlacePartsOnShelf(){
        for (int i = 0; i < requiredParts.Count; i++)
        {
            Part p = requiredParts[i];
            p.transform.parent = null;
            p.transform.position = locationsOnShelf[i].transform.position;
            p.transform.rotation = locationsOnShelf[i].transform.rotation;
        }
    }

    bool CheckPartIsPlacedCorrectly(Part part){
        bool v = false;
        int index = requiredParts.FindIndex(p => p == part);

        if(CheckPosition(part.transform.position, partsCorrectPosition[index])
            && CheckRotation(part.transform.rotation, partsCorrectRotation[index])){
            
            v = true;
        }
        
        return v;
    }

    bool CheckPosition(Vector3 partPositionInGlobal, Vector3 correctPosition){
        bool v = false;
        Vector3 partPosInLocal = transform.InverseTransformPoint(partPositionInGlobal);
        float dist = Vector3.Distance(partPosInLocal, correctPosition);
        if(dist < distanceThresholdForCorrectPlacement){
            v = true;
        }
        return v;
    }

    bool CheckRotation(Quaternion partRotationInGlobal, Quaternion correctRotation){
        bool v = false;

        Quaternion partRotInLocal = Quaternion.Inverse(this.transform.rotation) * partRotationInGlobal;
        float angle = Quaternion.Angle(partRotInLocal, correctRotation);
        if(angle < angleThresholdForCurrentPlacement){
            v = true;
        }
        return v;
    }
}
