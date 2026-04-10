using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class plasmaRilfeProjectile : MonoBehaviour
{
    // --- Config ---
    public float speed = 1;
    public LayerMask collisionLayerMask;

    private void Update()
    {
        // --- moves the game object in the forward direction at the defined speed ---
        transform.position += -transform.right * (speed * Time.deltaTime);
    }

    /// <summary>
    /// Explodes on contact.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
        if (!enabled) return;

        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }

        Destroy(gameObject);
    }
}
