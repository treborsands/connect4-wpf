using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Connect4.Models;

namespace Connect4.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly GameBoard _board = new();
    private string _statusText = "Red's turn";
    private Brush _currentPlayerBrush = Brushes.Red;

    public MainViewModel()
    {
        NewGameCommand = new RelayCommand(_ => ResetGame());
        ColumnClickCommand = new RelayCommand(OnColumnClick, CanDropInColumn);
    }

    public ICommand NewGameCommand { get; }
    public ICommand ColumnClickCommand { get; }

    public string StatusText
    {
        get => _statusText;
        private set { _statusText = value; OnPropertyChanged(); }
    }

    public Brush CurrentPlayerBrush
    {
        get => _currentPlayerBrush;
        private set { _currentPlayerBrush = value; OnPropertyChanged(); }
    }

    public bool IsGameOver => _board.State != GameState.InProgress;

    public Player GetCell(int row, int col) => _board.GetCell(row, col);

    public bool IsWinningCell(int row, int col) =>
        _board.WinningCells.Any(c => c.Row == row && c.Col == col);

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? BoardChanged;

    private void OnColumnClick(object? parameter)
    {
        if (parameter is not int column)
            return;

        if (_board.DropDisc(column) is null)
            return;

        BoardChanged?.Invoke(this, EventArgs.Empty);
        UpdateStatus();
    }

    private bool CanDropInColumn(object? parameter)
    {
        if (parameter is not int column)
            return false;

        return _board.State == GameState.InProgress && !_board.IsColumnFull(column);
    }

    private void ResetGame()
    {
        _board.Reset();
        BoardChanged?.Invoke(this, EventArgs.Empty);
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        StatusText = _board.State switch
        {
            GameState.InProgress => _board.CurrentPlayer == Player.Red ? "Red's turn" : "Yellow's turn",
            GameState.RedWins => "Red wins!",
            GameState.YellowWins => "Yellow wins!",
            GameState.Draw => "It's a draw!",
            _ => string.Empty
        };

        CurrentPlayerBrush = _board.State switch
        {
            GameState.InProgress when _board.CurrentPlayer == Player.Red => Brushes.Red,
            GameState.InProgress => Brushes.Gold,
            GameState.RedWins => Brushes.Red,
            GameState.YellowWins => Brushes.Gold,
            _ => Brushes.White
        };

        OnPropertyChanged(nameof(IsGameOver));
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);
}
