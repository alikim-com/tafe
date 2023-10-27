#define VERBOSE
#define DEBUG

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

   public SLNode<T>? firstNode; // to bootstrap enumtor.Current after enumtor.Reset
   SLNode<T>? lastNode; // to link new nodes to
   SLNode<T>?[] nodes; // container to hold nodes

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

   /// <summary>
   /// Follows .Child node chain, main usage - SLL bracket notation
   /// </summary>
   /// <param name="index">How many times to follow</param>
   /// <returns></returns>
   public SLNode<T> GetNodeByIndex(int index)
   {
      if (index < 0)
         throw new CustomException("SLL.GetNodeByIndex : negative index", new IndexOutOfRangeException());

      var enmtor = GetEnumerator();
      enmtor.Reset();

      int cnt = -1;
      while (cnt < index && enmtor.MoveNext()) cnt++;
      if (cnt != index)
         throw new CustomException($"SLL.GetNodeByIndex : MoveNext falsed before reaching index {index}", new IndexOutOfRangeException());

      return enmtor.Current;
   }

   /// <summary>
   /// If nodes max capacity reached,
   /// creates a new container with double the capacity and copies the nodes into it.
   /// </summary>
   /// <exception cref="CustomException"></exception>
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

   /// <summary>
   /// Finds the first empty (null) cell in nodes array
   /// </summary>
   /// <param name="index">index in nodes array to place a node at</param>
   /// <returns>
   /// true - cell found<br/>
   /// false - cell not found
   /// </returns>
   bool FindFreePosition(out int index)
   {
      for (int i = 0; i < nodes.Length; i++)
      {
         if (nodes[i] == null)
         {
            index = i;
            return true;
         }
      }

      index = -1;
      return false;
   }


   /******************* CUSTOM METHODS *******************/

   /// <summary>
   /// Bracket notation
   /// </summary>
   /// <param name="index">Node chain index</param>
   /// <returns></returns>
   public T? this[int index]
   {
      get => GetNodeByIndex(index).Data;
      set
      {
         if (value == null)
         {
            cwl("SLL.[] : use RemoveAt() to remove a node");
            return;
         }
         GetNodeByIndex(index).Data = value;
      }
   }

   public T? Get(int index) => GetNodeByIndex(index).Data;

   /// <summary>
   /// Inserts a node into the chain and reconnects parent/child, if nedeed
   /// </summary>
   /// <param name="parent">
   /// A parent after which to insert node<br/>
   /// null parent indicates either prepending or adding to the empty list
   /// </param>
   /// <param name="node">A node to insert after parent</param>
   public void InsertAfter(SLNode<T>? parent, SLNode<T> node)
   {
      AssertCapacity();
      if (!FindFreePosition(out int nodesIndex))
         throw new CustomException("SLL.FindFreePosition : no free position found");

      nodes[nodesIndex] = node;
      node.position = nodesIndex;

      if (parent != null)
      {
         node.Child = parent.Child;
         parent.Child = node;
      }
      else {
         // works for prepend and empty list
         node.Child = firstNode;
         firstNode = node;
      }
      
      // works for empty list as well, null = null
      if (parent == lastNode) lastNode = node; 

      currentCapacity++;
   }

   /// <summary>
   /// Adds node to the beginning of the list
   /// </summary>
   /// <param name="node">A node to prepend</param>
   public void Prepend(SLNode<T> node)
   {
      InsertAfter(null, node);
   }

   /// <summary>
   /// Adds new node to the end of the list
   /// </summary>
   /// <param name="node">A node to add</param>
   public void Add(SLNode<T> node)
   {
      InsertAfter(lastNode, node);
   }

   public void Add(T data)
   {
      InsertAfter(lastNode, new SLNode<T>(data));
   }

   /// <summary>
   /// Injects a node before child node,<br/>
   /// null child indicates adding to the list
   /// </summary>
   /// <param name="child">A node before which to inject</param>
   /// <param name="node">A node to inject</param>
   public void InsertBefore(SLNode<T>? child, SLNode<T> node)
   {
      SLNode<T>? parent = null;

      var enmtor = GetEnumerator();
      enmtor.Reset();
      while (enmtor.MoveNext() && enmtor.Current != child) parent = enmtor.Current;

      InsertAfter(parent, node);
   }

   /// <summary>
   /// Injects node at the specified position
   /// </summary>
   /// <param name="index">A list index to insert at</param>
   /// <param name="node">A node to inject</param>
   public void InsertAt(int index, SLNode<T> node)
   {
      SLNode<T>? parent = index == 0 ? null : GetNodeByIndex(index - 1);
      InsertAfter(parent, node);
   }

   /// <summary>
   /// Removes a node and connects its parent to its child, if possible
   /// </summary>
   /// <param name="node">A node to match</param>
   void Remove(SLNode<T>? parent, SLNode<T> node)
   {
      if (parent != null) parent.Child = node.Child;

      if (node == firstNode) firstNode = node.Child;
      if (node == lastNode) lastNode = parent;

      nodes[node.position] = null;

      currentCapacity--;
#if DEBUG
      if (currentCapacity < 0)
         throw new CustomException("SLL.Remove : negative capacity");
#endif
   }

   /// <summary>
   /// Removes a node from the list
   /// </summary>
   /// <param name="node">A node to match</param>
   /// <returns>
   /// true - on success<br/>
   /// false - on failure
   /// </returns>
   public bool RemoveNode(SLNode<T> node)
   {
      SLNode<T>? parent = null;

      var enmtor = GetEnumerator();
      enmtor.Reset();
      while (enmtor.MoveNext())
      {
         SLNode<T> current = enmtor.Current;
         if (node == current)
         {
            Remove(parent, node);
            return true;
         }
         parent = current;
      }

      return false;
   }

   /// <summary>
   /// Removes a node from the list
   /// </summary>
   /// <param name="index">A node chain index to match</param>
   public void RemoveAt(int index)
   {
      SLNode<T> node;
      SLNode<T>? parent;

      if (index == 0)
      {
         parent = null;
         node = firstNode ?? 
            throw new CustomException("SLL.RemoveAt : the list is empty", new IndexOutOfRangeException());
      }
      else {
         parent = GetNodeByIndex(index - 1);
         node = parent.Child ??
            throw new CustomException("SLL.RemoveAt : couldn't find parent.Child (null)");
      }
      
      Remove(parent, node);
   }
}

class SLLEnum<T> : IEnumerator<SLNode<T>>
{
   readonly SLL<T> list;
   SLNode<T>? _current;

   public SLLEnum(SLL<T> _list) => list = _list;

   public SLNode<T> Current
   {
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
      return _current != null;
   }

   public void Dispose()
   {
      throw new CustomException("SLLEnum.Dispose : not implemented", new NotImplementedException());
   }
}