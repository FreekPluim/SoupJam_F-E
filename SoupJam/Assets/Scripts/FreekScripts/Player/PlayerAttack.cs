using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Camera camera;

    [SerializeField] GameObject fireballPrefab;
    [SerializeField] LayerMask aimable;

    Vector3 hitPos;

    [SerializeField] Animator animator;
    [SerializeField] float AttackTimer = 0.8f;

    bool canAttack = true;
    [SerializeField] float attackSpeed;

    public void Attack()
    {
        if (canAttack)
        {
            StartCoroutine(Animation());
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimable))
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Debug.DrawRay(transform.position, direction, Color.green, Mathf.Infinity);

                hitPos = hit.point;

                GameObject newFireball = Instantiate(fireballPrefab, transform.position, transform.rotation);
                newFireball.GetComponent<Mover>().moveSpeed = new Vector3(direction.x, 0.1f, direction.z) * attackSpeed;
            }
        }
    }

    IEnumerator Animation()
    {
        canAttack = false;
        animator.SetBool("Attack", true);
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
