namespace Connect4.Models;

public enum Player
{
    None = 0,
    Red = 1,
    Yellow = 2
}

public enum GameState
{
    InProgress,
    RedWins,
    YellowWins,
    Draw
}

public class GameBoard
{
    public const int Rows = 6;
    public const int Columns = 7;

    private readonly Player[,] _cells = new Player[Rows, Columns];

    public Player CurrentPlayer { get; private set; } = Player.Red;
    public GameState State { get; private set; } = GameState.InProgress;
    public IReadOnlyList<(int Row, int Col)> WinningCells { get; private set; } = Array.Empty<(int, int)>();

    public Player GetCell(int row, int col) => _cells[row, col];

    public int? DropDisc(int column)
    {
        if (State != GameState.InProgress || column < 0 || column >= Columns)
            return null;

        for (int row = Rows - 1; row >= 0; row--)
        {
            if (_cells[row, column] == Player.None)
            {
                _cells[row, column] = CurrentPlayer;
                CheckGameEnd(row, column);
                if (State == GameState.InProgress)
                    CurrentPlayer = CurrentPlayer == Player.Red ? Player.Yellow : Player.Red;
                return row;
            }
        }

        return null;
    }

    public void Reset()
    {
        Array.Clear(_cells, 0, _cells.Length);
        CurrentPlayer = Player.Red;
        State = GameState.InProgress;
        WinningCells = Array.Empty<(int, int)>();
    }

    public bool IsColumnFull(int column) =>
        column >= 0 && column < Columns && _cells[0, column] != Player.None;

    private void CheckGameEnd(int row, int col)
    {
        var player = _cells[row, col];

        if (TryFindWin(row, col, player, out var winningCells))
        {
            WinningCells = winningCells;
            State = player == Player.Red ? GameState.RedWins : GameState.YellowWins;
            return;
        }

        if (IsBoardFull())
            State = GameState.Draw;
    }

    private bool IsBoardFull()
    {
        for (int c = 0; c < Columns; c++)
        {
            if (_cells[0, c] == Player.None)
                return false;
        }
        return true;
    }

    private bool TryFindWin(int row, int col, Player player, out List<(int Row, int Col)> cells)
    {
        cells = new List<(int, int)>();

        if (CountInDirection(row, col, player, 0, 1) >= 4 ||
            CountInDirection(row, col, player, 1, 0) >= 4 ||
            CountInDirection(row, col, player, 1, 1) >= 4 ||
            CountInDirection(row, col, player, 1, -1) >= 4)
        {
            CollectWinningLine(row, col, player, cells);
            return cells.Count >= 4;
        }

        return false;
    }

    private int CountInDirection(int row, int col, Player player, int dRow, int dCol)
    {
        int count = 1;
        count += CountOneWay(row, col, player, dRow, dCol);
        count += CountOneWay(row, col, player, -dRow, -dCol);
        return count;
    }

    private int CountOneWay(int row, int col, Player player, int dRow, int dCol)
    {
        int count = 0;
        int r = row + dRow;
        int c = col + dCol;

        while (r >= 0 && r < Rows && c >= 0 && c < Columns && _cells[r, c] == player)
        {
            count++;
            r += dRow;
            c += dCol;
        }

        return count;
    }

    private void CollectWinningLine(int row, int col, Player player, List<(int Row, int Col)> cells)
    {
        TryCollectDirection(row, col, player, 0, 1, cells);
        if (cells.Count >= 4) return;
        cells.Clear();

        TryCollectDirection(row, col, player, 1, 0, cells);
        if (cells.Count >= 4) return;
        cells.Clear();

        TryCollectDirection(row, col, player, 1, 1, cells);
        if (cells.Count >= 4) return;
        cells.Clear();

        TryCollectDirection(row, col, player, 1, -1, cells);
    }

    private void TryCollectDirection(int row, int col, Player player, int dRow, int dCol, List<(int Row, int Col)> cells)
    {
        cells.Add((row, col));

        int r = row + dRow;
        int c = col + dCol;
        while (r >= 0 && r < Rows && c >= 0 && c < Columns && _cells[r, c] == player)
        {
            cells.Add((r, c));
            r += dRow;
            c += dCol;
        }

        r = row - dRow;
        c = col - dCol;
        while (r >= 0 && r < Rows && c >= 0 && c < Columns && _cells[r, c] == player)
        {
            cells.Insert(0, (r, c));
            r -= dRow;
            c -= dCol;
        }
    }
}
