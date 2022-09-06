using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] HealthManager HealthBar, RageBar;

    [Header("Rage Bar Variables")]
    [SerializeField] int rageBarTimer;
    [SerializeField] int rageBarDecrease;

    private void Start()
    {
        if (RageBar == null)
            Debug.Log("No ragebar was added, Please add RageBarScript!! <3");
        else StartCoroutine(RageBarTimer());
    }


    IEnumerator RageBarTimer()
    {
        yield return new WaitForSeconds(rageBarTimer);
        RageBar.TakeDamage(rageBarDecrease);
    }
}
