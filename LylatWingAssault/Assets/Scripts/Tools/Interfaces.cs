public interface IDamagable
{
    public int HitPoints { get; set; }
    public void Damage(int amount);
}
