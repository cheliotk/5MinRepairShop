using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsManager : MonoBehaviour
{


    public void BackToMenu(){
        GameObject.FindObjectOfType<GameManagerScript>().StartLoadNextScene(0);
    }
}
