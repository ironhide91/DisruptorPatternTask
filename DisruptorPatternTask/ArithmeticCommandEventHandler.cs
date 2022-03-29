using Disruptor;
using System.Diagnostics;

namespace DisruptorPatternTask
{
    class ArithmeticCommandEventHandler : IWorkHandler<ArithmeticCommand>
    {
        private readonly Dictionary<string, IArithmeticCommandProcessor> processors =
            new Dictionary<string, IArithmeticCommandProcessor>();

        private readonly string CodeUnkownCommand = "OperationNotFound";
        private readonly string CodeSuccess = "Success";
        private readonly string CodeError = "Error";

        public ArithmeticCommandEventHandler()
        {

        }

        public void RegisterCommandProcessor(IArithmeticCommandProcessor processor)
        {
            if (processors.ContainsKey(processor.Key))
            {
                processors[processor.Key] = processor;
                return;
            }

            processors.Add(processor.Key, processor);
        }

        public void OnEvent(ArithmeticCommand evt)
        {
            var timer = new Stopwatch();
            timer.Start();

            if (!TryParse(evt))
            {
                timer.Stop();
                evt.Code = CodeError;
                evt.ExecutionDuration = timer.ElapsedMilliseconds;
                return;
            }

            if (!CanExecute(evt))
            {                
                timer.Stop();                
                evt.Code = CodeUnkownCommand;
                evt.ExecutionDuration = timer.ElapsedMilliseconds;
                return;
            }

            Execute(evt);            
            timer.Stop();            
            evt.Code = CodeSuccess;
            evt.ExecutionDuration = timer.ElapsedMilliseconds;
        }                

        private bool TryParse(ArithmeticCommand command)
        {
            ReadOnlySpan<char> span = command.RawTextCommand;

            int start = 0;

            var splits = new List<(int Start, int Length)>();

            int zeroIndexLength = span.Length - 1;

            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == ',')
                {
                    int length = i == 0 ? 1 : (i - start);

                    splits.Add((start, length));

                    start = i + 1;

                    continue;
                }

                if (i == zeroIndexLength)
                {
                    splits.Add((start, span.Length - start));
                }
            }

            if (splits.Count != 4)
                return false;

            command.Id = new string(span.Slice(0, splits[0].Length));

            command.Operation = new string(span.Slice(splits[1].Start, splits[1].Length));

            if (!int.TryParse(span.Slice(splits[2].Start, splits[2].Length), out int value1))
                return false;
            command.Value1 = value1;

            if (!int.TryParse(span.Slice(splits[3].Start, splits[3].Length), out int value2))
                return false;
            command.Value2 = value2;

            return true;
        }

        private bool CanExecute(ArithmeticCommand command)
        {
            switch (command.Operation)
            {
                case "Sum":
                case "Diff":
                    return true;
                default:
                    return false;
            }
        }

        private void Execute(ArithmeticCommand command)
        {
            if (processors.ContainsKey(command.Operation))
            {
                processors[command.Operation].Process(command);
                return;
            }
        }
    }
}