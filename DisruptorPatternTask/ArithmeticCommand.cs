namespace DisruptorPatternTask
{
    class ArithmeticCommand
    {
        public ArithmeticCommand()
        {

        }

        public string RawTextCommand  { get; set; }
        public string Id { get; set; }
        public string Operation { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public string Code { get; set; }
        public int? Result { get; set; }
        public long ExecutionDuration { get; set; }
    }

}