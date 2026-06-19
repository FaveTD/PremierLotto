using System;
using System.Collections.Generic;
using System.Text;

namespace PremierLotto.Utilities
{
    public static class ValidationExtensions
    {
        public static bool IsValidAge(this string inputString, out int verifiedAge)
        {
            if (!int.TryParse(inputString, out verifiedAge))
            {
                Console.WriteLine("Input a valid whole number ( no decimal or words).");
                return false;
            }
            if (verifiedAge < 18)
            {
                Console.WriteLine("You must be at least 18 years old to play Premier Lotto 😔. Please come back when you are of age.");
                return false; ;
            }
            return true;
        }
    
        



    }
}
