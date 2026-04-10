using UnityEngine;

[CreateAssetMenu(fileName = "OxygenZone", menuName = "Oxygen/Zone")]
public class OxygenZone : ScriptableObject
{
    public string zoneName;
    public float zThreshold;
    public int requiredFilterTier;
    public float drainRatePerSecond;
    public float refillRatePerSecond = 2f;
    public float tickOfZone = 2f;
}
