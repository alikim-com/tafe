/// <summary>
/// Single Linked List Node
/// </summary>
/// <typeparam name="T"></typeparam>
class SLNode<T> : Node<T>
{
   public SLNode<T>? Child;

   public SLNode(T data) : base(data) {}

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
