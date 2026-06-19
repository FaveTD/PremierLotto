using PremierLotto.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Utilities
{
    public class Validation
    {
        public bool TryParseGuesses(string input, GameSettings settings, out List<string> validGuesses)
        {
            validGuesses = new List<string>();

            string[] pieces = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (pieces.Length != 4) return false;

            foreach (string piece in pieces)
            {
                string formattedPiece = piece.ToUpper(); 

                if (!settings.AllowDuplicates && validGuesses.Contains(formattedPiece))
                {
                    return false;
                }

                if (settings.IsAlphanumeric)
                {

                    validGuesses.Add(formattedPiece);
                }
                else
                {
                    if (int.TryParse(formattedPiece, out int num))
                    {
                        if (num < 0 || num > settings.MaxNumber)
                        {
                            return false;
                        }
                        validGuesses.Add(formattedPiece);
                    }
                    else
                    {
                        return false; 
                    }
                }
            }

            return validGuesses.Count == 4;
        }

        public bool IsValidPlayerCount(string input, out int count)
        {
            return int.TryParse(input, out count) && count >= 2 && count <= 10;
        }
    }
}
