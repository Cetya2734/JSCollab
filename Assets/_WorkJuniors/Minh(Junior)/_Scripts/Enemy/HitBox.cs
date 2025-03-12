using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Target target; // Assign the parent enemy's Target script in the inspector
    public bool isLightBulb; // Check this for the light bulb collider

    public void TakeDamage(float amount, Vector3 hitPos)
    {
        target.TakeDamage(amount, hitPos, isLightBulb);
    }
}