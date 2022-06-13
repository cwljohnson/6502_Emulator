namespace ASM_Emulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            List<Instruction> instructions = parser.LoadFile("Code.asm");
            parser.WriteByteCode("Code.bin", instructions);

            
            Emulator emulator = new Emulator();
            emulator.LoadRaw("Code.bin");

            emulator.Execute();

            Console.WriteLine("Output Memory:");
            emulator.PrintRAM(0x0000, 8);

            Console.WriteLine("Output Instructions:");
            emulator.PrintInstructions();
            
        }
    }
}