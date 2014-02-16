using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRockies_one.Models
{
    public class CPlayer
    {
        public long PlayerId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int TeamId { get; set; }
    }
}