using Snake.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Models
{
    internal class SnakeModel
    {
        public Queue<CellVM> SnakeCells { get; } = new Queue<CellVM>();

        private List<List<CellVM>> _allCells;

        private CellVM _start;

        private Action _generateFood;

        public SnakeModel(List<List<CellVM>> allCells, CellVM start, Action generateFood)
        {
            SnakeCells.Enqueue(start);
            _allCells = allCells;
            _start = start;
            _start.CellType = CellType.Snake;
            _generateFood = generateFood;
        }

        public void Restart()
        {
            foreach (var cell in SnakeCells)
            {
                cell.CellType = CellType.None;
            }
            SnakeCells.Clear();
            SnakeCells.Enqueue(_start);
            _start.CellType = CellType.Snake;
        }

        public void Move(MoveDirection direction)
        {
            var liderCell = SnakeCells.Last();

            int nextRow = liderCell.Row;
            int nextColumn = liderCell.Column;

            switch (direction) 
            {
                case MoveDirection.Up:
                    nextRow--;
                    break;
                case MoveDirection.Down:
                    nextRow++;
                    break;
                case MoveDirection.Left:
                    nextColumn--;
                    break;
                case MoveDirection.Right:
                    nextColumn++;
                    break;
                default:
                    break;
            }

            try
            {
                var nextCell = _allCells[nextRow][nextColumn];
                switch (nextCell?.CellType)
                {
                    case CellType.None:
                        var tailCell = SnakeCells.Dequeue();
                        tailCell.CellType = CellType.None;
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Enqueue(nextCell);
                        break;
                    case CellType.Food:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Enqueue(nextCell);
                        _generateFood?.Invoke();
                        break;
                    default:
                        throw new Exception("Game over");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Game over");
            }
        }
    }
}
