using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float explosionRadius;
    private Vector3 targetPosition;

    public void Initialize(Vector3 target, float size)
    {
        targetPosition = target;
        transform.localScale = new Vector3(size, size, size);
        explosionRadius = size;
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Obstacle obstacle = nearbyObject.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.DestroyObstacle();
            }
        }
        Destroy(gameObject);
    }
}