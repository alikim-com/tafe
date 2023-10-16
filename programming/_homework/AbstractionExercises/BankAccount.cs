namespace AbstractionExercises;

public abstract class BankAccount
{
   protected string _accName = ""; // suppress warning CS8618
   public string AccName 
   { 
      get => _accName;
      protected set => _accName = value;
   }

   protected string _accAddress = ""; // suppress warning CS8618
   public string AccAddress 
   { 
      get => _accAddress;
      protected set => _accAddress = value;
   }

   protected uint _accNumber;
   public uint AccNumber 
   { 
      get => _accNumber;
      protected set => _accNumber = value;
   }

   protected decimal _accBalance;
   public virtual decimal AccBalance
   {
      get => _accBalance;
      protected set => _accBalance = value;
   } 

   public BankAccount(
      string name,
      string address,
      uint accNumber,
      decimal initDeposit
   )
   {
      AccName = name.ToUpper();
      AccAddress = address;
      AccNumber = accNumber;
      Deposit(initDeposit);
   }

   public abstract bool Withdraw(decimal amount);
   public abstract bool Deposit(decimal amount);
   public void PrintBalance()
   {
      Console.WriteLine($"""
      Acc.No {AccNumber}
      Balance: {AccBalance}
      """);
   }
}
