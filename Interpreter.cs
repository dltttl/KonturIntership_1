using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

namespace KizhiPart1
{
    public class Interpreter
    {
        private readonly TextWriter _writer;

        private  readonly Dictionary<string, Action<string[]>> _commands;

        private  readonly Dictionary<string, int> _variables;

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
                ["rem"] = (parameters)=>Remove(parameters[0])
            };
        }

        public void ExecuteLine(string command)
        {
            var parsedCommand = command.Split(' ');

            var commandType = parsedCommand.First();
            var commandParameters = parsedCommand.Skip(1).ToArray();

            _commands[commandType](commandParameters);
        }

        public void Set(string variableName, int settingValue)
        {
            _variables[variableName] = settingValue;
        }

        public void Sub(string variableName, int subbingValue)
        {
            if (!_variables.ContainsKey(variableName))
            {
                _writer.WriteLine(ErrorMessage);
                return;
            }

            _variables[variableName] -= subbingValue;
        }

        public void Print(string variableName)
        {
            if (!_variables.ContainsKey(variableName))
            {
                _writer.WriteLine(ErrorMessage);
                return;
            }

            _writer.WriteLine(_variables[variableName]);
        }

        public void Remove(string variableName)
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