namespace TwentyFortyEight.Models {
    public sealed class GameSave {
        public int[] Board { get; set; } = new int[16];

        public int Score { get; set; }

        public bool HasWon { get; set; }

        public bool IsGameOver { get; set; }

        public bool ContinueAfterWin { get; set; }

        public bool IsRecorded { get; set; }

        public DateTimeOffset SavedAtUtc { get; set; }
    }
}
