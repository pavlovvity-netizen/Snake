using Snake.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.ViewModels
{
    class CellVM : BindableBase
    {
        public int Row { get; }

        public int Column { get; }

        private CellType _cellType = CellType.None;

        public CellType CellType
        {
            get => _cellType; 
            set
            {
                _cellType = value;
                RaisePropertyChanged(nameof(CellType));
            }
        }

        public CellVM(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
