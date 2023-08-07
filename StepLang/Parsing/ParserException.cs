﻿using System.Diagnostics.CodeAnalysis;

namespace StepLang.Parsing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class ParserException : Exception
{
    public ParserException(string message) : base(message)
    {
    }
}