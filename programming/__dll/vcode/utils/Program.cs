
// https://github.com/alikim-com/tafe/blob/main/programming/__dll/vcode/utils/
namespace utils;

/// <summary>
/// Represents a menu that allows the user to select and execute tasks based on a list of options.
/// </summary>
public class Menu
{
   /// <summary>
   /// The range of valid menu choices.
   /// </summary>
   readonly int[] range;

   /// <summary>
   /// The content of the menu, displaying available options to the user.
   /// </summary>
   readonly string content;

   /// <summary>
   /// A warning message to be displayed if an invalid input is provided by the user.
   /// </summary>
   readonly string warn;

   /// <summary>
   /// A message that defines how to exit the menu.
   /// </summary>
   readonly string exit;

   /// <summary>
   /// The list of menu options, each associated with a task represented as a delegate.
   /// </summary>
   readonly List<KeyValuePair<string, Delegate?>> list;

   // Constructors

   /// <summary>
   /// Initializes a new instance of the <see cref="Menu"/> class.
   /// </summary>
   /// <param name="_list">A list of key-value pairs where keys represent menu options and values represent associated tasks as delegates.</param>
   /// <param name="first">The starting index for menu choices.</param>
   /// <param name="_warn">A warning message to display if an invalid input is provided by the user.</param>
   /// <param name="_exit">A custom word that will exit the menu; set it to "" to exit by pressing Enter or to "end" to exit by typing 'end'. This is used only when none of the menu options have a null delegate, which otherwise indicates the menu exit.</param>
   public Menu(List<KeyValuePair<string, Delegate?>> _list, int first = 1, string? _warn = null, string? _exit = null)
   {
      int len = _list.Count;
      content = "\n";
      range = new int[] { first, first + len - 1 };
      warn = _warn != null ? _warn : $"Please enter a number between {range[0]} and {range[1]}\n";
      if (_exit != null) exit = _exit;
      for (int i = 0; i < len; i++)
      {
         var rec = _list[i];
         int ind = first + i;
         content += $"{ind}. {rec.Key}\n";
         if (exit == null && rec.Value == null) exit = ind.ToString();
      }
      exit ??= "";
      list = _list;
   }

    /// <summary>
   /// Runs the menu and allows the user to select and execute tasks until an exit condition is met.
   /// </summary>
   public void RunUntilExit()
   {
      while (UserInput.UntilSafeCustom<int>(UserInput.IntRange, range, out int choice, content, warn, exit) == 0)
      {
         Delegate? task = list[choice - range[0]].Value;
         task?.DynamicInvoke();
      }
   }
}


/// <summary>
/// A utility class for custom output 
/// </summary>
public class Output
{

   /// <summary>
   /// A shortcut for Console.WriteLine
   /// </summary>
   /// <param name="obj">Console.WriteLine argument</param>
   public delegate void WL(object? obj);
   static public readonly WL cwl = Console.WriteLine;

   /// <summary>
   /// Logging multiple arguments via Console
   /// </summary>
   /// <param name="args">multiple arguments passes into an array</param>
   public static void log(params object[] args)
   {
      foreach (var obj in args) Console.WriteLine(obj);
   }

}

/// <summary>
/// A utility class for handling user input and parsing.
/// </summary>
public class UserInput
{

   /// <summary>
   /// Attempts to parse the input string to the specified type.
   /// </summary>
   /// <typeparam name="T">The generic type to parse to.</typeparam>
   /// <param name="str">The input string to parse.</param>
   /// <param name="val">The parsed value if successful.</param>
   /// <param name="warn">An optional warning to display in case of parsing failure.</param>
   /// <returns>
   ///  0 if parsing is successful,<br/>
   ///  1 if parsing fails with a known exception,<br/>
   /// -1 if an unforeseen exception occurs.
   /// </returns>
   public static int TryParseTo<T>(string str, out T val, string? warn = null) where T : struct
   {
      string _warn = string.IsNullOrEmpty(warn) ? $"<Invalid input for '{typeof(T)}'>" : warn;

      try
      {
         val = (T)Convert.ChangeType(str, typeof(T));
         return 0;
      }
      catch (Exception ex)
      {
         val = new();
         if (ex is InvalidCastException or FormatException or OverflowException or ArgumentNullException)
         {
            Console.WriteLine(_warn);
            return 1;
         }
         else
         {
            Console.WriteLine($"TryParseTo :: unforseen exception <{ex.Message}>");
            return -1;
         }
      }
   }

   /// <summary>
   /// Prompts the user for input until a valid value of type T (struct) is provided.
   /// </summary>
   /// <typeparam name="T">The type to parse to.</typeparam>
   /// <param name="val">The parsed value if successful.</param>
   /// <param name="prompt">An optional prompt to display before input.</param>
   /// <param name="warn">An optional warning to display in case of parsing (TryParseTo) failure.</param>
   /// <param name="exit">An optional exit command to terminate input.</param>
   /// <returns>
   /// 0 if parsing is successful,<br/> 
   /// 1 if the user exits,<br/> 
   /// -1 if an unforeseen exception (in TryParseTo) occurs.</returns>        
   public static int UntilSafe<T>(
      out T val,
      string? prompt = null, string? warn = null, string? exit = null) where T : struct
   {
      while (true)
      {
         if (!string.IsNullOrEmpty(prompt)) Console.WriteLine(prompt);

         string inp = Console.ReadLine() ?? "";

         if (exit != null && inp == exit)
         {
            Console.WriteLine("Terminated by user");
            val = new();
            return 1;
         }

         int res = TryParseTo<T>(inp, out val, warn);

         if (res != 1) return res;
      }

   }

   /// <summary>
   /// A delegate type used as UntilSafeCustom method parameter.
   /// </summary>
   public delegate int Validator<T>(string inp, string? warn, out T val, int[]? cfg = null);

   /// <summary>
   /// Checks if the input string is not empty or only whitespaces, and optionally, if it's of a certain length.
   /// </summary>
   /// <param name="inp">The input string.</param>
   /// <param name="warn">An optional warning message.</param>
   /// <param name="val">The output string.</param>
   /// <param name="cfg">An optional configuration array (zero element contains min length).</param>
   /// <returns>
   /// 0 on success,<br/>
   /// 1 on failure<br/>
   /// </returns>
   public static int FilledString(string inp, string? warn, out string val, int[]? cfg)
   {
      if (!string.IsNullOrWhiteSpace(inp) && (cfg == null || inp.Length >= cfg[0]))
      {
         val = inp;
         return 0;
      }
      if (!string.IsNullOrEmpty(warn)) Console.WriteLine(warn);
      val = "";
      return 1;
   }

   /// <summary>
   /// <code>Validator delegate</code>
   /// Checks if the input string has only one character and (optionally) if this character is in the provided array.
   /// </summary>
   /// <param name="inp">The input string to be checked.</param>
   /// <param name="warn">An optional warning message.</param>
   /// <param name="val">When successful, contains the character from the input string.</param>
   /// <param name="cfg">An optional configuration array.</param>
   /// <returns>
   /// 0 on success,<br/>
   /// 1 if the input string is not a single character or does not match the configuration.
   /// </returns>
   public static int CharRange(string inp, string? warn, out char val, int[]? cfg)
   {
      string trm = inp.Trim();
      if (trm.Length != 1)
      {
         if (!string.IsNullOrEmpty(warn)) Console.WriteLine(warn);
         val = '\0';
         return 1;
      }
      val = inp[0];

      if (cfg == null) return 0;

      return cfg.Contains(val) ? 0 : 1;
   }

   /// <summary>
   /// <code>Validator delegate</code>
   /// Tries to parse and then to validate if an integer input is within the specified range.
   /// </summary>
   /// <param name="inp">The input string to parse.</param>
   /// <param name="warn">An optional warning to display in case of parsing (TryParseTo) failure or invalid range.</param>
   /// <param name="val">The parsed integer value if successful.</param>
   /// <param name="cfg">An optional configuration array defining the allowed range.</param>
   /// <returns>
   ///  0 if parsing and validation are successful,<br/>
   ///  1 if parsing fails or validation fails,<br/> 
   /// -1 if an unforeseen exception occurs (in TryParseTo).</returns>
   public static int IntRange(string inp, string? warn, out int val, int[]? cfg)
   {
      int res = TryParseTo<int>(inp, out val, warn);
      if (res != 0) return res;

      bool inRange = cfg != null && val >= cfg[0] && val <= cfg[1];
      if (!inRange) Console.WriteLine(warn);

      return inRange ? 0 : 1;
   }

   /// <summary>
   /// <code>Validator delegate</code>
   /// Generates an array of words (of a minimum length defined by conf array) from a given input string.
   /// </summary>
   /// <param name="inp">The input string to be split into words.</param>
   /// <param name="warn">An optional warning message to be displayed when a condition is not met.</param>
   /// <param name="sArr">An output parameter that receives the resulting array of words.</param>
   /// <param name="cfg">An configuration array.</param>
   /// <returns>
   /// 0 if the operation is successful,<br/>
   /// 1 if the input can not be split into a min required number of words.
   /// </returns>
   public static int GenWords(string inp, string? warn, out string[] sArr, int[]? cfg)
   {
      string[] strArr = inp.Split(" ");
      string[] arr = strArr.Where(s => !string.IsNullOrEmpty(s)).ToArray();

      if (cfg != null && arr.Length >= cfg[0])
      {
         sArr = arr;
         return 0;
      }

      Console.WriteLine(warn);

      sArr = Array.Empty<string>();
      return 1;
   }

   /// <summary>
   /// Prompts the user for input until a valid value is provided satisfying a custom validator function.
   /// </summary>
   /// <typeparam name="T">The output type of the validator.</typeparam>
   /// <param name="Func">The validator function.</param>
   /// <param name="cfg">An optional configuration array for the validator.</param>
   /// <param name="val">The output validated value (if successful).</param>
   /// <param name="prompt">An optional prompt to display before input.</param>
   /// <param name="warn">An optional warning for the validator.</param>
   /// <param name="exit">An optional exit command to terminate input.</param>
   /// <returns>
   ///  0 if parsing and validation are successful,<br/>
   ///  1 if the user exits,<br/>
   /// -1 if an unforeseen exception occurs (in validator).</returns>
   public static int UntilSafeCustom<T>(
      Validator<T> Func, int[]? cfg, out T val,
      string? prompt = null, string? warn = null, string? exit = null
   )
   {

      while (true)
      {
         if (!string.IsNullOrEmpty(prompt)) Console.WriteLine(prompt);

         string inp = Console.ReadLine() ?? "";

         if (exit != null && inp == exit)
         {
            Console.WriteLine("Terminated by user");
            val = default!;
            return 1;
         }

         int res = Func(inp, warn, out val, cfg);

         if (res != 1) return res;
      }

   }

}