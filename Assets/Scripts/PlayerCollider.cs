using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

  // if gameobject needs to persist just stop rendering it
  //Renderer rend;

  //void Start()
  //{
  //  rend = GetComponent<Renderer>();
  //  rend.enabled = true;
  //}

  void OnTriggerEnter(Collider other)
  {
    //Debug.Log("Collision detected with: " + collision.gameObject.name);

    if (other.CompareTag("Player"))
    {
      Destroy(gameObject);

      // if gameobject needs to persist just stop rendering it
      //rend.enabled = false;
    }
  }
}
