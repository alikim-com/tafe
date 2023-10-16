namespace AbstractionExercises;

public class InvestmentAccount : SavingsAccount
{
   protected override string Type => "Investment";
   protected override decimal Interest => _interest + (AccBalance > 100_000 ? 0.02m : 0.01m);
   int transLimit = 3;

   public InvestmentAccount(
      string name,
      string address,
      uint accNumber,
      decimal initDeposit = 0
   ) : base(
      name,
      address,
      accNumber,
      initDeposit,
      withdrawalFee: 0
   )
   {

   }

   public new bool Withdraw(decimal amount)
   {
      if (transLimit <= 0)
      {
         cwl("Transaction limit reached.");
         return false;
      }
      base.Withdraw(amount);
      transLimit--;
      cwl($"Transactions left: {transLimit}.");
      return true;
   }

   public new bool Deposit(decimal amount)
   {
      if (transLimit <= 0)
      {
         cwl("Transaction limit reached.");
         return false;
      }
      base.Deposit(amount);
      transLimit--;
      cwl($"Transactions left: {transLimit}.");
      return true;
   }

   public override string ToString()
   {
      return $"""
      {Type} Account
      Acc.No {AccNumber}
      {AccName}
      {AccAddress}
      Balance: {AccBalance:C}
      Interest: {Interest:P}
      Transactions left: {transLimit}
      """;
   }

}
