namespace vita.MasterData
{
    public class CountryConsts
    {

        public const int MinAlphaCodeLength = 0;
        public const int MaxAlphaCodeLength = 2;
        public const string AlphaCodeRegex = @"^[a-zA-Z]*$";

        public const string NumericCodeRegex = @"^[0-9]*$";

        public const int MinAlpha3CodeLength = 0;
        public const int MaxAlpha3CodeLength = 3;
        public const string Alpha3CodeRegex = @"^[a-zA-Z]*$";

    }
}