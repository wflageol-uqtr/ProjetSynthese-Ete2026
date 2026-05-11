// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;

class ProgramTreatment
{
    private static ImmutableArray<string?> database = ["Some", "Data", null, null, null];

    interface ICommand
    {
        void Execute();
    }

    class UpdateCommand : ICommand
    {
        private readonly int index;
        private readonly string? newData;

        public UpdateCommand(int index, string? newData)
        {
            this.index = index;
            this.newData = newData;
        }

        public void Execute()
        {
            database = database.SetItem(index, newData);
        }
    }

    class CommandInvoker
    {
        Stack<ImmutableArray<string?>> databaseHistory = new();

        public void Execute(ICommand command)
        {
            databaseHistory.Push(database);
            command.Execute();
        }

        public void Undo()
        {
            if (databaseHistory.Peek() == null)
                throw new InvalidOperationException();
            else
                database = databaseHistory.Pop();
        }
    }

    private static void PrintDatabase()
    {
        Console.WriteLine("Base données :");
        foreach (var data in database)
            Console.WriteLine(data);
    }

    static void Main(string[] args)
    {
        PrintDatabase();

        var invoker = new CommandInvoker();

        var command = new UpdateCommand(0, "Hello");
        invoker.Execute(command);

        var command2 = new UpdateCommand(0, "Lots");
        invoker.Execute(command2);

        PrintDatabase();

        invoker.Undo();

        PrintDatabase();
    }
}