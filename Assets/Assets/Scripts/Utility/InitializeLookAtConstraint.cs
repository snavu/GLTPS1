using UnityEngine;
using UnityEngine.Animations;

public class InitializeLookAtConstraint : MonoBehaviour
{
    private ConstraintSource source;
    void Start()
    {
        source.sourceTransform = GameObject.FindWithTag("MainCamera").transform;
        source.weight = 1;
        GetComponent<LookAtConstraint>().SetSource(0, source);

    }
}
