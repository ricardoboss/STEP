﻿using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tokenizing;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class TokenizerException : Exception
{
    public TokenizerException(string message) : base(message)
    {
    }
}