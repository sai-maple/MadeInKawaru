namespace MadeInKawaru.Entity.Game
{
    public sealed class LifeEntity
    {
        public int Life { get; private set; }
        public bool IsLiving => Life > 0;
        
        public void Initialize()
        {
            Life = 3;
        }
        
        public void OnClear(bool result)
        {
            if (!result) Life--;
        }
    }
}