namespace AbstractionExercises;

public class PassbookAccount : SavingsAccount
{
   protected override string Type => "Passbook";

   public PassbookAccount(
      string name,
      string address,
      uint accNumber,
      decimal initDeposit = 0
   ) : base(
      name, 
      address, 
      accNumber,
      initDeposit
   ) 
   { 

   }
}
