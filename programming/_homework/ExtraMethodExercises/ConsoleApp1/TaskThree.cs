
using static utils.UserInput;

namespace ConsoleApp1;

public class TaskThree
{

   static readonly Tuple<int, double>[] taxTable = { 
      Tuple.Create(180000, 0.45),
      Tuple.Create(90001, 0.37),
      Tuple.Create(37001, 0.325),
      Tuple.Create(18201, 0.19),
   };

   public static void Menu()
   {
      while (UntilSafeCustom<int>(IntRange, new int[] { 1, 5 }, out int choice,
      @"
Would you like to:
   1. Create a tax report
   2. Exit
      ",
      "Please enter 1 or 2",
      exit: "2") == 0)
      {
         GenReport();
      }
   }

   static void GenReport() {

      int res = UntilSafeCustom<string>(
         FilledString, new int[] { 1 }, out string fname,
         "Please enter your first name or type 'menu' to go back:",
         "Your first name has to be at least one character long",
         "menu");

      if (res != 0) return;

      res = UntilSafeCustom<string>(
         FilledString, new int[] { 1 }, out string lname,
         "Please enter your last name or type 'menu' to go back:",
         "Your last name has to be at least one character long",
         "menu");

      if (res != 0) return;

      res = UntilSafeCustom<int>(
         IntRange, new int[] { 0, int.MaxValue }, out int salary,
         "Please enter your annual salary or type 'menu' to go back:",
         "Your salary must be a whole positive number",
         "menu");
      
      if (res != 0) return;

      Console.WriteLine($"\nGenerating report for {fname} {lname}...");

      double taxRate = CalcTaxRate(salary);
      double afterTax = salary * (1 - taxRate);

      PrintReport(fname, lname, salary, taxRate, afterTax);
   }

   static double CalcTaxRate(int salary) {
      double taxRate = 0;
      foreach(var br in taxTable)
         if(salary >= br.Item1) {
            taxRate = br.Item2;
            break;
         }
      return taxRate;
   }

   static void PrintReport(
      string fname, string lname, int salary, double taxRate, double afterTax)
   {
      string outp = @$"
Name: {fname} {lname}
Before Tax Salary: {salary}
Tax Rate: {taxRate:P}
After Tax Salary: {afterTax:f2}";

      Console.WriteLine(outp);
   }
}
