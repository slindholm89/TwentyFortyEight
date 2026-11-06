namespace TwentyFortyEight.Models {
    public sealed class GameStorageData {
        public GameSave? ActiveGame { get; set; }

        public int BestScore { get; set; }

        public List<GameRecord> History { get; set; } = [];
    }
}
