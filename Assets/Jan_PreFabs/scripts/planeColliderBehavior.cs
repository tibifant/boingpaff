using UnityEngine;

public class PlayerOnlyCollision : MonoBehaviour
{
    private Collider myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Überprüfen, ob das kollidierende Objekt nicht der Player ist
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Kollision ignorieren
            Physics.IgnoreCollision(myCollider, collision.collider);
        }
    }
}
