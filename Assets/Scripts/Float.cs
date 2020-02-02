using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float amplitude;          //Set in Inspector 
    public float speed;                  //Set in Inspector 
    private float tempVal;
    public Vector3 tempPos;

    public void InitAnim(){

    }

    void Start()
    {
        StartCoroutine(Wait(5.0F));
        speed = 20.0f;
        tempVal = transform.position.y;
        
    }

    void Update()
    {
        tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
        transform.localPosition = tempPos;
    }

    public IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
