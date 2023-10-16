
using static utils.UserInput;

namespace ConsoleApp1;

public class TaskTwo {

   static int MakeWords(out string[] words)  {

      return UntilSafeCustom<string[]>(
         GenWords, new int[] { 2 }, out words,
         "Please enter a few words (or type 'end' to exit):",
         "Please enter at least two words", 
         "end"
      );
   }

   static void SortWords(ref string[] words) {
      Array.Sort<string>(words);
   }

   static void PrintWords(ref string[] words) {
      Console.WriteLine($"[{string.Join(", ", words)}]");
   }

   public static void Words() {

      int res = MakeWords(out string[] words);
      if (res == 1) return;

      SortWords(ref words);

      PrintWords(ref words);  
   }

}
