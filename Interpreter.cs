using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KizhiPart1
{
    public class Interpreter
    {
        private TextWriter _writer;

        private  readonly Dictionary<string, Action<string[]>> _commands;

        private  readonly Dictionary<string, int> _variables;

        private readonly string _errorMessage;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;

            _commands = new Dictionary<string, Action<string[]>>();
            _variables = new Dictionary<string, int>();
            _errorMessage = "Переменная отсутствует в памяти";

            _commands.Add("set", (parameters ) =>
            {
                if (_variables.ContainsKey(parameters[0]))
                    _variables[parameters[0]] = int.Parse(parameters[1]);
                else _variables.Add(parameters[0], int.Parse(parameters[1]));
            });
            _commands.Add("sub", (parameters) =>
            {
                if (!_variables.ContainsKey(parameters[0]))
                {
                    _writer.WriteLine(_errorMessage);
                    return;
                }

                _variables[parameters[0]] -= int.Parse(parameters[1]);
            });
            _commands.Add("print", (parameters) =>
            {
                if (!_variables.ContainsKey(parameters[0]))
                {
                    _writer.WriteLine(_errorMessage);
                    return;
                }

                _writer.WriteLine(_variables[parameters[0]]);
            });

            _commands.Add("rem", (parameters) =>
            {
                if (!_variables.ContainsKey(parameters[0]))
                {
                    _writer.WriteLine(_errorMessage);
                    return;
                }

                _variables.Remove(parameters[0]);
            });

        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            var commandName = splitedCommand[0];
            var parameters = splitedCommand.Skip(1).ToArray();

            _commands[commandName](parameters);

        }
    }
}