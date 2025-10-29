using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.Models
{
    public class AddMediaRequest
    {
        public string MediaId { get; set; } = null!;
    }

}