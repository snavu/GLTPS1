using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VehicleFuelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private Vector2 movement;
    public float maxFuel;
    public float currentFuel;
    [SerializeField]
    private float defaultFuelConsumptionRate;
    [SerializeField]
    private float increasedFuelConsumptionRate;

    [SerializeField]
    private RectTransform fuelBar;
    private float rectTransformInitialHeight;

    void Start()
    {
        rectTransformInitialHeight = fuelBar.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        movement = playerMovementScript.actions.Vehicle.Drive.ReadValue<Vector2>();

        if (Mathf.Abs(movement.y) > 0)
        {
            if (playerMovementScript.actions.Vehicle.Accelerate.IsPressed())
            {
                currentFuel -= increasedFuelConsumptionRate * Time.deltaTime;
            }
            currentFuel -= defaultFuelConsumptionRate * Time.deltaTime;
        }
        //scale fuel bar directly proportionally to current fuel level
        fuelBar.sizeDelta = new Vector2(fuelBar.sizeDelta.x,  rectTransformInitialHeight * (currentFuel / maxFuel));
    
    }
}
