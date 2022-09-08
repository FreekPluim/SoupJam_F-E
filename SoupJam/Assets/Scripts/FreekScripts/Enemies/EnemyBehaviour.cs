using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Enemy SO;

    Vector2 randomDirection;
    float speed;

    private void Start()
    {
        randomDirection = Random.insideUnitCircle;
        randomDirection.Normalize();

        speed = Random.Range(0.5f, 5);
    }

    private void Update()
    {
        GetComponent<Mover>().moveSpeed = new Vector3(randomDirection.x, 0, randomDirection.y) * speed;
    }
}
