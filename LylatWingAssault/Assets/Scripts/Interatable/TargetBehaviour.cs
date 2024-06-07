using UnityEngine;

public class TargetBehaviour : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private Renderer render;
    [SerializeField] private Material inActive;

    [Header("Variables")]
    public bool isBroken = false; 
    [SerializeField] private int health;
    private int _currentHealth;


    public int HitPoints
    {
        get => health;
        set => _currentHealth = value;
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0) BreakTarget();
    }

    private void BreakTarget() 
    {
        isBroken = true;
        render.material = inActive;
    }
}