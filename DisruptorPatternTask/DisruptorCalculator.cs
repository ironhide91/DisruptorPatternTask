using Disruptor;
using Disruptor.Dsl;

namespace DisruptorPatternTask
{
    class DisruptorCalculator
    {
        private readonly IWorkHandler<ArithmeticCommand> busySimulator;
        private readonly IWorkHandler<ArithmeticCommand> commandHandler;
        private readonly IWorkHandler<ArithmeticCommand> consoleHandler;
        private Disruptor<ArithmeticCommand> disruptor;        

        public DisruptorCalculator(
            IWorkHandler<ArithmeticCommand> busySimulator,
            IWorkHandler<ArithmeticCommand> commandHandler,
            IWorkHandler<ArithmeticCommand> consoleHandler)
        {
            this.busySimulator = busySimulator;
            this.commandHandler = commandHandler;
            this.consoleHandler = consoleHandler;
        }

        public void Initialize(int ringBufferSize)
        {
            disruptor = new Disruptor<ArithmeticCommand>(() => new ArithmeticCommand(), ringBufferSize);

            disruptor
                .HandleEventsWithWorkerPool(busySimulator)
                .ThenHandleEventsWithWorkerPool(commandHandler)
                .ThenHandleEventsWithWorkerPool(consoleHandler);

            disruptor.Start();
        }

        public void Calculate(string command)
        {
            using (var scope = disruptor.PublishEvent())
            {
                var evt = scope.Event();
                evt.RawTextCommand = command;
            }
        }

        public void ShutDown()
        {
            disruptor?.Shutdown();
        }
    }
}