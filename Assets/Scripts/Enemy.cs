using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public int pointValue = 10;

    // Bottom of screen kill zone (world Y)
    private float killY = -6f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < killY)
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        GameManager.Instance.AddScore(pointValue);
        Destroy(gameObject);
    }

    // Backup: destroy on projectile collision via trigger (handled in Projectile too)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Die();
        }
    }
}