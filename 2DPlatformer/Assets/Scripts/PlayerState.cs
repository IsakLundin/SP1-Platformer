using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public int healthPoints = 2;
    public int initialHealthPoints = 2;

    public int coinAmount = 0;

    public ParticleSystem deathParticles;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator animator;
    private float timer;
    public float respawnTimer = 1f;
    private bool playerDead = false;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Collider2D coll2D;

    private GameObject respawnPosition;
    [SerializeField] private GameObject startPosition;
    [SerializeField] private bool useStartPosition = true;

    // Start is called before the first frame update
    void Start()
    { 
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        coll2D = gameObject.GetComponent<Collider2D>();

        healthPoints = initialHealthPoints;
        if(useStartPosition == true)
        {
            gameObject.transform.position = startPosition.transform.position;
        }
        respawnPosition = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDead == true)
        {
            timer += Time.deltaTime;
            if(timer >= respawnTimer)
            {
                Respawn();
                timer = 0f;
            }
        }
    }

    public void DoHarm(int doHarmByThisMuch)
    {
        healthPoints -= doHarmByThisMuch;

        if(healthPoints <= 0)
        {
            PlayerDie();
        }
    }

    public void PlayerDie()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        playerSpriteRenderer.enabled = false;
        animator.enabled = false;
        deathParticles.Play();
        rigidBody2D.bodyType = RigidbodyType2D.Static;
        coll2D.enabled = false;
        playerDead = true;
    }

    public void Respawn()
    {
        playerDead = false;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidBody2D.gravityScale = Mathf.Abs(rigidBody2D.gravityScale);
        transform.eulerAngles = Vector3.zero;
        gameObject.GetComponent<PlayerMovement>().gravityTop = false;
        coll2D.enabled = true;
        playerSpriteRenderer.enabled = true;
        animator.enabled = true;
        healthPoints = initialHealthPoints;
        gameObject.transform.position = respawnPosition.transform.position;
    }

    public void CoinPickup()
    {
        coinAmount++;
    }

    public void ChangeRespawnPosition(GameObject newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }
}
