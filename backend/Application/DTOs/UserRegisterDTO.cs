using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class UserRegisterDTO
    {
        [JsonPropertyName("login")]
        public string Name { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
