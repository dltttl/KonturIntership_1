using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KizhiPart1
{
    public class Interpreter
    {
        private readonly TextWriter _writer;

        private readonly Dictionary<string, Action<string[]>> _commands;

        private readonly Dictionary<string, int> _variables;

        private const string ErrorMessage = "Переменная отсутствует в памяти";

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
            _variables = new Dictionary<string, int>();
            _commands = new Dictionary<string, Action<string[]>>
            {
                ["set"] = (parameters) => Set(parameters[0], int.Parse(parameters[1])),
                ["sub"] = (parameters) => Sub(parameters[0], int.Parse(parameters[1])),
                ["print"] = (parameters) => Print(parameters[0]),
                ["rem"] = (parameters)=> Remove(parameters[0])
            };
        }

        private void ExecuteLine(string command)
        {
            var parsedCommand = command.Split(' ');

            var commandType = parsedCommand.First();
            var commandParameters = parsedCommand.Skip(1).ToArray();

            _commands[commandType](commandParameters);
        }

        private void Set(string variableName, int settingValue)
        {
            _variables[variableName] = settingValue;
        }

        private void Sub(string variableName, int subbingValue)
        {
            if (!_variables.ContainsKey(variableName))
            {
                _writer.WriteLine(ErrorMessage);
                return;
            }

            _variables[variableName] -= subbingValue;
        }

        private void Print(string variableName)
        {
            _writer.WriteLine(_variables.TryGetValue(variableName, out var value) ? value.ToString() : ErrorMessage);
        }

        private void Remove(string variableName)
        {
            if (!_variables.ContainsKey(variableName))
            {
                _writer.WriteLine(ErrorMessage);
                return;
            }

            _variables.Remove(variableName);
        }
    }
}