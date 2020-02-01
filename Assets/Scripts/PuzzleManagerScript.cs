using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManagerScript : MonoBehaviour
{
    public bool isPuzzleSetup = false;
    public bool isPuzzleSolved = false;

    SceneManagerScript sceneMan;
    
    public void Init(){
        sceneMan = GameObject.FindObjectOfType<SceneManagerScript>();
        isPuzzleSetup = true;
    }

    public void PuzzleSolved(){
        isPuzzleSolved = true;
        sceneMan.PuzzleSolved();
    }
}
