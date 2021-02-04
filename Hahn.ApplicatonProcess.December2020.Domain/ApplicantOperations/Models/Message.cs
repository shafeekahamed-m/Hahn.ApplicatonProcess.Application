using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models
{
    public class Message
    {
        public Message()
        {
            Messages = new List<string>();
        }
        public int Code { get; set; }
        public List<string> Messages { get; set; }
    }
}
