
using System.Reflection;

using static utils.UserInput;

namespace ConsoleApp1;

public class TaskOne
{

   public static void Test(int a) {
      Console.WriteLine(a);
   }
   
   public static void Menu() {

      string[] funcNames = { "Add", "Subtract", "Multiply", "Divide"};
      int len = funcNames.Length;
      MethodInfo[] tasks = new MethodInfo[len];

      Type tp = typeof(TaskOne); 

      for(int i = 0; i < len; i++) {
         MethodInfo? fn = tp.GetMethod(funcNames[i], BindingFlags.Static | BindingFlags.NonPublic);
         if (fn == null) {
            Console.WriteLine($"Function '{funcNames[i]}' not found");
            return;
         }
         tasks[i] = fn;
      }
      Console.WriteLine("\nTasks assembled... \n");

      while(UntilSafeCustom<int>(IntRange, new int[] {1, 5}, out int choice, 
      @"
Would you like to:
   1. Add numbers
   2. Subtract numbers
   3. Multiply numbers
   4. Divide numbers
   5. Exit
      ", 
      "Please enter a number from 1 to 5",
      exit: "5") == 0) {
         tasks[choice - 1].Invoke(null, null);
      }
   }

   static bool GetTwoNumbers(out double n1, out double n2) {
      int res1 = UntilSafe<double>(out n1, "Please enter the first number or 'menu' to go back:", null, "menu");
      if (res1 != 0)
      {
         n2 = 0;
         return false;
      }
      int res2 = UntilSafe<double>(out n2, "Please enter the second number or 'menu' to go back:", null, "menu");
      return res2 == 0;
   }

   static void PrintResults(double n1, double n2, double res, string op) {
      Console.WriteLine($"{n1:f3} {op} {n2:f3} = {res:f3}");
   }

   static void Add() {
      if (!GetTwoNumbers(out double n1, out double n2)) return;
      double res = n1 + n2;
      PrintResults(n1, n2, res, "+");
   }

   static void Subtract() {
      if (!GetTwoNumbers(out double n1, out double n2)) return;
      double res = n1 - n2;
      PrintResults(n1, n2, res, "-");
   }

   static void Multiply() {
      if (!GetTwoNumbers(out double n1, out double n2)) return;
      double res = n1 * n2;
      PrintResults(n1, n2, res, "*");
   }

   static void Divide() {
      if (!GetTwoNumbers(out double n1, out double n2)) return;
      double res = n1 / n2;
      PrintResults(n1, n2, res, "/");
   }
}
