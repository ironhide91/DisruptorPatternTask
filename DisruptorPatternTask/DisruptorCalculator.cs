using Disruptor;
using Disruptor.Dsl;

namespace DisruptorPatternTask
{
    class DisruptorCalculator
    {
        private readonly IWorkHandler<ArithmeticCommand> busySimulator;
        private readonly IWorkHandler<ArithmeticCommand> commandHandler;
        private readonly IWorkHandler<ArithmeticCommand> consoleHandler;
        private readonly IWorkHandler<ArithmeticCommand> metricHandler;
        private Disruptor<ArithmeticCommand> disruptor;        

        public DisruptorCalculator(
            IWorkHandler<ArithmeticCommand> busySimulator,
            IWorkHandler<ArithmeticCommand> commandHandler,
            IWorkHandler<ArithmeticCommand> consoleHandler,
            IWorkHandler<ArithmeticCommand> metricHandler)
        {
            this.busySimulator = busySimulator;
            this.commandHandler = commandHandler;
            this.consoleHandler = consoleHandler;
            this.metricHandler = metricHandler;
        }

        public void Initialize(int ringBufferSize)
        {
            disruptor = new Disruptor<ArithmeticCommand>(() => new ArithmeticCommand(), ringBufferSize);

            disruptor
                .HandleEventsWithWorkerPool(busySimulator)
                .ThenHandleEventsWithWorkerPool(commandHandler)
                .ThenHandleEventsWithWorkerPool(consoleHandler)
                .ThenHandleEventsWithWorkerPool(metricHandler);

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