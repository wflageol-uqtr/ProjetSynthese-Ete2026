// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations.Schema;

class ProgramControl
{
    private static string?[] database = { "Some", "Data", null, null, null };

    interface ICommand
    {
        void Execute();
        void Undo();
    }

    class UpdateCommand : ICommand
    {
        private int index;
        private string? newData;
        private string? oldData;

        public UpdateCommand(int index, string? newData)
        {
            this.index = index;
            this.newData = newData;
        }

        public void Execute()
        {
            oldData = database[index];
            database[index] = newData;
        }

        public void Undo()
        {
            database[index] = oldData;
        }
    }

    class CommandInvoker
    {
        Stack<ICommand> executedCommands = new();

        public void Execute(ICommand command)
        {
            executedCommands.Push(command);
            command.Execute();
        }

        public void Undo()
        {
            if (executedCommands.Peek() == null)
                throw new InvalidOperationException();
            else
                executedCommands.Pop().Undo();
        }
    }

    private static void PrintDatabase()
    {
        Console.WriteLine("Base données :");
        foreach (var data in database)
            Console.WriteLine(data);
    }

    //static void Main(string[] args)
    //{
    //    PrintDatabase();

    //    var invoker = new CommandInvoker();

    //    var command = new UpdateCommand(0, "Hello");
    //    invoker.Execute(command);

    //    var command2 = new UpdateCommand(0, "Lots");
    //    invoker.Execute(command2);

    //    PrintDatabase();

    //    invoker.Undo();

    //    PrintDatabase();
    //}
}