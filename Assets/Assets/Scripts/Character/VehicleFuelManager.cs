using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VehicleFuelManager : MonoBehaviour
{
    public PlayerInputInitialize playerInputScript;
    [SerializeField] private Vector2 movement;
    public float maxFuel;
    public float currentFuel;
    [SerializeField] private float defaultFuelConsumptionRate;
    [SerializeField] private float increasedFuelConsumptionRate;
    public float refuelingRate;

    [SerializeField] private RectTransform fuelBar;
    private float rectTransformInitialHeight;

    [SerializeField] private SkinnedMeshRenderer ketBarrelMesh;
    void Start()
    {
        rectTransformInitialHeight = fuelBar.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        movement = playerInputScript.actions.Vehicle.Drive.ReadValue<Vector2>();

        //clamp fuel
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);

        if (Mathf.Abs(movement.y) > 0 && ketBarrelMesh.enabled)
        {
            if (playerInputScript.actions.Vehicle.Accelerate.IsPressed())
            {
                currentFuel -= increasedFuelConsumptionRate * Time.deltaTime;
            }
            else
            {
                currentFuel -= defaultFuelConsumptionRate * Time.deltaTime;
            }
        }
        //scale fuel bar directly proportionally to current fuel level
        fuelBar.sizeDelta = new Vector2(fuelBar.sizeDelta.x, rectTransformInitialHeight * (currentFuel / maxFuel));

    }
}
