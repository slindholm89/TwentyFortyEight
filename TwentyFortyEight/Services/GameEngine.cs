using TwentyFortyEight.Models;

namespace TwentyFortyEight.Services {
    public sealed class GameEngine {
        private const int Size = 4;
        private readonly Random _random = new();

        public int[,] Board { get; } = new int[Size, Size];

        public int Score { get; private set; }

        public bool HasWon { get; private set; }

        public bool IsGameOver { get; private set; }

        public void NewGame() {
            Array.Clear(Board);
            Score = 0;
            HasWon = false;
            IsGameOver = false;

            SpawnTile();
            SpawnTile();
        }

        public bool Move(MoveDirection direction) {
            if (IsGameOver)
                return false;

            bool changed = false;

            for (int line = 0; line < Size; line++) {
                int[] values = new int[Size];

                // Read the line in movement direction.
                for (int i = 0; i < Size; i++) {
                    var (row, col) = GetPosition(direction, line, i);
                    values[i] = Board[row, col];
                }

                int[] merged = MergeLine(values);

                // Write the resulting line back.
                for (int i = 0; i < Size; i++) {
                    var (row, col) = GetPosition(direction, line, i);

                    if (Board[row, col] != merged[i])
                        changed = true;

                    Board[row, col] = merged[i];
                }
            }

            if (!changed)
                return false;

            SpawnTile();

            HasWon |= Board.Cast<int>().Any(x => x >= 2048);

            IsGameOver = !CanMove();

            return true;
        }

        private int[] MergeLine(int[] values) {
            // Remove empty cells.
            var numbers = values
                .Where(x => x != 0)
                .ToList();

            var result = new List<int>();

            for (int i = 0; i < numbers.Count; i++) {
                if (i + 1 < numbers.Count &&
                    numbers[i] == numbers[i + 1]) {
                    int merged = numbers[i] * 2;

                    result.Add(merged);
                    Score += merged;

                    i++;
                } else {
                    result.Add(numbers[i]);
                }
            }

            // Fill remaining positions with zero.
            while (result.Count < Size)
                result.Add(0);

            return result.ToArray();
        }

        private static (int row, int col) GetPosition(
            MoveDirection direction,
            int line,
            int position) {
            return direction switch {
                MoveDirection.Left => (line, position),
                MoveDirection.Right => (line, Size - 1 - position),
                MoveDirection.Up => (position, line),
                MoveDirection.Down => (Size - 1 - position, line),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(direction))
            };
        }

        private void SpawnTile() {
            var empty = new List<(int row, int col)>();

            for (int row = 0; row < Size; row++) {
                for (int col = 0; col < Size; col++) {
                    if (Board[row, col] == 0)
                        empty.Add((row, col));
                }
            }

            if (empty.Count == 0)
                return;

            var position = empty[_random.Next(empty.Count)];

            Board[position.row, position.col] =
                _random.Next(10) == 0 ? 4 : 2;
        }

        private bool CanMove() {
            for (int row = 0; row < Size; row++) {
                for (int col = 0; col < Size; col++) {
                    int value = Board[row, col];

                    if (value == 0)
                        return true;

                    if (row < Size - 1 &&
                        value == Board[row + 1, col])
                        return true;

                    if (col < Size - 1 &&
                        value == Board[row, col + 1])
                        return true;
                }
            }

            return false;
        }

        public GameSave ExportState(bool continueAfterWin, bool isRecorded) {
            return new GameSave {
                Board = Board.Cast<int>().ToArray(),
                Score = Score,
                HasWon = HasWon,
                IsGameOver = IsGameOver,
                ContinueAfterWin = continueAfterWin,
                IsRecorded = isRecorded,
                SavedAtUtc = DateTimeOffset.UtcNow
            };
        }

        public void RestoreState(GameSave save) {
            ArgumentNullException.ThrowIfNull(save);

            if (save.Board.Length != Size * Size ||
                save.Board.Any(x => x < 0)) {
                throw new ArgumentException("Invalid saved game board.", nameof(save));
            }

            if (save.Score < 0)
                throw new ArgumentException("Invalid saved game score.", nameof(save));

            for (int row = 0; row < Size; row++) {
                for (int col = 0; col < Size; col++) {
                    Board[row, col] = save.Board[row * Size + col];
                }
            }

            Score = save.Score;
            HasWon = save.HasWon;
            IsGameOver = save.IsGameOver;
        }
    }
}
