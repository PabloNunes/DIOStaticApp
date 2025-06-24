using BlazorBasic.Services;
using Microsoft.JSInterop;
using Moq;
using System.Text.Json;

namespace BlazorBasic.Tests;

public class PollServiceTests
{
    private readonly Mock<IJSRuntime> _mockJSRuntime;
    private readonly PollService _pollService;

    public PollServiceTests()
    {
        _mockJSRuntime = new Mock<IJSRuntime>();
        _pollService = new PollService(_mockJSRuntime.Object);
    }

    [Fact]
    public async Task GetVotesAsync_WhenNoStoredData_ReturnsDefaultVotes()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _pollService.GetVotesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(0, result["Agentic AI"]);
        Assert.Equal(0, result["Simple LLMs deployment"]);
        Assert.Equal(0, result["MCP Model Context protocol"]);
    }

    [Fact]
    public async Task GetVotesAsync_WhenStoredDataExists_ReturnsStoredVotes()
    {
        // Arrange
        var expectedVotes = new Dictionary<string, int>
        {
            { "Agentic AI", 5 },
            { "Simple LLMs deployment", 3 },
            { "MCP Model Context protocol", 7 }
        };
        var votesJson = JsonSerializer.Serialize(expectedVotes);
        
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ReturnsAsync(votesJson);

        // Act
        var result = await _pollService.GetVotesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedVotes.Count, result.Count);
        Assert.Equal(expectedVotes["Agentic AI"], result["Agentic AI"]);
        Assert.Equal(expectedVotes["Simple LLMs deployment"], result["Simple LLMs deployment"]);
        Assert.Equal(expectedVotes["MCP Model Context protocol"], result["MCP Model Context protocol"]);
    }

    [Fact]
    public async Task GetVotesAsync_WhenJSRuntimeThrows_ReturnsDefaultVotes()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ThrowsAsync(new Exception("JS Runtime error"));

        // Act
        var result = await _pollService.GetVotesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(0, result["Agentic AI"]);
        Assert.Equal(0, result["Simple LLMs deployment"]);
        Assert.Equal(0, result["MCP Model Context protocol"]);
    }

    [Fact]
    public async Task HasVotedAsync_WhenUserHasVoted_ReturnsTrue()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ReturnsAsync("true");

        // Act
        var result = await _pollService.HasVotedAsync();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HasVotedAsync_WhenUserHasNotVoted_ReturnsFalse()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ReturnsAsync("false");

        // Act
        var result = await _pollService.HasVotedAsync();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HasVotedAsync_WhenJSRuntimeThrows_ReturnsFalse()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ThrowsAsync(new Exception("JS Runtime error"));

        // Act
        var result = await _pollService.HasVotedAsync();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetVotesAsync_WithEmptyString_ReturnsDefaultVotes()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ReturnsAsync("");

        // Act
        var result = await _pollService.GetVotesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(0, result["Agentic AI"]);
        Assert.Equal(0, result["Simple LLMs deployment"]);
        Assert.Equal(0, result["MCP Model Context protocol"]);
    }

    [Fact]
    public async Task GetVotesAsync_WithInvalidJson_ReturnsDefaultVotes()
    {
        // Arrange
        _mockJSRuntime.Setup(x => x.InvokeAsync<string>(
            "localStorage.getItem", 
            It.IsAny<object[]>()))
            .ReturnsAsync("invalid json");

        // Act
        var result = await _pollService.GetVotesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(0, result["Agentic AI"]);
        Assert.Equal(0, result["Simple LLMs deployment"]);
        Assert.Equal(0, result["MCP Model Context protocol"]);
    }
}