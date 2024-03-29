using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Player : MonoBehaviour
{
    //Movement
    public float jumpForce;
    public float speed;
    private bool isGrounded;

    //Other
    private Rigidbody2D rb;

    //Player
    [SerializeField] private int health;
    private GameObject player;

    //Gun
    public GameObject playerGun;
    public GameObject projectile;
    private GameObject gun;
    private GameObject bullet;
    public Transform bulletHole;
    private SpriteRenderer gunSR;
    public bool canShoot;

    //Audio
    public AudioSource shootSound;

    //Death
    [SerializeField] GameObject deathScreen;
    [SerializeField] private int damageAmount;
    [SerializeField] private GameObject deathParticle;
    private GameObject endDeathParticle;

    private void Awake()
    {
        //Player
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        //Gun
        gun = GameObject.Find("Gun");
        gunSR = playerGun.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        canShoot = false;
    }

    void Update()
    {
        //Player
        PlayerHealth();
        PlayerControls();
        PlayerMovement();
        CheckGround();

        //Other
        CheckGunExistence();
    }

    private void PlayerHealth()
    {
        if (health <= 0)
        {
            endDeathParticle = Instantiate(deathParticle, transform.position, Quaternion.identity);
            deathScreen.SetActive(true);
            Destroy(endDeathParticle, 0.2f);
            Destroy(gameObject);
        }
    }

    private void CheckGround()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void CheckGunExistence()
    {
        if (gun == null)
        {
            playerGun.SetActive(true);
        }

        FlipGun();
    }

    private void FlipGun()
    {
        if (playerGun.transform.rotation.z > 0.7f)
        {
            gunSR.flipY = true;
        }
        else if (playerGun.transform.rotation.z < -0.7f)
        {
            gunSR.flipY = true;
        }
        else
        {
            gunSR.flipY = false;
        }
    }

    private void PlayerControls()
    {
        ShootBullet();
        BackToMenu();
    }

    private void ShootBullet()
    {
        if (canShoot && Input.GetMouseButtonDown(0))
        {
            bullet = Instantiate(projectile, bulletHole.position, Quaternion.identity);
            shootSound.Play();
            Destroy(bullet, 3f);
        }
    }

    private void PlayerMovement()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(movX * speed, rb.velocity.y);
    }

    private static void BackToMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= damageAmount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}