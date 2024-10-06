public class PotionManager : ResourceBase<PotionManager>
{
    public void AddPotion(int amount)
    {
        AddResource(amount);
    }

    public void UsePotion(int amount)
    {
        UseResource(amount);
    }
    
    public int GetCurrentPotions()
    {
        return GetCurrentResource();
    }
}
