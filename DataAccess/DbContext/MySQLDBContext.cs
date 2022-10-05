using Microsoft.EntityFrameworkCore;

public class MySQLDBContext: DbContext
{
    public DbSet<UserModel> User {get;set;}
    public DbSet<EmployeeModel> Employee {get;set;}
    public MySQLDBContext(DbContextOptions<MySQLDBContext> options): base (options)
    {
        
    }
}