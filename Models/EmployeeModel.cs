using System.ComponentModel.DataAnnotations.Schema;

public class EmployeeModel: BaseModel
{
    public int EmployeeId {get;set;}
    public string? EmployeeName {get;set;}
    public string? JobTitle {get;set;}
    public DateTime? BirthDate {get;set;}
    public char? Gender {get;set;}
    public DateTime? HireDate {get;set;}

    [ForeignKey("Id")]
    public virtual UserModel User { get; set; }
}