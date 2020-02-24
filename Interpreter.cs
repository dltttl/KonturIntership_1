using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

namespace KizhiPart1
{
    public class Interpreter
    {
        private TextWriter _writer;

        private  readonly Dictionary<string, Action<string, string[]>> _commands;

        private  readonly Dictionary<string, int> _variables;

        private readonly string _errorMessage;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;

            
            _variables = new Dictionary<string, int>();
            _errorMessage = "Переменная отсутствует в памяти";
            _commands = new Dictionary<string, Action<string, string[]>>
            {
                ["set"] = (variable, parameters) =>
                {
                    if (_variables.ContainsKey(variable))
                        _variables[variable] = int.Parse(parameters[0]);
                    else _variables.Add(variable, int.Parse(parameters[0]));
                },
                ["sub"] = (variable, parameters) =>
                {
                    if (!_variables.ContainsKey(variable))
                    {
                        _writer.WriteLine(_errorMessage);
                        return;
                    }

                    _variables[variable] -= int.Parse(parameters[0]);
                },
                ["print"] = (variable, parameters) =>
                {
                    if (!_variables.ContainsKey(variable))
                    {
                        _writer.WriteLine(_errorMessage);
                        return;
                    }

                    _writer.WriteLine(_variables[variable]);
                },
                ["rem"] = (variable, parameters) =>
                {
                    if (!_variables.ContainsKey(variable))
                    {
                        _writer.WriteLine(_errorMessage);
                        return;
                    }

                    _variables.Remove(variable);
                }
            };
        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            var commandName = splitedCommand[0];
            var variableName = splitedCommand[1];
            var parameters = splitedCommand.Skip(2).ToArray();

            _commands[commandName](variableName, parameters);

        }
    }
}