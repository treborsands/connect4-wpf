using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Connect4.Converters;
using Connect4.Models;
using Connect4.ViewModels;

namespace Connect4;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();
    private readonly PlayerToBrushConverter _brushConverter = new();
    private readonly Ellipse[,] _discs = new Ellipse[GameBoard.Rows, GameBoard.Columns];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
        _viewModel.BoardChanged += (_, _) => RefreshBoard();
        BuildBoard();
        BuildColumnButtons();
    }

    private void BuildColumnButtons()
    {
        for (int col = 0; col < GameBoard.Columns; col++)
        {
            int column = col;
            var button = new Button
            {
                Content = "▼",
                Foreground = Brushes.White,
                FontSize = 16,
                Style = (Style)FindResource("ColumnButtonStyle"),
                Command = _viewModel.ColumnClickCommand,
                CommandParameter = column
            };
            ColumnButtonsPanel.Children.Add(button);
        }
    }

    private void BuildBoard()
    {
        for (int row = 0; row < GameBoard.Rows; row++)
        {
            for (int col = 0; col < GameBoard.Columns; col++)
            {
                var disc = new Ellipse
                {
                    Fill = (Brush)_brushConverter.Convert(Player.None, typeof(Brush), null!, System.Globalization.CultureInfo.CurrentCulture)!,
                    Margin = new Thickness(4),
                    Stroke = Brushes.Transparent,
                    StrokeThickness = 3
                };

                _discs[row, col] = disc;
                BoardPanel.Children.Add(disc);
            }
        }
    }

    private void RefreshBoard()
    {
        for (int row = 0; row < GameBoard.Rows; row++)
        {
            for (int col = 0; col < GameBoard.Columns; col++)
            {
                var player = _viewModel.GetCell(row, col);
                var disc = _discs[row, col];
                disc.Fill = (Brush)_brushConverter.Convert(player, typeof(Brush), null!, System.Globalization.CultureInfo.CurrentCulture)!;
                disc.Stroke = _viewModel.IsWinningCell(row, col) ? Brushes.White : Brushes.Transparent;
            }
        }
    }
}
