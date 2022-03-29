using Disruptor;

namespace DisruptorPatternTask
{
    class SimulateBusyWorkHandler : IWorkHandler<ArithmeticCommand>
    {
        private readonly int duration;

        private readonly string message;

        public SimulateBusyWorkHandler(int duration)
        {
            this.duration = duration;
            message = $"Simulating busy for {duration} milliseconds";
        }

        public void OnEvent(ArithmeticCommand _)
        {
            Console.WriteLine(message);
            Thread.Sleep(duration);
        }
    }
}