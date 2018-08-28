namespace MjIot.Devices.Reference.HysteresisController
{
    public class StringToFloatConverter : IStringToFloatConverter
    {
        public bool IsNumeric(string value)
        {
            return float.TryParse(FormatString(value), out float number);
        }

        public float Convert(string value)
        {
            return float.Parse(FormatString(value));
        }

        private string FormatString(string value)
        {
            return value.Replace(".", ",");
        }
    }
}