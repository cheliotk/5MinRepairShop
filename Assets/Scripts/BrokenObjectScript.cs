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

    bool isPuzzleSolved = false;

    public void Init(){
        DisassembleObject();
    }

    void DisassembleObject(){
        partsCorrectPosition = new List<Vector3>();
        partsCorrectRotation = new List<Quaternion>();
        foreach(Part p in requiredParts){
            partsCorrectPosition.Add(p.transform.localPosition);
            partsCorrectRotation.Add(p.transform.localRotation);
        }

        PlacePartsOnShelf();
        TurnOffLocationsOnShelf();
    }

    void TurnOffLocationsOnShelf(){
        foreach(GameObject l in locationsOnShelf){
            l.GetComponent<Renderer>().enabled = false;
        }
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

    public bool CheckPartIsPlacedCorrectly(Part part){
        bool v = false;
        int index = requiredParts.FindIndex(p => p == part);

        if(CheckPosition(part.transform.position, partsCorrectPosition[index])
            && CheckRotation(part.transform.rotation, partsCorrectRotation[index])){
            
            v = true;
        }
        
        return v;
    }

    bool CheckAllPartsPlaced(){
        bool v = true;
        foreach(Part p in requiredParts){
            if (!CheckPartIsPlacedCorrectly(p)){
                v = false;
            }
        }

        return v;
    }

    public void PlacePartOnObject(Part part){
        part.transform.parent = this.transform;

        int index = requiredParts.FindIndex(p => p == part);
        part.transform.localPosition = partsCorrectPosition[index];
        part.transform.localRotation = partsCorrectRotation[index];

        if(CheckAllPartsPlaced()){
            GameObject.FindObjectOfType<PuzzleManagerScript>().PuzzleSolved();
        }
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
