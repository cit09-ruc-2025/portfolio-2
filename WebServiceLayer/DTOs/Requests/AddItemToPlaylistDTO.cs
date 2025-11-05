using System;
using System.ComponentModel.DataAnnotations;

namespace WebServiceLayer.DTOs.Requests
{
    public class AddItemToPlaylistDTO
    {
        [Required]
        public string ItemId { get; set; } = null!;
        [Required]
        public bool IsMedia { get; set; }
    }
}
