namespace XFlow.Gold
{
    public class GoldResource
    {
        private const int DEFAULT_CURRENT_GOLD = 100;
        public int CurrentGold { get; set; }

        public GoldResource()
        {
            CurrentGold = DEFAULT_CURRENT_GOLD;
        }
    }
}
