using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollide : MonoBehaviour
{
  float time;

  Renderer rend;
  float offset = 0.6f;
  float z = 1f;
  //Vector3 pos = new Vector3(0.015f, 1.201026f, 0.389f);

  void Start()
  {
    rend = GetComponent<Renderer>();
    rend.enabled = true;
  }

  void Update()
  {
    if (!rend.enabled)
    {
      if (time + 3 < Time.time)
      {
        int r = Random.Range(0, 6);
        Debug.Log(r);
        float a = (2 * Mathf.PI) / r;
        float x = Mathf.Sin(a) * offset;
        float y = Mathf.Cos(a) * offset;
        Debug.Log("angle: " + a);
        Debug.Log("sin: " + Mathf.Sin(a));
        Debug.Log("cos: " + Mathf.Cos(a));
        gameObject.transform.position = new Vector3(x, z, y);
        Debug.Log(gameObject.transform.position);
        rend.enabled = true;
      }
    }
  }

  void OnTriggerEnter(Collider other)
  {
    //Debug.Log("Collision detected with: " + collision.gameObject.name);

    if (other.CompareTag("Player"))
    {
      time = Time.time;
      rend.enabled = false;
    }
  }
}
