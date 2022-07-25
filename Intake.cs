using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Intake
    {
        // Intake functions
        // Test if number/char is in a given Range
        public bool InRange<T>(T inputVal, T Min, T Max)
        {

            // if (inputVal is byte || inputVal is sbyte || inputVal is int || inputVal is long || inputVal is uint || inputVal is ulong inputVal is short || inputVal is ushort)
            if (inputVal is int)
            {
                int Val = (int)Convert.ChangeType(inputVal, typeof(int));
                int MinVal = (int)Convert.ChangeType(Min, typeof(int));
                int MaxVal = (int)Convert.ChangeType(Max, typeof(int));

                // Check that Width is in Range
                if (Val < MinVal)
                {
                    BlackJack.MyWrite(ConsoleColor.Yellow,"Your input is " + inputVal + " and it is smaller than " + Min + "\n");
                    return false;
                }

                if (Val > MaxVal)
                {
                    BlackJack.MyWrite(ConsoleColor.Yellow,"Your input is " + inputVal + " and it is larger than " + Max + "\n");
                    return false;
                }
            }
            else if (inputVal is char)
            {
                char Val = (char)Convert.ChangeType(inputVal, typeof(char));

                if (!char.IsLetter(Val) && !char.IsDigit(Val) && !char.IsSymbol(Val) && !char.IsPunctuation(Val))
                {
                    BlackJack.MyWrite(ConsoleColor.Yellow,"Character should be Printable ASCII\n");
                    return false;
                }
            }

            return true;
        }

        //* Loop on Input Request until receives a number/char in a given Range
        public T CrInRange<T>(string inputRequest, T Min, T Max, out string inputString, string invalidFeedback = null)
        {
            T inputVal = (T)default(T);
            String inputMsg = inputRequest;

            // Collect Width until it is in range or request to stop
            if (inputVal is int)
                inputMsg = inputRequest + " in range [" + Min + "," + Max + "] (Enter to stop): ";
            else if (inputVal is char)
                inputMsg = inputRequest + " [Printable ASCII] (Enter to stop): ";

            do
            {
                inputVal = Cr<T>(inputMsg, out inputString);

                // If Abort - Break loop
                if (inputString == String.Empty)
                    break;
            }
            while (!InRange<T>(inputVal, Min, Max));                // While Width is NOT in Range

            // If Abort - STOP
            if (inputString == String.Empty)
                return (T)default(T);

            return inputVal;
        }

        //* Loop on Input Request until receives a legal number/char
        public T Cr<T>(string inputRequest, out string inputString, string invalidFeedback = null)
        {
            try
            {
                BlackJack.MyWrite(ConsoleColor.Cyan, inputRequest + "\n");
                inputString = Console.ReadLine();

                if (inputString == string.Empty)
                    return (T)default(T);

                return (T)Convert.ChangeType(inputString, typeof(T));
            }
            catch (Exception Err)
            {
                if (invalidFeedback == null)
                    invalidFeedback = $"Your input type was not a valid {typeof(T)}";

                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(invalidFeedback);
                Console.WriteLine(Err.Message);
                Console.ForegroundColor = oldColor;

                return Cr<T>(inputRequest, out inputString, invalidFeedback);
            }
        }
    }
}
