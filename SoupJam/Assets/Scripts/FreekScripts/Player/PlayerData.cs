using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    [SerializeField] HealthManager RageBar;

    [Header("Rage Bar Variables")]
    public int rageBarTimer;
    [SerializeField] int rageBarDecrease;

    private void Start()
    {
        if (instance == null) instance = this;
        else { Destroy(this.gameObject); }

        if (RageBar == null)
            Debug.Log("No ragebar was added, Please add RageBarScript!! <3");
        else StartCoroutine(RageBarTimer());
    }


    public void GainRage(int amount)
    {
        RageBar.Heal(amount);
    }

    IEnumerator RageBarTimer()
    {
        yield return new WaitForSeconds(rageBarTimer);
        RageBar.TakeDamage(rageBarDecrease);
        StartCoroutine(RageBarTimer());
    }
}
