using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] chunk;
    [SerializeField]
    private float[] percentOccurance;

    public void GenerateChunk(){
        GameObject chunkCopy = Instantiate(chunk[0], transform.position, transform.rotation);
    }
}
