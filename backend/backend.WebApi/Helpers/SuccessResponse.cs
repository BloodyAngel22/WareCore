using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace backend.WebApi.Helpers
{
    public class SuccessResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;

        [JsonPropertyName("data")]
        public required T Data { get; set; }
    }
}