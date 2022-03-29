namespace DisruptorPatternTask
{
    interface IArithmeticCommandProcessor
    {
        string Key { get; }

        void Process(ArithmeticCommand command);
    }
}