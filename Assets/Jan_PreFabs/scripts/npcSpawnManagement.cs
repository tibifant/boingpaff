using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcSpawnManagement : MonoBehaviour
{
    public GameObject[] npcs;         
    public float minSpawnInterval = 1f;   // Minimaler Zeitabstand zwischen Spawns
    public float maxSpawnInterval = 5f;

    private Dictionary<int, Vector3> spawnPoints;
    private Dictionary<int, Quaternion> spawnRotations;
    private Transform spawnPoint;
    private Dictionary<GameObject,int> spawnPositionsInUse;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new Dictionary<int, Vector3>();
        spawnRotations = new Dictionary<int, Quaternion>();
        spawnPositionsInUse = new Dictionary<GameObject, int>();
        setSpawnPoints();
        setSpawnRotations();
        StartCoroutine(SpawnObjectsRandomly());
    }

    IEnumerator SpawnObjectsRandomly()
    {
        while (true)
        {
            // Zufälliges Objekt aus dem Array auswählen
            GameObject objectToSpawn = npcs[Random.Range(0, npcs.Length)];

            // Zufälligen SpawnPoint aus der Liste auswählen
            int spawnPointIndex = getRandomSpawnPointIndex();

            // Zufälligen Zeitabstand für das nächste Spawnen bestimmen
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);

            // Warten, bis der nächste Spawn durchgeführt wird
            yield return new WaitForSeconds(waitTime);

            // Position und Rotation aus dem Dictionary holen
            Vector3 spawnPosition = spawnPoints[spawnPointIndex];
            Quaternion spawnRotation = spawnRotations[spawnPointIndex];

            if(objectToSpawn.name == "NPC_Faller")
            {
                spawnPosition.y = 50;
            }
            // Objekt spawnen
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            // Hinzufügen des gespawnten Objekts zur Liste
            spawnPositionsInUse.Add(spawnedObject, spawnPointIndex);

            logSpawnPositions();
        }
    }

    void setSpawnPoints()
    {
        spawnPoints.Add(0, new Vector3(0.496f, 0, 1.039f));
        spawnPoints.Add(1, new Vector3(0.887f, 0, 0.864f));
        spawnPoints.Add(2, new Vector3(1.085f, 0, 0.381f));
        spawnPoints.Add(3, new Vector3(0.881f, 0, -0.045f));
        spawnPoints.Add(4, new Vector3(0.429f, 0, -0.204f));
        spawnPoints.Add(5, new Vector3(0, 0, 0));
        spawnPoints.Add(6, new Vector3(-0.157f, 0, 0.454f));
        spawnPoints.Add(7, new Vector3(0.048f, 0, 0.879f));
    }
    void setSpawnRotations()
    {
        spawnRotations.Add(0, Quaternion.Euler(0, 180, 0));
        spawnRotations.Add(1, Quaternion.Euler(0, 225, 0));
        spawnRotations.Add(2, Quaternion.Euler(0, 270, 0));
        spawnRotations.Add(3, Quaternion.Euler(0, 315, 0));
        spawnRotations.Add(4, Quaternion.Euler(0, 0, 0));
        spawnRotations.Add(5, Quaternion.Euler(0, 45, 0));
        spawnRotations.Add(6, Quaternion.Euler(0, 90, 0));
        spawnRotations.Add(7, Quaternion.Euler(0, 135, 0));
    }

    int getRandomSpawnPointIndex()
    {
        int randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPositionsInUse.ContainsValue(randomIndex));
        return randomIndex;
    }

    public void setSpawnPositionsInUse(GameObject objectAsKey)
    {
        if(objectAsKey == null)
        {
            Debug.LogError("Object is null");
        }
        else if (spawnPositionsInUse.ContainsKey(objectAsKey))
        {
            spawnPositionsInUse.Remove(objectAsKey);
            Debug.Log("Removed object from spawnPositionsInUse");
        }
    }

    void logSpawnPositions()
    {
        Debug.Log(spawnPositionsInUse);
    }
}
