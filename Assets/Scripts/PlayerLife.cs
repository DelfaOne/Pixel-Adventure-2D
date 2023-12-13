using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator anim;

    [SerializeField] public float health;
    [SerializeField] public float numberOfHearts;
    private float trapDamages = 0.5f;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

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

    private void Update()
    {
        UpdateHeartsUi();
    }

    private void UpdateHeartsUi()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            float heartValue = i + 1;
            float displayValue = health - heartValue + 0.5f;

            hearts[i].sprite = (displayValue > 0) ? fullHeart : (displayValue == 0) ? halfHeart : emptyHeart;
            hearts[i].enabled = (heartValue <= numberOfHearts);

            if (health > numberOfHearts)
            {
                health = numberOfHearts;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) //TODO make protected
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            ReduceHealth(trapDamages);
        }
    }

    private void ReduceHealth(float amount)
    {
        health -= amount;
        //TODO add damage sound
        //TODO add invicibility time during hit
        if (health <= 0)
        {
            health = 0;
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
