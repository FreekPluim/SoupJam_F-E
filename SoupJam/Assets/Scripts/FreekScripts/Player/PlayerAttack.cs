using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Camera camera;

    [SerializeField] GameObject fireballPrefab;
    [SerializeField] LayerMask aimable;

    [SerializeField] Transform leftSpawn, rightSpawn;

    Vector3 hitPos;

    [SerializeField] Animator animator;
    [SerializeField] float AttackTimer = 0.8f;

    bool canAttack = true;
    [SerializeField] float attackSpeed;

    [SerializeField] SpriteRenderer spriteRenderer;

    public void Attack()
    {
        if (canAttack)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimable))
            {
                StartCoroutine(Animation(hit.point.x));
                Vector3 direction;
                GameObject newFireball;
                if (hitPos.x < transform.position.x)
                {
                    direction = (hit.point - leftSpawn.position).normalized;
                    newFireball = Instantiate(fireballPrefab, leftSpawn.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));
                }
                else
                {
                    direction = (hit.point - rightSpawn.position).normalized;
                    newFireball = Instantiate(fireballPrefab, rightSpawn.position, transform.rotation);
                }

                //newFireball.transform.LookAt(new Vector3(hitPos.x, newFireball.transform.position.y, hitPos.z));
                newFireball.GetComponent<Mover>().moveSpeed = new Vector3(direction.x, 0.1f, direction.z) * attackSpeed;
            }
        }
    }

    IEnumerator Animation(float hitPointX)
    {
        canAttack = false;
        animator.SetBool("Attack", true);
        Debug.Log("MouseHit: " + hitPointX + " PlayerX: " + transform.position.x);
        if (hitPointX < transform.position.x)
        {
            spriteRenderer.flipX = true;
        } else spriteRenderer.flipX = false;

        yield return new WaitForSeconds(AttackTimer);

        animator.SetBool("Attack", false);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hitPos, 0.5f);
    }
}
