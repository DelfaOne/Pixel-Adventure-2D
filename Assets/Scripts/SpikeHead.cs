using System.Collections;
using UnityEngine;

public class SpikeHead : PlayerLife
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    private float checkTimer;
    [SerializeField] private LayerMask playerLayer;
    private Vector3 destination;
    private Vector3[] directions = new Vector3[4];
    private Vector3 initialPosition;
    private bool attacking;

    [Header("Directions check selection")]
    [SerializeField] private bool checkRight;
    [SerializeField] private bool checkLeft;
    [SerializeField] private bool checkDown;
    [SerializeField] private bool checkUp;

    private Animator anim;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    //call when object is activated
    private void OnEnable()
    {
        Stop();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }

    }


    private void CheckForPlayer()
    {
        CalculateDirections();
        //Check if spikehead see player

        for (int i = 0; i < directions.Length; i++) //TODO try foreach
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                anim.SetTrigger("playerDetected");
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }

    }

    private void CalculateDirections()
    {
        if (checkRight)
        {
            directions[0] = transform.right * range; //right direction
        }
        else if (checkLeft)
        {
            directions[1] = -transform.right * range; //left direction
        }
        else if (checkUp)
        {
            directions[2] = transform.up * range; //up direction
        }
        else if (checkDown)
        {
            directions[3] = -transform.up * range; //down direction
        }

    }

    private void Stop()
    {
        destination = transform.position; // set destination as current position
        attacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("isHit");
        audioManager.PlaySFX(audioManager.rockHit);
        base.OnCollisionEnter2D(collision);
        Stop();
        ResetPosition(); //TODO fix reset dont detect player after reset position
    }

    private void ResetPosition()
    {
        StartCoroutine(MoveToInitialPosition());
    }

    private IEnumerator MoveToInitialPosition()
    {
        yield return new WaitForSeconds(2f);

        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < 10f)  // You can adjust the duration of the movement
        {
            transform.position = Vector3.Lerp(startingPosition, initialPosition, elapsedTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        attacking = false;
    }
}
