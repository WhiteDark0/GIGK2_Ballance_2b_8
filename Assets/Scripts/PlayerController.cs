using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

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

        // Get main camera's direction
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        // Ignore y
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * movementY + camRight * movementX);
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

            // Restart the level after 3 seconds
            StartCoroutine(restartLevel());
        }
    }

    IEnumerator restartLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
