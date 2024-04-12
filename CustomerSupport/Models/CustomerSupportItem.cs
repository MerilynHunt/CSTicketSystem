using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Models;

public class CustomerSupportItem
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string Description { get; set; }

    public DateTime CreationTime { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

	public string FormattedCreationTime => CreationTime.ToString("dd-MM-yyyy HH:mm");
	public string FormattedDueDate => DueDate.ToString("dd-MM-yyyy HH:mm");
}
