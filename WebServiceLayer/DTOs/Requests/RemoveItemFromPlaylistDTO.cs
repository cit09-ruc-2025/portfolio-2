using System;
using System.ComponentModel.DataAnnotations;

namespace WebServiceLayer.DTOs.Requests
{
    public class RemoveItemFromPlaylistDTO
    {
        [Required]
        public string ItemId { get; set; } = null!;
        [Required]
        public bool IsMedia { get; set; }
    }
}
