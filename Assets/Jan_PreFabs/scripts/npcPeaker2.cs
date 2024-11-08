using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcPeaker2 : MonoBehaviour
{
    public float restingPointY = 5f;
    public float restingTime = 1f;
    public float riseSpeed = 0.5f;
    public int scoreValue = 1;

    private Rigidbody rb;
    private float timeResting;
    private bool hasRisen;
    private npcSpawnManagement spawnManager;
    private bool gotHit = false;

    void Start()
    {
        spawnManager = FindObjectOfType<npcSpawnManagement>();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        timeResting = 0;
        hasRisen = false;
    }

    void FixedUpdate()
    {
        // Berechne die neue Position unter Verwendung des Rigidbody
        if (rb.position.y < restingPointY && !hasRisen)
        {
            rb.useGravity = false;
            rise();

        }
        else if (rb.position.y >= restingPointY && timeResting < restingTime)
        {
            hasRisen = true;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            timeResting += Time.fixedDeltaTime;
        }
        else if (timeResting >= restingTime)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            Debug.Log("Gravity is on");
            if (rb.position.y < - 0.1)
            {
                Destroy(gameObject);
            }
        }
    }
    void rise()
    {
        Vector3 upwardMovement = Vector3.up * riseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + upwardMovement);
    }

    private void OnDestroy()
    {
        spawnManager.setSpawnPositionsInUse(gameObject);
        if (gotHit && !gameObject.CompareTag("Bomb"))
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        else if (gotHit && gameObject.CompareTag("Bomb"))
        {
            GameManager.Instance.ResetScore();
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gotHit = true;
            Destroy(gameObject);
        }
    }
}
