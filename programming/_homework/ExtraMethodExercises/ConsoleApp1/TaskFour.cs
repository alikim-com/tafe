﻿
using static utils.UserInput;

namespace ConsoleApp1;

public class TaskFour
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
         "Your first name must be at least one character long",
         "menu");

      if (res != 0) return;

      res = UntilSafeCustom<string>(
         FilledString, new int[] { 1 }, out string lname,
         "Please enter your last name or type 'menu' to go back:",
         "Your last name must be at least one character long",
         "menu");

      if (res != 0) return;

      res = UntilSafeCustom<int>(
         IntRange, new int[] { 0, int.MaxValue }, out int salary,
         "Please enter your annual salary or type 'menu' to go back:",
         "Your salary must be a whole positive number",
         "menu");
      
      if (res != 0) return;

      res = UntilSafeCustom<char>(
         CharRange, new int[] { 'y', 'n', 'Y', 'N' }, out char choice,
         "Do you have any deductions? (Y\\N):", 
         "Please type Y or N"
      );

      int cnt = 1;
      List<Tuple<string, int>> deductions = new();

      bool yes = choice == 'y' || choice == 'Y';
      while(yes) {

         res = UntilSafeCustom<string>(
            FilledString, new int[] { 1 }, out string item,
            $"Enter name of deduction {cnt++} (Enter to stop):",
            "Your deductible item name must be at least one character long",
            "");

         if (res != 0) break;

         res = UntilSafeCustom<int>(
            IntRange, new int[] { 1, int.MaxValue }, out int amount,
            $"Enter deduction amount for {item}:",
            "The deduction must be a whole positive number"
            );

         deductions.Add(Tuple.Create(item, amount));
      }

      Console.WriteLine($"\nGenerating report for {fname} {lname}...");

      int deducted = salary;
      foreach (var d in deductions) deducted -= d.Item2;
      if (deducted < 0) deducted = 0;

      double taxRate = CalcTaxRate(deducted);
      double afterTax = deducted * (1 - taxRate);

      PrintReport(fname, lname, salary, deducted, taxRate, afterTax, deductions.Count > 0 ? deductions : null);
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
      string fname, string lname, int salary, int deducted, double taxRate, 
      double afterTax, List<Tuple<string, int>>? deductions)
   {

      string dedStr = "";
      if (deductions != null)
      {
         dedStr = "\n";
         foreach (var d in deductions) dedStr += $"Deduction {d.Item1}: {d.Item2}\n";
         dedStr += $"Before Tax Salary including Deductions: {deducted}";
      }

      string outp = @$"
Name: {fname} {lname}
Before Tax Salary: {salary}
Tax Rate: {taxRate:P}{dedStr}
After Tax Salary: {afterTax:f2}";

      Console.WriteLine(outp);
   }
}
