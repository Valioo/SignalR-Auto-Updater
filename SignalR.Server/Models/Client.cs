using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Server.Models
{
    public class Client
    {
        [Key]
        public string ClientHwid { get; set; }
        public bool IsBetaTester { get; set; }
    }
}
