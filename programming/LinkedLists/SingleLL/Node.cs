/// <summary>
/// Generic Data Node
/// </summary>
/// <typeparam name="T"></typeparam>
class Node<T>
{

    public T? Data { get; set; }

    public Node()
    {

    }

    public Node(T data)
    {
        Data = data;
    }

   public override string ToString() {
      return $"""
      -----------
      object Node
      Data: {Data}
      """;
   }

}