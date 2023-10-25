/// <summary>
/// Generic Data Node
/// </summary>
/// <typeparam name="T"></typeparam>
class DataNode<T>
{
    public T? Data { get; set; }

    public DataNode()
    {

    }

    public DataNode(T data)
    {
        Data = data;
    }

   public override string ToString() {
      return $"""
      -----------
      object DataNode
      Data: {Data}
      """;
   }

}