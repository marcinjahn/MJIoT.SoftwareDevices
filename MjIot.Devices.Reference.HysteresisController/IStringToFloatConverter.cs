namespace MjIot.Devices.Reference.HysteresisController
{
    public interface IStringToFloatConverter
    {
        bool IsNumeric(string value);
        float Convert(string value);
    }
}