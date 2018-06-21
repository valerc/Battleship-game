using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    enum Direction
    {
        Vertical,
        Horizontal
    }

    struct Cell
    {
        public int row;
        public int column;

        public Cell(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    class Battleship
    {
        readonly int rowQty, columnQty;
        readonly int[,] fleet;

        bool[,] field;
        Cell[] freeCells;

        Random random = new Random((int)DateTime.Now.Ticks);

        public Battleship()
           : this(10, 10, new int[,]
            {
                { 4, 1 },
                { 3, 2 },
                { 2, 3 },
                { 1, 4 }
            })
        { }

        protected Battleship(int rowQty, int columnQty, int[,] fleet)
        {
            this.rowQty = rowQty;
            this.columnQty = columnQty;
            this.fleet = fleet;
        }

        public virtual void DrawBattlefield()
        {
            FillField();

            Console.Clear();

            Console.Write("   ");
            for (int i = 0; i < field.GetLength(1); i++)
            {
                Console.Write("{0, 2}", (char)(i + 65));
            }
            Console.WriteLine();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write("{0, 2}|", i + 1);
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    Console.Write("{0, 2}", field[i, j] ? "O" : "-");
                }
                Console.WriteLine();
            }
        }

        private void FillField()
        {
            field = new bool[rowQty, columnQty];

            for (int i = 0; i < fleet.GetLength(0); i++)
            {
                var shipLength = fleet[i, 0];
                for (int j = 0; j < fleet[i, 1]; j++)
                {
                    Direction direction = (Direction)random.Next(2);

                    // Here you can add code for the case when there isn't any free space left: 
                    // change direction and (if not helped) goto to start of the metod.
                    freeCells = ScanField(shipLength, direction);

                    Cell newShipPosition = freeCells[random.Next(freeCells.Length)];

                    PlaceShip(newShipPosition, shipLength, direction);
                }
            }
        }

        private void PlaceShip(Cell place, int shipLength, Direction direction)
        {
            if (direction == Direction.Horizontal)
            {
                for (int i = place.column; i < place.column + shipLength; i++)
                {
                    field[place.row, i] = true;
                }
            }
            else
            {
                for (int i = place.row; i < place.row + shipLength; i++)
                {
                    field[i, place.column] = true;
                }
            }
        }

        private Cell[] ScanField(int shipLength, Direction direction)
        {
            List<Cell> list = new List<Cell>();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (IsMatch(i, j, shipLength, direction))
                        list.Add(new Cell(i, j));
                }
            }
            return list.ToArray();
        }

        private bool IsMatch(int row, int column, int shipLength, Direction direction)
        {
            int rowMin = row - 1 < 0 ? 0 : row - 1;
            int columnMin = column - 1 < 0 ? 0 : column - 1;
            int rowMax, columnMax;


            if (direction == Direction.Horizontal)
            {
                if (field.GetLength(1) - columnMin <= shipLength)
                    return false;

                rowMax = row + 1 >= field.GetLength(0) ? field.GetLength(0) - 1 : row + 1;
                columnMax = column + shipLength >= field.GetLength(1) ? field.GetLength(1) - 1 : column + shipLength;
            }
            else
            {
                if (field.GetLength(0) - rowMin <= shipLength)
                    return false;

                rowMax = row + shipLength >= field.GetLength(0) ? field.GetLength(0) - 1 : row + shipLength;
                columnMax = column + 1 >= field.GetLength(1) ? field.GetLength(1) - 1 : column + 1;
            }

            for (int i = rowMin; i <= rowMax; i++)
            {
                for (int j = columnMin; j <= columnMax; j++)
                {
                    if (field[i, j])
                        return false;
                }
            }
            return true;
        }
    }
}
