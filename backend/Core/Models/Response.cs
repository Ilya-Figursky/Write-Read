using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Response
    {
        public string Message { get; set; }
        public Response(string message) 
        { 
            if(message == "Bad" || message == "Successful") Message = message;
            else
            {
                throw new ArgumentException("Message must be either 'Bad' or 'Successful': ", nameof(message));
            }
        }
    }
}
