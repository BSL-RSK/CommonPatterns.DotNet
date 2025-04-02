using System;
using FluentAssertions;

namespace Common.Tests.CustomAssertions;

/// <summary>
/// Extension methods for asserting expected exception behavior with helpful syntax.
/// Useful for simplifying exception validation in tests.
/// </summary>
public static class ExceptionAssertionExtensions
{
    /// <summary>
    /// Asserts that an action throws an exception of the expected type with the specified message.
    /// </summary>
    public static void ShouldThrowWithMessage<TException>(this Action action, string expectedMessage)
        where TException : Exception
    {
        action.Should().Throw<TException>()
            .WithMessage(expectedMessage);
    }

    /// <summary>
    /// Asserts that an action throws an exception of the expected type, containing a partial message.
    /// </summary>
    public static void ShouldThrowContaining<TException>(this Action action, string partialMessage)
        where TException : Exception
    {
        action.Should().Throw<TException>()
            .Where(ex => ex.Message.Contains(partialMessage));
    }

    /// <summary>
    /// Asserts that an exception was thrown with a specific inner exception type.
    /// </summary>
    public static void ShouldHaveInnerException<TOuter, TInner>(this Action action)
        where TOuter : Exception
        where TInner : Exception
    {
        action.Should().Throw<TOuter>()
            .Which.InnerException.Should().BeOfType<TInner>();
    }

    /// <summary>
    /// Asserts that an action throws exactly the specified exception type (not a derived type).
    /// </summary>
    public static void ShouldThrowExactly<TException>(this Action action)
        where TException : Exception
    {
        action.Should().ThrowExactly<TException>();
    }

    /// <summary>
    /// Asserts that the exception's message contains the specified keyword.
    /// </summary>
    public static void ShouldThrowWithMessageContaining<TException>(this Action action, string keyword)
        where TException : Exception
    {
        action.Should().Throw<TException>()
            .Where(ex => ex.Message.Contains(keyword),
                   $"Expected exception message to contain: \"{keyword}\"");
    }

    /// <summary>
    /// Asserts that an inner exception of a specific type also contains a specific message.
    /// </summary>
    public static void ShouldHaveInnerExceptionWithMessage<TOuter, TInner>(this Action action, string expectedInnerMessage)
        where TOuter : Exception
        where TInner : Exception
    {
        action.Should().Throw<TOuter>()
            .Which.InnerException.Should().BeOfType<TInner>()
            .Which.Message.Should().Contain(expectedInnerMessage);
    }

    /// <summary>
    /// Asserts that an AggregateException contains an inner exception of a specific type.
    /// </summary>
    public static void ShouldThrowAggregateWithInner<TInner>(this Action action)
        where TInner : Exception
    {
        action.Should().Throw<AggregateException>()
            .Which.InnerExceptions.Should().Contain(e => e is TInner);
    }
}