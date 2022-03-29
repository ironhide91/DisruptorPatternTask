namespace DisruptorPatternTask
{
    class DiffCommandProcessor : IArithmeticCommandProcessor
    {
        public string Key => "Diff";

        public void Process(ArithmeticCommand command)
        {
            command.Result = command.Value1 - command.Value2;
        }
    }
}