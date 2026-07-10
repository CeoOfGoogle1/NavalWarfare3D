using UnityEngine;

[CreateAssetMenu(fileName = "ShipHullData", menuName = "Scriptable Objects/ShipHullData")]
public class ShipHullData : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3[] turretPositions;
    [SerializeField] Vector3[] torpedoLauncherPositions;
    [SerializeField] Vector3[] antiAirPositions;

}
