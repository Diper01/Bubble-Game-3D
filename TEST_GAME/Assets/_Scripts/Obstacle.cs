using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool _canInfect;
    public void DestroyObstacle()
    {
        GetComponent<Collider>().enabled = false;
        _canInfect = true;
        StartCoroutine(Infection());
    }

    private IEnumerator Infection()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.yellow;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 8 && _canInfect)
        {
            Obstacle otherObstacle = other.GetComponent<Obstacle>();
            if (otherObstacle != null)
            {
                otherObstacle.DestroyObstacle();
            }
        }
    }
}