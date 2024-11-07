using UnityEngine;

public class LimitSpeed : MonoBehaviour
{
    public float maxSpeed; // Maximale Geschwindigkeit

    private Rigidbody rb;
    private npcSpawnManagement spawnManager;

    void Start()
    {
        spawnManager = FindObjectOfType<npcSpawnManagement>();

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Geschwindigkeit begrenzen, wenn sie das Maximum überschreitet
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if(rb.position.y < -5.8)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        spawnManager.setSpawnPositionsInUse(gameObject);
    }

}
