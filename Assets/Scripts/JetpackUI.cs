using UnityEngine;
using UnityEngine.UI;

public class JetpackUI : MonoBehaviour
{
    public JetpackController jetpack; // Drag your Player with the script attached
    public Slider fuelBar; // Drag the UI Slider here

    void Update()
    {
        if (jetpack != null && fuelBar != null)
        {
            fuelBar.value = jetpack.FuelNormalized;
        }
    }
}
