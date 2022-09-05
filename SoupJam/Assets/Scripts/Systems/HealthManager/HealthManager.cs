using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public enum HealthBarMode {
        None, Slider, Transform
    }

    [SerializeField] private float health;
    private float maxHealth;

    [Header("Settings")]
    [Tooltip("Determines how health is displayed.\n\n" +
        "None: health will not be displayed.\n" +
        "Slider: target slider will be used.\n" +
        "Transform: transform will be scaled.")]
    [SerializeField] private HealthBarMode healthBarMode;

    [Space(10)]
    [Tooltip("When true, triggers onHit event when onDeath is triggered")]
    [SerializeField] private bool hitOnDeath = false;
    [Tooltip("when true, allows healing through taking negative damage")]
    [SerializeField] private bool allowNegDamage = false;
    [Tooltip("when true, allows taking damage through negative healing")]
    [SerializeField] private bool allowNegHeal = false;

    [Space(10)]
    [Tooltip("Only used when health bar mode is 'Slider'")]
    [SerializeField] private Slider targetSlider;
    [Tooltip("Only used when health bar mode is 'Transform'")]
    [SerializeField] private Transform targetTransform;

    [Header("Events")]
    public UnityEvent onHit;
    public UnityEvent onDeath;

    public UnityEvent onHeal;

    //vars
    private float startSize; //used for transform healthbar mode

    private void Start()
    {
        maxHealth = health;
        if (hitOnDeath) { //call on hit when on death is called
            onDeath.AddListener(() => onHit?.Invoke());
        }
        //check external components
        switch (healthBarMode) {
            case HealthBarMode.Slider:
                if (targetSlider == null) { Debug.LogError("No healthbar slider was set on " + transform.name + "!"); }
                targetSlider.minValue = 0f;
                targetSlider.maxValue = 1f;
                targetSlider.value = 1f;
                break;

            case HealthBarMode.Transform:
                if (targetTransform == null) { Debug.LogError("No healthbar tarnsform was set on " + transform.name +"!"); }
                startSize = targetTransform.localScale.x;
                break;
        }
    }

    //-------manage health-------
    public void TakeDamage(float damage)
    {
        if (!allowNegDamage && damage < 0f) { return; } //neg damage check
        //take damage
        health -= damage;
        HandleHealthChange(damage > 0f);
        //health bar
        UpdateHealthBar();
    }

    public void Heal(float toHeal)
    {
        if (!allowNegHeal && toHeal < 0f) { return; } //neg heal check
        //heal
        health += toHeal;
        HandleHealthChange(toHeal < 0f);
        //health bar
        UpdateHealthBar();
    }

    private void HandleHealthChange(bool tookDamage)
    {
        //handle health
        bool died = health <= 0f;
        health = Mathf.Clamp(health, 0f, maxHealth);
        //call events
        if (tookDamage) {
            if (died) { onDeath?.Invoke(); }
            else { onHit?.Invoke(); }
        }
        else { onHeal?.Invoke(); }
    }

    //-----------manage health bar------------
    private void UpdateHealthBar()
    {
        switch (healthBarMode) {
            case HealthBarMode.Slider:
                targetSlider.value = (health / maxHealth);
                break;

            case HealthBarMode.Transform:
                targetTransform.localScale = new Vector3((health / maxHealth) * startSize, targetTransform.localScale.y, targetTransform.localScale.z);
                break;
        }
    }
}
