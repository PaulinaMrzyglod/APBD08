using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class ClientDTO
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string Telephone { get; set; }
    [Required]
    public string Pesel { get; set; }
}