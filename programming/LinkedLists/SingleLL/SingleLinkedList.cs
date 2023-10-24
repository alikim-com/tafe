#define VERBOSE

using System.Collections;

/// <summary>
/// Single Link List 
/// </summary>
/// <typeparam name="T"></typeparam>
class SLL<T> : IEnumerable<SLNode<T>>
{
   // array size management
   int size = 0;
   readonly int initSize = 16;
   int currentCapacity = 0;

   public SLNode<T>? firstNode; // to bootstrap enumtor.Current after Reset
   SLNode<T>? lastNode; // to link new nodes
   SLNode<T>?[] nodes;

   public SLL() => nodes = Array.Empty<SLNode<T>?>();

   public SLL(int _size)
   {
      size = _size;
      nodes = new SLNode<T>[_size];
   }

   public IEnumerator<SLNode<T>> GetEnumerator()
   {
      return new SLLEnum<T>(this);
   }

   IEnumerator IEnumerable.GetEnumerator()
   {
      return GetEnumerator();
   }

   SLNode<T> SetCurrentByIndex(int index)
   {
      if (index < 0)
         throw new CustomException("SLL SetCurrentByIndex : negative index", new IndexOutOfRangeException());

      var enmtor = GetEnumerator();
      enmtor.Reset();

      int cnt = -1;
      while (cnt < index && enmtor.MoveNext()) cnt++;
      if (cnt != index)
         throw new CustomException("SLL SetCurrentByIndex : MoveNext falsed", new IndexOutOfRangeException());

      return enmtor.Current;   
   }

   /// <summary>
   /// Follows .Child node chain
   /// </summary>
   /// <param name="index">How many times to follow</param>
   /// <returns></returns>
   public T? this[int index]
   {
      get => SetCurrentByIndex(index).Data;
      set => SetCurrentByIndex(index).Data = value;
   }

   void AssertCapacity()
   {
      if (currentCapacity == size)
      {
         int newSize = (size > 0 ? size : initSize) * 2;
         SLNode<T>?[] newNodes = new SLNode<T>[newSize];
         Array.Copy(nodes, newNodes, size);
         nodes = newNodes;
         size = newSize;

#if VERBOSE
         cwl("SLL.AssertCapacity : {size} -> {newSize}");
#endif
         return;
      }

      if (currentCapacity > size)
         throw new CustomException("SLL.AssertCapacity : {currentCapacity} > {size}");
   }

   bool FindFreePosition(out int index)
   {
      for (int i = 0; i < nodes.Length; i++)
         if (nodes[i] == null)
         {
            index = i;
            return true;
         }

#if VERBOSE
      cwl("SLL.FindFreePosition : no free position found");
#endif
      index = -1;
      return false;
   }

   public void Add(SLNode<T> node)
   {
      AssertCapacity();
      if (!FindFreePosition(out int index))
         throw new CustomException("SLL.FindFreePosition : no free position found");

      nodes[index] = node;  
      
      firstNode ??= node;
      
      if(lastNode != null) lastNode.Child = node;
      lastNode = node;

      currentCapacity++;
   }
}

class SLLEnum<T> : IEnumerator<SLNode<T>>
{
   readonly SLL<T> list;
   SLNode<T>? _current;

   public SLLEnum(SLL<T> _list)
   {
      list = _list;
   }

   public SLNode<T> Current {
      get
      {
         if (_current == null) throw new CustomException("SLL.Current : access of null state");
         return _current;
      }
   }

   object IEnumerator.Current => Current;

   public void Reset() => _current = null;

   public bool MoveNext()
   {
      _current = _current == null ? list.firstNode : _current.Child;
      return _current == null;
   }

   public void Dispose()
   {
      throw new CustomException("SLLEnum.Dispose : not implemented", new NotImplementedException());
   }
}