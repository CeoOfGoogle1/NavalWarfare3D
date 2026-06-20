using System;
using UnityEngine;

[Serializable]
public class PIDController 
{
    public enum DerivativeMeasurement 
    {
        Velocity,
        ErrorRateOfChange
    }

    //PID coefficients
    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;
    public float outputMin = -1;
    public float outputMax = 1;
    public float integralSaturation;
    public DerivativeMeasurement derivativeMeasurement;
    float valueLast;
    float errorLast;
    float integralAccumulation;
    bool derivativeInitialized;

    public void Reset() 
    {
        derivativeInitialized = false;
        integralAccumulation = 0f;
        valueLast = 0f;
        errorLast = 0f;
    }

    public float Update(float dt, float currentValue, float targetValue, bool isAngle = false)
    {
        if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));

        float error;
        if (isAngle) { error = Mathf.DeltaAngle(currentValue, targetValue); }
        else { error = targetValue - currentValue; }

        //calculate P term
        float P = proportionalGain * error;

        //calculate I term
        integralAccumulation = Mathf.Clamp(integralAccumulation + (error * dt), -integralSaturation, integralSaturation);
        float I = integralGain * integralAccumulation;

        //calculate both D terms
        float errorRateOfChange = isAngle ? Mathf.DeltaAngle(errorLast, error) / dt : (error - errorLast) / dt;

        float valueRateOfChange = isAngle ? Mathf.DeltaAngle(valueLast, currentValue) / dt : (currentValue - valueLast) / dt;

        errorLast = error;
        valueLast = currentValue;

        //choose D term to use
        float derivativeMeasure = 0;

        if (derivativeInitialized)
        {
            derivativeMeasure = derivativeMeasurement == DerivativeMeasurement.Velocity ? -valueRateOfChange : errorRateOfChange;
        } 
        else { derivativeInitialized = true; }

        float D = derivativeGain * derivativeMeasure;

        float result = P + I + D;

        return Mathf.Clamp(result, outputMin, outputMax);
    }
}
