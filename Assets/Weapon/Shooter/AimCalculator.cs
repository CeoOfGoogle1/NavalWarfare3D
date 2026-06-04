using UnityEngine;

public class AimCalculator : MonoBehaviour
{
    public int iterations = 3;

    public bool CalculateAim(
        Vector3 targetPosition,
        Vector3 targetVelocity,
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float muzzleVelocity,
        out Vector3 aimDirection)
    {
        aimDirection = Vector3.forward;

        // relative target motion
        Vector3 relVel = targetVelocity - shooterVelocity;

        // Initial intercept time estimate
        float t = FirstInterceptTime(
            muzzleVelocity,
            targetPosition - shooterPosition,
            relVel);

        if (t <= 0f)
            return false;

        // Refine several times because gravity changes flight time
        for(int i = 0; i < iterations; i++)
        {
            Vector3 interceptPos =
                targetPosition + relVel * t;

            if(!SolveBallisticArc(
                shooterPosition,
                interceptPos,
                muzzleVelocity,
                out aimDirection,
                out float flightTime))
                return false;

            t = flightTime;
        }

        return true;
    }

    float FirstInterceptTime(
        float shotSpeed,
        Vector3 relPos,
        Vector3 relVel)
    {
        float a = Vector3.Dot(relVel, relVel) - shotSpeed * shotSpeed;
        float b = 2f * Vector3.Dot(relVel, relPos);
        float c = Vector3.Dot(relPos, relPos);

        float det = b*b - 4*a*c;

        if(det < 0)
            return -1;

        if(Mathf.Abs(a) < 0.001f)
            return -c / b;

        float t1 = (-b - Mathf.Sqrt(det))/(2*a);
        float t2 = (-b + Mathf.Sqrt(det))/(2*a);

        if(t1 > 0 && t2 > 0)
            return Mathf.Min(t1,t2);

        return Mathf.Max(t1,t2);
    }


    bool SolveBallisticArc(
        Vector3 origin,
        Vector3 target,
        float speed,
        out Vector3 fireDir,
        out float flightTime)
    {
        fireDir = Vector3.forward;
        flightTime = 0f;

        Vector3 diff = target - origin;

        Vector3 horizontal =
            new Vector3(diff.x,0,diff.z);

        float x = horizontal.magnitude;
        float y = diff.y;

        float g = Mathf.Abs(Physics.gravity.y);

        float v2 = speed*speed;

        float root =
            v2*v2 -
            g*(g*x*x + 2*y*v2);

        if(root < 0)
            return false;

        // Low arc
        float angle =
            Mathf.Atan(
                (v2 - Mathf.Sqrt(root))
                /(g*x)
            );

        Vector3 flatDir = horizontal.normalized;

        fireDir =
            flatDir*Mathf.Cos(angle)
            + Vector3.up*Mathf.Sin(angle);

        flightTime =
            x/(speed*Mathf.Cos(angle));

        return true;
    }
}