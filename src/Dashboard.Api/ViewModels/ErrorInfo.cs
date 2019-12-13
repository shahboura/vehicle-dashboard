using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dashboard.Api.ViewModels
{
    public class ErrorInfo
    {
        private readonly List<string> _messages = new List<string>();

        public ErrorInfo(ErrorType type, string message)
        {
            Type = type;
            _messages.Add(message);
        }

        public ErrorInfo(ErrorType type, IEnumerable<string> messages)
        {
            Type = type;
            _messages.AddRange(messages);
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorType Type { get; }
        public IEnumerable<string> Messages => _messages;
    }
}