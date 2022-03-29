using Disruptor;

namespace DisruptorPatternTask
{
    class ConsoleWorkHandler : IWorkHandler<ArithmeticCommand>
    {
        public ConsoleWorkHandler()
        {

        }

        public void OnEvent(ArithmeticCommand command)
        {
            string message = $"Operation Id: {command.Id}, Code: {command.Code}, Result: {command.Result}";
            Console.WriteLine(message);
        }
    }
}