namespace PdfTools.Commands
{
    // this interface defines the basic functionality of a 
    // generic command, this is a check if the command can be
    // executed and the execution itself.
    public interface ICommand
    {
        // lets put some usage notes so every command knows what to do
        string Usage { get; }

        // checks if the parameter suit the expected count. 
        // in out PdfTools this could be a string[].
        bool CanExecute(object context);

        void Execute(object context);
    }
}
