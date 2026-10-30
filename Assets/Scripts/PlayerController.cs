using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI gameOverText;
    private Vector3 _initialSpawnPosition;
    private Rigidbody rb;

    private int score;
    private byte lives = 3;

    private float movementX;
    private float movementY;
    private float timer;
    public float delayAmount = 0.5f;
    public bool isGameOver;
    public bool isRespawning;
    private Camera mainCamera;
    private PlayerAudio playerAudio;
    private TransformationManager transformationManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerAudio = GetComponent<PlayerAudio>();
        transformationManager = GetComponent<TransformationManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _initialSpawnPosition = transform.position;
        endText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        score = 1000;
        SetScoreText();
        SetLivesText();

        // Set the player in the CheckpointController
        Assets.Scripts.CheckpointController.Instance.SetPlayer(this);
    }

    void FixedUpdate()
    {
        if (isGameOver || isRespawning) return;

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
        if (isGameOver || isRespawning) return;

        timer += Time.deltaTime;

        if (timer >= delayAmount)
        {
            timer -= delayAmount;
            score--;
            SetScoreText();
        }

        if(this.transform.position.y < -10f && !isRespawning)
        {
            playerAudio.PlayFallDownSound();
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;
        lives--;
        SetLivesText();

        if (lives <= 0)
        {
            isGameOver = true;
            gameOverText.gameObject.SetActive(true);

            if (Assets.Scripts.CheckpointController.Instance != null)
            {
                Assets.Scripts.CheckpointController.Instance.ClearCheckpoints();
            }

            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield break;
        }

        yield return new WaitForSeconds(2f);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Restore default player values
        transform.position = _initialSpawnPosition;
        transformationManager.ResetToDefault();
        score = 1000;
        timer = 0f;
        SetScoreText();

        // If there's a checkpoint, the controller will restore the player to that position and score
        if (Assets.Scripts.CheckpointController.Instance != null)
        {
            Assets.Scripts.CheckpointController.Instance.SetPlayer(this);
            Assets.Scripts.CheckpointController.Instance.RestoreObjects();
        }

        isRespawning = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.gameObject.CompareTag("PointExtra"))
        {
            other.gameObject.SetActive(false);
            score += 220;
            SetScoreText();
            playerAudio.PlayExtraPointsSound();
        }
        if (other.gameObject.CompareTag("ExtraLife"))
        {
            other.gameObject.SetActive(false);
            lives++;
            SetLivesText();
            playerAudio.PlayExtraLifeSound();
        }
        if (other.gameObject.CompareTag("Spike"))
        {
            other.gameObject.SetActive(false);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            playerAudio.PlayBreakSound();
            StartCoroutine(Respawn());
        }
        if (other.gameObject.CompareTag("Transformer"))
        {
            other.gameObject.SetActive(false);
            // Get the touched transformer's component and apply the transformation using the TransformationManager
            Transformer transformer = other.GetComponent<Transformer>();
            if (transformer != null && transformationManager != null)
            {
                transformationManager.ApplyTransformation(transformer.TargetForm);
            }
            playerAudio.PlayTransformSound();
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            score += lives * 200; // Bonus points for remaining lives

            isGameOver = true;
            other.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            livesText.gameObject.SetActive(false);
            endText.text += "\nScore: " + score;
            endText.gameObject.SetActive(true);

            // Stop the ball
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transformationManager.ResetToDefault();

            // Restart the level from scratch after 10 seconds
            Assets.Scripts.CheckpointController.Instance.ClearCheckpoints();
            StartCoroutine(restartLevel());
        }
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            other.gameObject.SetActive(false);
            playerAudio.PlayCheckpointSound();
            Assets.Scripts.CheckpointController.Instance.SetCheckpoint(other.transform.position, score, transformationManager.CurrentForm);
        }
    }

    IEnumerator restartLevel()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestoreFromCheckpoint(Vector3 savedPosition, int savedScore, TransformationManager.PlayerMaterialType savedForm)
    {
        this.transform.position = savedPosition;
        this.score = savedScore;
        SetScoreText();
        SetLivesText();
        transformationManager.ApplyTransformation(savedForm);

        // Reset the ball's velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }   
        Debug.Log($"Restored from checkpoint at position: {savedPosition}, score: {savedScore}, form: {savedForm}");
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

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
    }
}
