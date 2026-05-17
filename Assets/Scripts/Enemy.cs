using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public int pointValue = 10;

    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;

    private float killY = -16f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < killY)
        {
            GameManager.Instance.LoseLife();
            StartCoroutine(GameManager.Instance.ShakeCamera());
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        // Spawn hit effect
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        GameManager.Instance.AddScore(pointValue);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
            Die();
    }
}