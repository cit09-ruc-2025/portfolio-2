using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceLayer.DTOs.Requests
{
    public class DeletePlaylistDTO
    {
        public Guid PlaylistId { get; set; }
    }

}