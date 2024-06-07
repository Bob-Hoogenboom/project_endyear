using UnityEngine;
using UnityEngine.Events;

public class TargetBehaviour : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private Renderer render;
    [SerializeField] private Material inActive;

    [Header("Variables")]
    public bool isBroken = false; 
    [SerializeField] private int _currentHealth;

    [Header("Events")]
    public UnityEvent onHit;


    public int HitPoints
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth == 0) BreakTarget();
    }

    private void BreakTarget() 
    {
        onHit.Invoke();
        isBroken = true;
        render.material = inActive;
    }
}