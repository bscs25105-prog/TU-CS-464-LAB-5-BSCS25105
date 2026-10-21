using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI bonusText;
    public GameObject winTextObject;
    public GameObject redCoinPrefab;
    public GameObject blueCoinPrefab;
    public float spawnRadius = 6f;

    Rigidbody rb;
    float movementX, movementY;
    int count = 0;
    int bonusCount = 0;
    int redTriggerScore;     
    bool redSpawned = false; 
    int totalPickups;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        redTriggerScore = Random.Range(1, 11);  
        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;
        bonusText.gameObject.SetActive(false);  
        winTextObject.SetActive(false);
        SetCountText();
    }

    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        movementX = v.x;
        movementY = v.y;
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(movementX, 0, movementY) * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            CheckRedCoinSpawn();
        }
        else if (other.CompareTag("RedCoin"))
        {
            Destroy(other.gameObject);
            SpawnBlueCoins();
        }
        else if (other.CompareTag("BlueCoin"))
        {
            Destroy(other.gameObject);
            bonusCount++;
            bonusText.gameObject.SetActive(true);
            bonusText.text = "BonusCoins: " + bonusCount + "/50";
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + count;
        if (count >= totalPickups) winTextObject.SetActive(true);
    }

    void CheckRedCoinSpawn()
    {
        if (!redSpawned && count >= redTriggerScore)
        {
            redSpawned = true;
            Vector3 pos = new Vector3(Random.Range(-12f, 12f), 0.5f, Random.Range(-12f, 12f));
            Instantiate(redCoinPrefab, pos, Quaternion.identity);
        }
    }

    void SpawnBlueCoins()
    {
        int total = 50;
        for (int i = 0; i < total; i++)
        {
            float angle = i * Mathf.PI * 2f / total;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;
            Vector3 pos = transform.position + offset;
            pos.y = 0.5f;
            Instantiate(blueCoinPrefab, pos, Quaternion.identity);
        }
    }
}