using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ProfileImg { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiryTime { get; set; }

    public bool IsActive { get; set; }
    public bool? RememberMe { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? UserName { get; set; }

    public string? Username1 { get; set; }

    public string? Password { get; set; }

    public string? ResetCode { get; set; }

    public int RoleId { get; set; }

    public string? ZipCode { get; set; }
    public string? RefreshToken { get; set; }

    public int? Country { get; set; }

    public int? State { get; set; }

    public int? City { get; set; }

    public virtual ICollection<Auditlog> Auditlogs { get; } = new List<Auditlog>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; } = new List<Category>();

    public virtual City? CityNavigation { get; set; }

    public virtual Country? CountryNavigation { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerUpdatedByNavigations { get; } = new List<Customer>();

    public virtual ICollection<User> InverseCreatedByNavigation { get; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; } = new List<User>();

    public virtual ICollection<Item> ItemCreatedByNavigations { get; } = new List<Item>();

    public virtual ICollection<Item> ItemUpdatedByNavigations { get; } = new List<Item>();

    public virtual ICollection<Modifier> ModifierCreatedByNavigations { get; } = new List<Modifier>();

    public virtual ICollection<ModifierGroup> ModifierGroupCreatedByNavigations { get; } = new List<ModifierGroup>();

    public virtual ICollection<ModifierGroup> ModifierGroupUpdatedByNavigations { get; } = new List<ModifierGroup>();

    public virtual ICollection<ModifierItem> ModifierItemCreatedByNavigations { get; } = new List<ModifierItem>();

    public virtual ICollection<ModifierItem> ModifierItemUpdatedByNavigations { get; } = new List<ModifierItem>();

    public virtual ICollection<Modifier> ModifierUpdatedByNavigations { get; } = new List<Modifier>();

    public virtual ICollection<Order> OrderCreatedByNavigations { get; } = new List<Order>();

    public virtual ICollection<Order> OrderUpdatedByNavigations { get; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual ICollection<Permission> PermissionCreatedByNavigations { get; } = new List<Permission>();

    public virtual ICollection<Permission> PermissionUpdatedByNavigations { get; } = new List<Permission>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Section> SectionCreatedByNavigations { get; } = new List<Section>();

    public virtual ICollection<Section> SectionUpdatedByNavigations { get; } = new List<Section>();

    public virtual State? StateNavigation { get; set; }

    public virtual ICollection<Table> TableCreatedByNavigations { get; } = new List<Table>();

    public virtual ICollection<Table> TableUpdatedByNavigations { get; } = new List<Table>();

    public virtual ICollection<Tax> TaxCreatedByNavigations { get; } = new List<Tax>();

    public virtual ICollection<Tax> TaxUpdatedByNavigations { get; } = new List<Tax>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<WaitingToken> WaitingTokenCreatedByNavigations { get; } = new List<WaitingToken>();

    public virtual ICollection<WaitingToken> WaitingTokenUpdatedByNavigations { get; } = new List<WaitingToken>();
}
