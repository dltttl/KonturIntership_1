using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KizhiPart1
{
    public class Interpreter
    {
        private TextWriter _writer;

        private  readonly Dictionary<string, Action<string, int>> _commands;

        private  readonly Dictionary<string, int> _variables;

        private readonly string _errorMessage;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;

            _commands = new Dictionary<string, Action<string, int>>();
            _variables = new Dictionary<string, int>();
            _errorMessage = "Переменная отсутствует в памяти";

            _commands.Add("set", (variable, parameter ) =>
            {
                if (_variables.ContainsKey(variable))
                    _variables[variable] = parameter;
                else _variables.Add(variable, parameter);
            });
            _commands.Add("sub", (variable, parameter) =>
            {
                if (!_variables.ContainsKey(variable))
                {
                    _writer.WriteLine(_errorMessage);
                    return;
                }

                _variables[variable] -= parameter;
            });
            _commands.Add("print", (variable, parameter) =>
            {
                if (!_variables.ContainsKey(variable))
                {
                    _writer.WriteLine(_errorMessage);
                    return;
                }

                _writer.WriteLine(_variables[variable]);
            });

            _commands.Add("rem", (variable, parameter) =>
            {
                if (!_variables.ContainsKey(variable))
                {
                    _writer.WriteLine(_errorMessage);
                    return;
                }

                _variables.Remove(variable);
            });

        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');

            _commands[splitedCommand[0]](splitedCommand[1],
                splitedCommand.Length == 3 ? int.Parse(splitedCommand[2]) : 0);

        }
    }
}