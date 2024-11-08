using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public float rotationSpeed = 0.01f;
    public GameObject onCollectEffect;
    public int scoreValue = 10; // Punktewert des Balls

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
         transform.Rotate(0, rotationSpeed, 0);
         // increase the rotation speed
            rotationSpeed += 0.00001f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // Instantiate the effect at the collectible's position
            Instantiate(onCollectEffect, transform.position, Quaternion.identity);
            // Destroy the collectible
            Destroy(gameObject);
        }
        GameManager.Instance.AddScore(scoreValue);
    }
}
