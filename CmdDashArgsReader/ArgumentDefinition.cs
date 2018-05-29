namespace Mj.CmdDashArgsReaderLibrary
{
    public class ArgumentDefinition
    {
        public ArgumentDefinition(string argument, string name)
        {
            Argument = argument;
            Name = name;
        }

        public string Argument { get; set; }  //without dash
        public string Name { get; set; }
    }
}
