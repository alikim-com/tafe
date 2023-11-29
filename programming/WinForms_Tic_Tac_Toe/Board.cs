using System.Collections;
using Test;

namespace test;

/*
      var Rows = new Line[] { 
        new(brd, new Point[] { new(0,0), new(0,1), new(0,2) }),
        new(brd, new Point[] { new(1,0), new(1,1), new(1,2) }),
        new(brd, new Point[] { new(2,0), new(2,1), new(2,2) }),
      };
      var Cols = new Line[] { 
        new(brd, new Point[] { new(0,0), new(1,0), new(2,0) }),
        new(brd, new Point[] { new(0,1), new(1,1), new(2,1) }),
        new(brd, new Point[] { new(0,2), new(1,2), new(2,2) }),
      };
      var Diag = new Line[] {
        new(brd, new Point[] { new(2,0), new(1,1), new(0,2) }), // Fwd
        new(brd, new Point[] { new(0,0), new(1,1), new(2,2) }), // Bwd
      };

*/

public struct Point
{
   public int row;
   public int col;

   public Point(int _row, int _col) {
      row = _row;
      col = _col;
   }
}

class Board : IEnumerable<Game.Roster>
{

   readonly Game.Roster[] board;

   readonly int width;
   readonly int height;
   public readonly int Length;

   public Board(int _width, int _height, Game.Roster _def)
   {
      width = _width;
      height = _height;
      Length = _width * _height;
      board = new Game.Roster[Length];

      Array.Fill(board, _def);
   }

   public Game.Roster this[int index]
   {
      get
      {
         if (index < 0 || index >= board.Length)
            throw new IndexOutOfRangeException("Board.get[] : index is out of range");

         return board[index];
      }
      set
      {
         if (index < 0 || index >= board.Length)
            throw new IndexOutOfRangeException("Board.set[] : index is out of range");

         board[index] = value;
      }
   }

   public Game.Roster this[int row, int col]
   {
      get
      {
         if (row < 0 || row >= height)
            throw new IndexOutOfRangeException("Board.get[,] : row index is out of range");
         if (col < 0 || col >= width)
            throw new IndexOutOfRangeException("Board.get[,] : column index is out of range");
         var index = row * width + col;
         return board[index];
      }
      set
      {
         if (row < 0 || row >= height)
            throw new IndexOutOfRangeException("Board.get[,] : row index is out of range");
         if (col < 0 || col >= width)
            throw new IndexOutOfRangeException("Board.get[,] : column index is out of range");
         var index = row * width + col;

         board[index] = value;
      }
   }

   public IEnumerator<Game.Roster> GetEnumerator() => new BoardEtor(board);

   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class BoardEtor : IEnumerator<Game.Roster>
{
   readonly Game.Roster[] list;
   int head;

   public BoardEtor(Game.Roster[] _list)
   {
      list = _list;
      head = -1;
   }

   public Game.Roster Current => list[head];

   object IEnumerator.Current => Current;

   public void Dispose() { }

   public bool MoveNext()
   {
      head++;
      return head != list.Length;
   }

   public void Reset() { head = -1; }
}

class Line : IEnumerable<Game.Roster>
{
   public readonly int Length;
   readonly Point[] rc;
   readonly Board board;

   public Line(Board _board, Point[] _rc)
   {
      rc = _rc;
      Length = _rc.Length;
      board = _board;
   }

   public Game.Roster this[int index]
   {
      get => board[rc[index].row, rc[index].col];
      set => board[rc[index].row, rc[index].col] = value;
   }

   public IEnumerator<Game.Roster> GetEnumerator() => new LineEtor(board, rc);

   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class LineEtor : IEnumerator<Game.Roster>
{
   readonly Board list;
   readonly Point[] rc;
   int head;

   public LineEtor(Board _list, Point[] _rc)
   {
      list = _list;
      rc = _rc;
      head = -1;
   }

   public Game.Roster Current => list[rc[head].row, rc[head].col];

   object IEnumerator.Current => Current;

   public void Dispose() { }

   public bool MoveNext()
   {
      head++;
      return head != rc.Length;
   }

   public void Reset() { head = -1; }
}