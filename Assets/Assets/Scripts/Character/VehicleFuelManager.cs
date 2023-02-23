using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {

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
    }
}
