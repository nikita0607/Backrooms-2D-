namespace BHSCamp
{
    //интерфейс для взаимодействиями с объектами, которым можно нанести урон
    public interface IDamageable
    {
        int TakeDamage(int amount);
    }
}