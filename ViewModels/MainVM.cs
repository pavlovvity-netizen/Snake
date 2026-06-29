using Snake.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Snake.ViewModels
{
	class MainVM : BindableBase
	{
		private bool _continueGame;

		public bool ContinueGame
		{
			get => _continueGame;
			private set
			{
				_continueGame = value;
				RaisePropertyChanged(nameof(ContinueGame));

				if (_continueGame) SnakeGo();
            }
		}

		public List<List<CellVM>> AllCells { get; } = new List<List<CellVM>>();

		public DelegateCommand StartStopCommand { get; }

		private MoveDirection _currentMoveDirection = MoveDirection.Right;

		private int _rowCount = 10;
		private int _columnCount = 10;

		private SnakeModel _snake;

		private MainWindow _mainWindow;

        private CellVM _lastFood;

        private int _speed = 300;

        public MainVM(MainWindow mainWindow)
        {
            StartStopCommand = new DelegateCommand(StartStop);

            _mainWindow = mainWindow;

            for (int row = 0; row < _rowCount; row++)
            {
                var rowList = new List<CellVM>();
                for (int column = 0; column < _columnCount; column++)
                {
                    var cell = new CellVM(row, column);
					rowList.Add(cell);
                }

                AllCells.Add(rowList);
            }

            _snake = new SnakeModel(AllCells, AllCells[_rowCount / 2][_columnCount / 2], GenerateFood);

			_mainWindow.KeyDown += MainWindow_KeyDown;

            GenerateFood();
        }

        private void StartStop()
		{
            ContinueGame = !ContinueGame;
        }

		private async Task SnakeGo()
		{
			while (ContinueGame)
			{
                await Task.Delay(_speed);

				try
				{
					_snake.Move(_currentMoveDirection);

				}
				catch (Exception ex)
				{
					ContinueGame = false;
					MessageBox.Show(ex.Message, "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                    _snake.Restart();
                    _lastFood.CellType = CellType.None;
                    GenerateFood();
                }
            }
		}

		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    if (_currentMoveDirection != MoveDirection.Down)
                        _currentMoveDirection = MoveDirection.Up;
                    break;
                case Key.S:
                    if (_currentMoveDirection != MoveDirection.Up)
                        _currentMoveDirection = MoveDirection.Down;
                    break;
                case Key.A:
                    if (_currentMoveDirection != MoveDirection.Right)
                        _currentMoveDirection = MoveDirection.Left;
                    break;
                case Key.D:
                    if (_currentMoveDirection != MoveDirection.Left)
                        _currentMoveDirection = MoveDirection.Right;
                    break;
                default:
                    break;
            }
        }

        private void GenerateFood()
        {
            var Random = new Random();

            int row = Random.Next(0, _rowCount);
            int column = Random.Next(0, _columnCount);

            _lastFood = AllCells[row][column];

            if (_snake.SnakeCells.Contains(_lastFood))
            {
                GenerateFood();
            }

            _lastFood.CellType = CellType.Food;
        }
    }
}