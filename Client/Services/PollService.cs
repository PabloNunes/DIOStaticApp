using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorBasic.Services
{
    public class PollService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string STORAGE_KEY = "poll_votes";

        public PollService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<Dictionary<string, int>> GetVotesAsync()
        {
            try
            {
                var votesJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", STORAGE_KEY);
                if (string.IsNullOrEmpty(votesJson))
                {
                    return new Dictionary<string, int>
                    {
                        { "Agentic AI", 0 },
                        { "Simple LLMs deployment", 0 },
                        { "MCP Model Context protocol", 0 }
                    };
                }
                return JsonSerializer.Deserialize<Dictionary<string, int>>(votesJson) ?? new Dictionary<string, int>();
            }
            catch
            {
                return new Dictionary<string, int>
                {
                    { "Agentic AI", 0 },
                    { "Simple LLMs deployment", 0 },
                    { "MCP Model Context protocol", 0 }
                };
            }
        }

        public async Task VoteAsync(string option)
        {
            var votes = await GetVotesAsync();
            if (votes.ContainsKey(option))
            {
                votes[option]++;
                var votesJson = JsonSerializer.Serialize(votes);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", STORAGE_KEY, votesJson);
            }
        }

        public async Task<bool> HasVotedAsync()
        {
            try
            {
                var hasVoted = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "has_voted");
                return hasVoted == "true";
            }
            catch
            {
                return false;
            }
        }

        public async Task SetVotedAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "has_voted", "true");
        }

        public async Task ResetVoteAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "has_voted");
        }
    }
}