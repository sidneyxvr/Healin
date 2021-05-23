using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Healin.Shared.Utils
{
    public static class CustomValidation
    {
        public static string PhoneRegularExpression => @"^[1-9]{2}[9]{0,1}[6-9]{1}[0-9]{3}[0-9]{4}$";
        public static string EmailRegularExpression => @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// Regular expression for name and last name
        /// </summary>
        public static string NameRegularExpression => @"^([a-zA-Z]{2,}\s[a-zA-z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)";

        public static bool ValidateCpf(string cpf)
        {
            return CpfValidation.Validate(cpf);
        }

        public static bool ValidateCnpj(string cnpj)
        {
            return CnpjValidation.Validate(cnpj);
        }

        internal static class CpfValidation
        {
            public const int CpfSize = 11;

            public static bool Validate(string cpf)
            {
                if(cpf is null || cpf.Length != CpfSize)
                {
                    return false;
                }

                var cpfNumbers = cpf.OnlyNumber();

                if (!ValidSize(cpfNumbers)) return false;
                return !HasRepeatedDigits(cpfNumbers) && HasValidDigits(cpfNumbers);
            }

            private static bool ValidSize(string value)
            {
                return value.Length == CpfSize;
            }

            private static bool HasRepeatedDigits(string value)
            {
                string[] invalidNumbers =
                {
                    "00000000000",
                    "11111111111",
                    "22222222222",
                    "33333333333",
                    "44444444444",
                    "55555555555",
                    "66666666666",
                    "77777777777",
                    "88888888888",
                    "99999999999"
                };
                return invalidNumbers.Contains(value);
            }

            private static bool HasValidDigits(string value)
            {
                var number = value.Substring(0, CpfSize - 2);
                var verifyDigit = new VerifyDigit(number)
                    .WithMultipliersUpTo(2, 11)
                    .Replacing("0", 10, 11);
                var firstDigit = verifyDigit.CalculaDigito();
                verifyDigit.AddDigit(firstDigit);
                var secondDigit = verifyDigit.CalculaDigito();

                return string.Concat(firstDigit, secondDigit) == value.Substring(CpfSize - 2, 2);
            }
        }

        internal static class CnpjValidation
        {
            public const int CnpjSize = 14;

            public static bool Validate(string cpnj)
            {
                var cnpjNumbers = cpnj.OnlyNumber();

                if (!HasValidSize(cnpjNumbers)) return false;
                return !HasRepeatedDigits(cnpjNumbers) && HasValidDigits(cnpjNumbers);
            }

            private static bool HasValidSize(string value)
            {
                return value.Length == CnpjSize;
            }

            private static bool HasRepeatedDigits(string value)
            {
                string[] invalidNumbers =
                {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };
                return invalidNumbers.Contains(value);
            }

            private static bool HasValidDigits(string value)
            {
                var number = value.Substring(0, CnpjSize - 2);

                var verifyDigit = new VerifyDigit(number)
                    .WithMultipliersUpTo(2, 9)
                    .Replacing("0", 10, 11);
                var firstDigit = verifyDigit.CalculaDigito();
                verifyDigit.AddDigit(firstDigit);
                var secondDigit = verifyDigit.CalculaDigito();

                return string.Concat(firstDigit, secondDigit) == value.Substring(CnpjSize - 2, 2);
            }
        }

        internal sealed class VerifyDigit
        {
            private string _number;
            private const int Module = 11;
            private readonly List<int> _multipliers = new (){ 2, 3, 4, 5, 6, 7, 8, 9 };
            private readonly IDictionary<int, string> _replacements = new Dictionary<int, string>();
            private readonly bool _complementaryModule = true;

            public VerifyDigit(string number)
            {
                _number = number;
            }

            public VerifyDigit WithMultipliersUpTo(int firstMultiplier, int lastMultiplier)
            {
                _multipliers.Clear();
                for (var i = firstMultiplier; i <= lastMultiplier; i++)
                    _multipliers.Add(i);

                return this;
            }

            public VerifyDigit Replacing(string substitute, params int[] digits)
            {
                foreach (var i in digits)
                {
                    _replacements[i] = substitute;
                }
                return this;
            }

            public void AddDigit(string digit)
            {
                _number = string.Concat(_number, digit);
            }

            public string CalculaDigito()
            {
                return (_number.Length <= 0) ? "" : GetDigitSum();
            }

            private string GetDigitSum()
            {
                var soma = 0;
                for (int i = _number.Length - 1, m = 0; i >= 0; i--)
                {
                    var produto = (int)char.GetNumericValue(_number[i]) * _multipliers[m];
                    soma += produto;

                    if (++m >= _multipliers.Count) m = 0;
                }

                var mod = (soma % Module);
                var result = _complementaryModule ? Module - mod : mod;

                return _replacements.ContainsKey(result) ? _replacements[result] : result.ToString(CultureInfo.CurrentCulture);
            }
        }
    }
}
