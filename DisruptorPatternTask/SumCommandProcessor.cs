namespace DisruptorPatternTask
{
    class SumCommandProcessor : IArithmeticCommandProcessor
    {
        public string Key => "Sum";

        public void Process(ArithmeticCommand command)
        {
            command.Result = command.Value1 + command.Value2;
        }
    }
}