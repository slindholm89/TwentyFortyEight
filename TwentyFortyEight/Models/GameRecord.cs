namespace TwentyFortyEight.Models {
    public sealed class GameRecord {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Score { get; set; }

        public int HighestTile { get; set; }

        public bool Won { get; set; }

        public DateTimeOffset CompletedAtUtc { get; set; }
    }
}
