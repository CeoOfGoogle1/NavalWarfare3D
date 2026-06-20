using UnityEngine;

public class Atmosphere : MonoBehaviour
{
    public static float GetAirDensity(float altitude)
    {
        // Sea-level density
        float rho0 = 1.225f; 

        if (altitude < 11000f) // Troposphere
        {
            float temp = 288.15f - 0.0065f * altitude;
            return rho0 * Mathf.Pow(temp / 288.15f, 4.256f); 
        }
        else if (altitude < 20000f) // Lower stratosphere
        {
            return 0.36391f * Mathf.Exp(-0.00015769f * (altitude - 11000f)); 
        }
        else // High altitudes, simple exponential decay
        {
            return 0.08803f * Mathf.Exp(-0.0001651f * (altitude - 20000f));
        }
    }

    public static float GetAirDensityFast(float altitude)
    {
        return 1.225f * Mathf.Exp(-0.00011856f * altitude); // sea-level density * decay
    }
}
