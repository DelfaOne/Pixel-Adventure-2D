using UnityEngine;

public class TrampolineJump : MonoBehaviour
{

    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float bounceForce = 20f;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() == player)
        {
            audioManager.PlaySFX(audioManager.trampoline);
            anim.SetTrigger("jump");
            player.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
