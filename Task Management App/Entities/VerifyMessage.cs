namespace Task_Management_App.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class VerifyMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(6)")]
    public string VerifysMessage {get; set;}
    
    [Required]
    [Column(TypeName = "int")]
    public int UsersId {get; set;}
    
    [ForeignKey("UsersId")]
    public User User {get; set;}
    
    [Required]
    public DateTime CurrentTime {get; set;} = DateTime.Now;
    
    [Column(TypeName = "varchar(20)")]
    public CodeType code {get; set;}
    public VerifyMessage(string verifyMessage, int usersId, CodeType code)
    {
        this.VerifysMessage = verifyMessage;
        this.UsersId = usersId;
        this.code = code;
    }

    public VerifyMessage()
    {
        
    }

    public override string ToString()
    {
        return this.VerifysMessage;
    }
    
}