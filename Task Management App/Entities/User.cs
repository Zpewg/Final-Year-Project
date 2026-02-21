using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Task_Management_App.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(20)")]
    [MaxLength(20)]
    public string Name { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(60)")]
    [MaxLength(60)]
    public string Email { get; set; }
    
    [Required] 
    public string Password { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(10)")]
    [MaxLength(10)]
    public string PhoneNumber { get; set; }

    [Column(TypeName = "bit")] 
    public bool Active { get; set; } = false; 
    
    
    public Point? Location { get; set; }
    
    
    public User(){}
    
    public User(int  userId, string name, string email, string password, string phoneNumber)
    {
        UserId = userId;
        Name = name;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    public User(string name, string email, string password, string phoneNumber)
    {
        Name = name;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    public override string ToString()
    {
        return $"User: {Name}, Email: {Email}, Phone: {PhoneNumber}, Status: {Active}";
    }
}