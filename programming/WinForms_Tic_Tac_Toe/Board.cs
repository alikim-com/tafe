using System.Collections;
using Test;

namespace test;

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
         var index = row * width + col;
         if (index < 0 || index >= board.Length)
            throw new IndexOutOfRangeException("Board.get[] : index is out of range");

         return board[index];
      }
      set
      {
         var index = row * width + col;
         if (index < 0 || index >= board.Length)
            throw new IndexOutOfRangeException("Board.set[] : index is out of range");

         board[index] = value;
      }
   }

   public IEnumerator<Game.Roster> GetEnumerator() => new LineEtor(board);

   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}



class LineEtor : IEnumerator<Game.Roster>
{
   readonly Game.Roster[] list;
   int head;

   public LineEtor(Game.Roster[] _list)
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