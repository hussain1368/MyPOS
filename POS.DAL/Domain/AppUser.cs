using System;
using System.Collections.Generic;

namespace POS.DAL.Domain;

public partial class AppUser
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string DisplayName { get; set; }

    public string UserRole { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
