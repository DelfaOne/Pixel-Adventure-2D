using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) //get the top of the platform
    {

        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform); //transform compnent of the sticky platform because we are inide StickyPlatform class
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null); //transform compnent of the sticky platform because we are inide StickyPlatform class
        }
    }
}
