using System.ComponentModel.DataAnnotations;

namespace BeautySalonSystem.UI.Util
{
    public class MinValueAttribute : ValidationAttribute
    {
        private readonly decimal _maxValue;

        public MinValueAttribute(int maxValue)
        {
            _maxValue = (decimal) maxValue;
        }

        public override bool IsValid(object value)
        {
            return (decimal) value <= _maxValue;
        }
    }
}