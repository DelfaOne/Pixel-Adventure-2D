using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int apples = 0;

    [SerializeField] private Text applesText;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) // can override because is trigger is checked
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            audioManager.PlaySFX(audioManager.collectItem);
            Destroy(collision.gameObject);
            apples++;
            applesText.text = apples.ToString();
        }
    }
}
