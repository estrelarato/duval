using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerMov : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D colliderPlayer;
    private float moveX;

    public float speed;
    public int addJumps;
    public bool isGrounded;
    public float jumpForce;
    public int life;
    public int maxLife;  // Vida máxima
    public TextMeshProUGUI textLife;
    public string levelName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderPlayer = GetComponent<CapsuleCollider2D>();

        // Inicializa a vida com o valor máximo ao começar o jogo
        life = maxLife;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        textLife.text = life.ToString();

        // Verifica se a vida chegou a 0
        if (life <= 0)
        {
            this.enabled = false;
            colliderPlayer.enabled = false;
            rb.gravityScale = 0;
            anim.Play("DeathP", -1);
            SceneManager.LoadScene(levelName);
        }
    }

    void FixedUpdate()
    {
        Move();
        Slash();

        if (isGrounded)
        {
            addJumps = 1;
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && addJumps > 0)
            {
                addJumps--;
                Jump();
            }
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        if (moveX > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetBool("isRun", true);
        }
        else if (moveX < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetBool("isJump", true);
    }

    void Slash()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.Play("SlashP", -1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solo"))
        {
            isGrounded = true;
            anim.SetBool("isJump", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solo"))
        {
            isGrounded = false;
        }
    }

    // Função para adicionar vida (não ultrapassando o máximo)
    public void AddLife(int value)
    {
        life += value;
        if (life > maxLife) life = maxLife;  // Garante que não ultrapasse a vida máxima
    }

    // Função para tirar vida (não deixando a vida negativa)
    public void TakeDamage(int value)
    {
        life -= value;
        if (life < 0) life = 0;  // Garante que a vida não fique negativa
    }
}
