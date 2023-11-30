using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollider : MonoBehaviour
{

    [SerializeField] private GameObject spikeBallTrap;
    private Vector2 initialPosition = new Vector2(-4f, 22.48f);
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            spikeBallTrap.transform.position = initialPosition;
        }
    }
}
