namespace AbstractionExercises;

class Program
{

   static readonly List<BankAccount> accounts = new()
   {
      // initial accounts
      new ChequeAccount("Cheque Name 1", "Cheque Address 1", 1, 100.0m, 200.0m),
      new SavingsAccount("Savings Name 1", "Savings Address 1", 2, 0.0m),
      new PassbookAccount("Passbook Name 1", "Passbook Address 1", 3, 50.0m),
      new InvestmentAccount("Investment Name 1", "Investment Address 1", 4, 10000.0m)
   };
   static uint lastAccNo = 4;

   static List<KeyValuePair<string, Delegate?>> menuInfo = new()
   {
      new( "Quit", null ),
      new( "Add New Account", AddNewAcount ),
      new( "Withdraw", Withdraw ),
      new( "Deposit", Deposit ),
      new( "Update overdraft", UpdateOverdraft ),
      new( "Add interest", AddInterest ),
      new( "Print balance", PrintBalance ),
      new( "Print all", PrintAll ),
   };

   static void Main(string[] args)
   {
      var menu = new utils.Menu(menuInfo, 0);

      menu.RunUntilExit();
   }

   public static BankAccount? GetAccountByNumber()
   {
      string prompt = "Please enter account number (or 0 to exit): ";
      if (UntilSafe<int>(out int accNo, prompt, null, "0") != 0) return null;

      var acc = accounts.Find(acc => acc.AccNumber == accNo);
      if (acc == null) cwl("The account does not exist.");
      return acc;
   }

   public static void PostAction(string op, BankAccount acc)
   {
      string prompt = $"{op} was successfull, print new balance? Y/N";
      int[] cfg = { 'y', 'n', 'Y', 'N' };
      if (UntilSafeCustom<char>(CharRange, cfg, out char val, prompt) != 0) return;
      if (val == 'y' || val == 'Y') acc.PrintBalance();
   }

   public static void AddNewAcount()
   {
      cwl("Creating new account, press Enter to exit.");

      int[] cfgN = { 3 };
      string prompt = $"Please enter your name (min {cfgN[0]} characters): ";
      if (UntilSafeCustom<string>(FilledString, cfgN, out string name, prompt) != 0) return;

      int[] cfgA = { 5 };
      prompt = $"Please enter your name (min {cfgA[0]} characters): ";
      if (UntilSafeCustom<string>(FilledString, cfgA, out string address, prompt) != 0) return;

      uint accNo = lastAccNo++;

      int[] cfgD = { 0, 1_000_000_000 };
      prompt = $"Please enter initial deposit amount: ";
      string warn = "Please enter a zero or a positive rounded amount";
      if (UntilSafeCustom<int>(IntRange, cfgA, out int deposit, prompt, warn) != 0) return;

      prompt = """
      Please enter account type (1 - 4): 
      1. Cheque
      2. Savings
      3. Passbook
      4. Investment
      """;
      int[] cfgT = { 1, 4 };
      if (UntilSafeCustom<int>(IntRange, cfgT, out int type, prompt) != 0) return;

      BankAccount? acc = null;
      switch (type)
      {
         case 1:
            int[] cfgO = { 0, 1_000_000_000 };
            prompt = $"Please enter overdraft amount: ";
            warn = "Please enter a zero or a positive rounded amount";
            if (UntilSafeCustom<int>(IntRange, cfgA, out int overdraft, prompt, warn) != 0) return;
            acc = new ChequeAccount(name, address, accNo, deposit, overdraft);
            break;
         case 2:
            acc = new SavingsAccount(name, address, accNo, deposit);
            break;
         case 3:
            acc = new PassbookAccount(name, address, accNo, deposit);
            break;
         case 4:
            acc = new InvestmentAccount(name, address, accNo, deposit);
            break;
      }
      if (acc != null)
      {
         accounts.Add(acc);
         cwl("Account created successfully.\n" + acc.ToString() + '\n');
      }
   }

   public static void Withdraw()
   {
      var acc = GetAccountByNumber();
      if (acc == null) return;

      string op = "Withdrawal";

      string prompt = $"Please enter {op.ToLower()} amount (or 0 to exit): ";
      if (UntilSafe<decimal>(out decimal amount, prompt, null, "0") != 0) return;

      if (acc.Withdraw(amount))
         PostAction(op, acc);
      else
         cwl($"{op} error.");
   }

   public static void Deposit()
   {
      var acc = GetAccountByNumber();
      if (acc == null) return;

      string op = "Deposit";

      string prompt = $"Please enter {op.ToLower()} amount (or 0 to exit): ";
      if (UntilSafe<decimal>(out decimal amount, prompt, null, "0") != 0) return;

      if (acc.Deposit(amount))
         PostAction(op, acc);
      else
         cwl($"{op} error.");
   }

   public static void UpdateOverdraft()
   {
      var acc = GetAccountByNumber();
      if (acc == null) return;

      if (acc is not ChequeAccount cheque)
      {
         cwl("This account doesn't have overdraft.");
         return;
      }

      string prompt = $"Please enter new overdraft amount (or 0 to exit): ";
      if (UntilSafe<decimal>(out decimal amount, prompt, null, "0") != 0) return;
      cheque.Overdraft = amount;

      cwl($"Overdraft set.");
   }

   public static void AddInterest()
   {
      var acc = GetAccountByNumber();
      if (acc == null) return;

      if (acc is not SavingsAccount savings)
      {
         cwl("This account does not accrue interest.");
         return;
      }
      savings.AddInterest();

      PostAction("Adding interest", acc);
   }

   public static void PrintBalance()
   {
      var acc = GetAccountByNumber();
      if (acc == null) return;
      acc.PrintBalance();
   }

   public static void PrintAll()
   {
      foreach (var acc in accounts) cwl(acc.ToString() + '\n');
   }

}
