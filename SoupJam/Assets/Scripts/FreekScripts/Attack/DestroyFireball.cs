using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFireball : MonoBehaviour
{
    [SerializeField] float destroyAfterSeconds = 1;

    private void Start()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);
        if(this.gameObject != null) Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.tag == "Enemy")
        {
            PlayerData.instance.GainRage(other.transform.parent.GetComponent<EnemyBehaviour>().SO.rageGain);
            Destroy(other.transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}
