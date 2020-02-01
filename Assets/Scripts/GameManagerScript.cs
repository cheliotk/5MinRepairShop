using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void LoadNextScene(){
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex < 4){
            SceneManager.LoadSceneAsync(sceneIndex+1);
        }
    }
}
