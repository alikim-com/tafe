namespace AbstractionExercises;

public class ChequeAccount : BankAccount
{
   const string type = "Cheque";

   decimal _overdraft;
   public decimal Overdraft {
      get => _overdraft;
      set => _overdraft = value;
   }

   public ChequeAccount(
      string name,
      string address,
      uint accNumber,
      decimal initDeposit = 0,
      decimal overdraft = 0
   ) : base(
      name, 
      address, 
      accNumber, 
      initDeposit
   )
   {
      Overdraft = overdraft;
   }

   public override bool Withdraw(decimal amount)
   {
      if (AccBalance + Overdraft >= amount)
      {
         AccBalance -= amount;
         return true;
      }
      cwl("Insufficient balance.");
      return false;
   }
   public override bool Deposit(decimal amount)
   {
      AccBalance += amount;
      return true;
   }

   public override string ToString()
   {
      return $"""
      {type} Account
      Acc.No {AccNumber}
      {AccName}
      {AccAddress}
      Balance: {AccBalance:C}
      Overdraft: {Overdraft:C}
      """;
   }

}
