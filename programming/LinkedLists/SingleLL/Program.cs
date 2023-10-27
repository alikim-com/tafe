
class Program
{
   public static void Main(String[] args)
   {

      int[] a = Array.Empty<int>();

      cwl(a[0]);

      var sll = new SLL<int>(5);

      var node = new DataNode<int>(3);
      var slNode = new SLNode<int>(5);
      var slNode2 = new SLNode<int>(10);
      slNode.Child = slNode2;

//      cwl(node);
//      cwl(slNode);

   }
}
