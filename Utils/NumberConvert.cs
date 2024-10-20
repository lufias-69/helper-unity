namespace Helper.NumberConvert
{
    public class NumberConvert
    {
        /// <summary>
        /// Converts a long number into a shortened string representation (e.g., "1k", "2.5M", "3.75B") 
        /// based on its value. Supports thousands (k), millions (M), and billions (B).
        /// </summary>
        /// <param name="number">The long number to be formatted.</param>
        /// <param name="decimalPlaces">The number of digits to show after the decimal point.</param>
        /// <returns>A string representation of the number with the appropriate suffix and formatting.</returns>
        public static string FormatNumber(long number, int decimalPlaces = 1)
        {
            string format = "0." + new string('#', decimalPlaces);

            if (number >= 1_000_000_000)
                return (number / 1_000_000_000.0).ToString(format) + "B";
            else if (number >= 1_000_000)
                return (number / 1_000_000.0).ToString(format) + "M";
            else if (number >= 1_000)
                return (number / 1_000.0).ToString(format) + "k";
            else
                return number.ToString();
        }

    }
}