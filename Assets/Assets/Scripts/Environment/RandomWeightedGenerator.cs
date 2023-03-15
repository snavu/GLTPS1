using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeightedGenerator : MonoBehaviour
{
    //returns random index of the array
    //note: index of weights array correlates to index of item selected
    public static int GenerateRandomIndex(int[] weights)
    {
        //construct array of cumulative values from weights 
        //ie from weight values 5,3,4: |.....5...8....12| 
        List<int> bounds = new List<int>();
        bounds.Add(weights[0]);
        for (int i = 1; i < weights.Length; i++)
        {
            bounds.Add(weights[i] + bounds[i - 1]);
        }

        //return the index for which the random value in less than the value of the bound
        int randomVal = Random.Range(0, bounds[bounds.Count - 1] + 1);
        for (int i = 0; i < bounds.Count; i++)
        {
            if (randomVal <= bounds[i])
            {
                return i;
            }
        }
        return 0;
    }
}
