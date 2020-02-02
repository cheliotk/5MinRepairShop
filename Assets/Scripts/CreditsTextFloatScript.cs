using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsTextFloatScript : MonoBehaviour
{
    Vector3 startPosition;
    [Range(0f,1f)]
    public float jitterAmt = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position + Random.insideUnitSphere * jitterAmt;
        Vector3 offset = newPos - startPosition;
        offset.Normalize();
        this.transform.position = startPosition + offset;
    }
}
