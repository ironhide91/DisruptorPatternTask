namespace DisruptorPatternTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var arithmeticCommandEventHandler = new ArithmeticCommandEventHandler();
            arithmeticCommandEventHandler.RegisterCommandProcessor(new SumCommandProcessor());
            arithmeticCommandEventHandler.RegisterCommandProcessor(new DiffCommandProcessor());

            var calculator = new DisruptorCalculator(
                new SimulateBusyWorkHandler(2000),
                arithmeticCommandEventHandler,
                new ConsoleWorkHandler(),
                new MetericWorkHandler());

            calculator.Initialize(ringBufferSize: 1024);

            while (true)
            {
                var command = Console.ReadLine();

                if (command == "exit")
                    break;

                calculator.Calculate(command);
            }

            calculator.ShutDown();
            Console.WriteLine("press any key to exit ...");
            Console.ReadKey();
        }
    }
}