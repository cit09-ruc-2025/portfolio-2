using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Dtos
{
    public class AddToRecent
    {
        public string? MediaId { get; set; } = "";
        public string? PeopleId { get; set; } = "";
    }
}