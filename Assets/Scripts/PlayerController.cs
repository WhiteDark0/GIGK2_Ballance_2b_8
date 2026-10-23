using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endText;
    private Rigidbody rb;
    private int score;
    private float movementX;
    private float movementY;
    private float timer;
    public float delayAmount = 0.5f;
    public bool isGameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endText.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        score = 1000;
        SetScoreText();
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void Update()
    {
        if (isGameOver) return;

        timer += Time.deltaTime;

        if (timer >= delayAmount)
        {
            timer -= delayAmount;
            score--;
            SetScoreText();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.gameObject.CompareTag("PointExtra"))
        {
            other.gameObject.SetActive(false);
            score += 220;
            SetScoreText();
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            isGameOver = true;
            other.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            endText.text += "\nScore: " + score;
            endText.gameObject.SetActive(true);

            // Stop the ball
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
