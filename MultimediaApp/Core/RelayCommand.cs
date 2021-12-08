using System;
using System.Windows.Input;

namespace MultimediaApp
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    // Но есть и команды, которые делегируют более сложные операции другим
    // объектам, называемым «получателями».
    //class ComplexCommand : ICommand
    //{
    //    private Receiver _receiver;

    //    // Данные о контексте, необходимые для запуска методов получателя.
    //    private string _a;

    //    private string _b;

    //    // Сложные команды могут принимать один или несколько объектов-
    //    // получателей вместе с любыми данными о контексте через конструктор.
    //    public ComplexCommand(Receiver receiver, string a, string b)
    //    {
    //        this._receiver = receiver;
    //        this._a = a;
    //        this._b = b;
    //    }

    //    // Команды могут делегировать выполнение любым методам получателя.
    //    public void Execute()
    //    {
    //        Console.WriteLine("ComplexCommand: Complex stuff should be done by a receiver object.");
    //        this._receiver.DoSomething(this._a);
    //        this._receiver.DoSomethingElse(this._b);
    //    }
    //}

    // Классы Получателей содержат некую важную бизнес-логику. Они умеют
    // выполнять все виды операций, связанных с выполнением запроса. Фактически,
    // любой класс может выступать Получателем.
    //class Receiver
    //{
    //    public void DoSomething(string a)
    //    {
    //        Console.WriteLine($"Receiver: Working on ({a}.)");
    //    }

    //    public void DoSomethingElse(string b)
    //    {
    //        Console.WriteLine($"Receiver: Also working on ({b}.)");
    //    }
    //}

    // Отправитель связан с одной или несколькими командами. Он отправляет
    // запрос команде.
    //class Invoker
    //{
    //    private ICommand _onStart;

    //    private ICommand _onFinish;

    //    // Инициализация команд
    //    public void SetOnStart(ICommand command)
    //    {
    //        this._onStart = command;
    //    }

    //    public void SetOnFinish(ICommand command)
    //    {
    //        this._onFinish = command;
    //    }

    //    // Отправитель не зависит от классов конкретных команд и получателей.
    //    // Отправитель передаёт запрос получателю косвенно, выполняя команду.
    //    public void DoSomethingImportant()
    //    {
    //        Console.WriteLine("Invoker: Does anybody want something done before I begin?");
    //        if (this._onStart is ICommand)
    //        {
    //            this._onStart.Execute();
    //        }

    //        Console.WriteLine("Invoker: ...doing something really important...");

    //        Console.WriteLine("Invoker: Does anybody want something done after I finish?");
    //        if (this._onFinish is ICommand)
    //        {
    //            this._onFinish.Execute();
    //        }
    //    }
    //}

}
