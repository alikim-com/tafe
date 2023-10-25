/// <summary>
/// Single Linked Node
/// </summary>
/// <typeparam name="T"></typeparam>
class SLNode<T> : DataNode<T>
{
   public int position; // for fast removal from list
   public SLNode<T>? Child;

   public SLNode(T data) : base(data) { }

   public override string ToString()
   {
      return $"""
      -------------------------
      object Single Linked Node
      Data: {Data}
      Child:
      {Child}
      """;
   }
}
