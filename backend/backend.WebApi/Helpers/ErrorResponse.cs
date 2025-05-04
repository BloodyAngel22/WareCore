using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace backend.WebApi.Helpers
{
    public class ErrorResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}