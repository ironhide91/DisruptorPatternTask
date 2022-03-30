using Disruptor;

namespace DisruptorPatternTask
{
    class MetericWorkHandler : IWorkHandler<ArithmeticCommand>
    {
        public MetericWorkHandler()
        {

        }

        private int totalErrors;
        private int totalSuccess;
        private int totalUnkownCommand;

        public void OnEvent(ArithmeticCommand command)
        {
            switch (command.Code)
            {
                case StatusCode.Error:
                    totalErrors++;
                    break;
                case StatusCode.Success:
                    totalSuccess++;
                    break;
                case StatusCode.UnkownCommand:
                    totalUnkownCommand++;
                    break;
                default:
                    break;
            }

            string message = $"Metics Success: {totalSuccess}, Errors: {totalErrors}, UnkownCommands: {totalUnkownCommand}";
            Console.WriteLine(message);
        }
    }
}