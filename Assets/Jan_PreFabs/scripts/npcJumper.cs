using System.Collections;
using UnityEngine;

public class NpcJumper : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Vector3 startingPosition;
    private float timeSinceStart;
    private npcSpawnManagement spawnManager;
    private bool gotHit = false;

    public int scoreValue = 1;
    public float jumpSpeed;
    public float gravity;

    void Start()
    {
        spawnManager = FindObjectOfType<npcSpawnManagement>();

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        timeSinceStart = 0f;
    }

    void Update()
    {
        timeSinceStart += Time.deltaTime;
        if (transform.position.y < startingPosition.y - 10)
        {
            Destroy(gameObject);
        }
        else
        {
            calcYPositon(timeSinceStart);
        }
    }

    void calcYPositon(float timeSinceStart)
    {
        float newY = startingPosition.y + (jumpSpeed * timeSinceStart - 0.5f * gravity * (timeSinceStart * timeSinceStart));
        transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
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
