using System.Net;
using System.Net.Http;
using FluentAssertions;

namespace Common.Tests.CustomAssertions;

/// <summary>
/// Extension methods for asserting common aspects of <see cref="HttpResponseMessage"/>.
/// Useful for API and integration test scenarios.
/// </summary>
public static class HttpResponseAssertionExtensions
{
    /// <summary>
    /// Asserts that the HTTP response has the expected status code.
    /// </summary>
    public static void ShouldHaveStatusCode(this HttpResponseMessage response, HttpStatusCode expected)
    {
        response.StatusCode.Should().Be(expected);
    }

    /// <summary>
    /// Asserts that the HTTP response contains the specified header.
    /// </summary>
    public static void ShouldContainHeader(this HttpResponseMessage response, string headerName)
    {
        response.Headers.Should().ContainKey(headerName);
    }

    /// <summary>
    /// Asserts that the response content contains the specified string.
    /// </summary>
    public static async Task ShouldContainInBodyAsync(this HttpResponseMessage response, string expectedContent)
    {
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain(expectedContent);
    }

    /// <summary>
    /// Asserts that the response content is empty.
    /// </summary>
    public static async Task ShouldHaveEmptyBodyAsync(this HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        body.Should().BeNullOrWhiteSpace();
    }
}