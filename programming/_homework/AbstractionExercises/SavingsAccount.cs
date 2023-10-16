namespace AbstractionExercises;

public class SavingsAccount : BankAccount
{
   protected virtual string Type { get => "Savings"; }
   protected decimal _interest;
   protected virtual decimal Interest => _interest;
   protected decimal withdrawalFee;

   public SavingsAccount(
      string name,
      string address,
      uint accNumber,
      decimal initDeposit = 0,
      decimal _interest = 0.05m,
      decimal withdrawalFee = 5.0m
   ) : base(
      name,
      address,
      accNumber,
      initDeposit
   )
   {
      this._interest = _interest;
      this.withdrawalFee = withdrawalFee;
   }

   public bool AddInterest()
   {
      AccBalance += AccBalance * Interest;
      return true;
   }

   public override bool Withdraw(decimal amount)
   {
      decimal totalWD = amount + withdrawalFee;
      if (AccBalance >= totalWD)
      {
         AccBalance -= totalWD;
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
      {Type} Account
      Acc.No {AccNumber}
      {AccName}
      {AccAddress}
      Balance: {AccBalance:C}
      Interest: {Interest:P}
      Withdrawal fee: {withdrawalFee:C}
      """;
   }

}
