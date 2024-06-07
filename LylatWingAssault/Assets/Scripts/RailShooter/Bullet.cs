using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 5f;
    [SerializeField] private float speed = 100f;
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        StartCoroutine(BulletActive());
    }

    IEnumerator BulletActive()
    {
        yield return new WaitForSeconds(bulletLifeTime);
        BreakBullet();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Projectile")) return;
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();

        if(damagable != null)
        {
            damagable.Damage(1);
        }

        BreakBullet();
    }

    private void BreakBullet()
    {
        Destroy(this.gameObject);
    }
}
