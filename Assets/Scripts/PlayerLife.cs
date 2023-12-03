using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator anim;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision) //TODO make protected
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        audioManager.PlaySFX(audioManager.death);
        player.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death"); //use trigger instead of boolean to call it one time
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
