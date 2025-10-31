using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Data;

public class DbInitializer
{
    public static void Initialize(EmployeeDbContext context)
    {
        context.Database.EnsureCreated();

        // Department
        if (!context.Departments.Any())
        {
            var departments = new[]
            {
                new Models.Department { Name = "Human Resources", Description = "Handles employee relations and benefits" },
                new Models.Department { Name = "Development", Description = "Responsible for software development" },
                new Models.Department { Name = "Testing", Description = "Ensures software quality through testing" }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();
        }

        // JobRole
        if (!context.JobRoles.Any())
        {
            var jobRoles = new[]
            {
                new Models.JobRole { JobTitle = "Administrator", Description = "Manages system settings and user access", BaseSalary = 260000 },
                new Models.JobRole { JobTitle = "Manager", Description = "Oversees team operations and performance", BaseSalary = 240000 },
                new Models.JobRole { JobTitle = "HR Admin", Description = "Handles employee relations and benefits", BaseSalary = 200000 },
                new Models.JobRole { JobTitle = "Team Lead", Description = "Leads a team of developers", BaseSalary = 220000 },
                new Models.JobRole { JobTitle = "Developer", Description = "Writes and maintains code", BaseSalary = 180000 },
                new Models.JobRole { JobTitle = "Tester", Description = "Tests software for bugs and issues", BaseSalary = 160000 }
            };
            context.JobRoles.AddRange(jobRoles);
            context.SaveChanges();
        }

        // Employee
        if (!context.Employees.Any())
        {
            var adminRole = context.JobRoles.FirstOrDefault(j => j.JobTitle == "Administrator");

            CreatePasswordHash("Admin@123", out byte[] passwordHash, out byte[] passwordSalt);
            var adminEmployee = new Employee
            {
                Name = "Admin",
                Email = "admin@email.com",
                DepartmentId = context.Departments.First().Id,
                JobRoleId = context.JobRoles.First(j => j.JobTitle == "Administrator").Id,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt

            };

            var managerRole = context.JobRoles.FirstOrDefault(j => j.JobTitle == "Manager");
            CreatePasswordHash("Manager@123", out byte[] managerPasswordHash, out byte[] managerPasswordSalt);
            var managerEmployee = new Employee
            {
                Name = "Test Manager",
                Email = "manager@email.com",
                DepartmentId = context.Departments.First().Id,
                JobRoleId = context.JobRoles.First(j => j.JobTitle == "Manager").Id,
                PasswordHash = managerPasswordHash,
                PasswordSalt = managerPasswordSalt
            };

            var hrRole = context.JobRoles.FirstOrDefault(j => j.JobTitle == "HR");
            CreatePasswordHash("HRadmin@123", out byte[] hrPasswordHash, out byte[] hrPasswordSalt);
            var hrEmployee = new Employee
            {
                Name = "Test HR",
                Email = "hr@email.com",
                DepartmentId = context.Departments.First().Id,
                JobRoleId = context.JobRoles.First(j => j.JobTitle == "HR").Id,
                PasswordHash = hrPasswordHash,
                PasswordSalt = hrPasswordSalt
            };

            var teamLeadRole = context.JobRoles.FirstOrDefault(j => j.JobTitle == "Team Lead");
            CreatePasswordHash("TeamLead@123", out byte[] tlPasswordHash, out byte[] tlPasswordSalt);
            var teamLeadEmployee = new Employee
            {
                Name = "Test Team Lead",
                Email = "teamlead@email.com",
                DepartmentId = context.Departments.First().Id,
                JobRoleId = context.JobRoles.First(j => j.JobTitle == "Team Lead").Id,
                PasswordHash = tlPasswordHash,
                PasswordSalt = tlPasswordSalt
            };
            

            var developerRole = context.JobRoles.FirstOrDefault(j => j.JobTitle == "Developer");
            CreatePasswordHash("Developer@123", out byte[] devPasswordHash, out byte[] devPasswordSalt);
            var developerEmployee = new Employee
            {
                Name = "Test Developer",
                Email = "dev@email.com",
                DepartmentId = context.Departments.First().Id,
                JobRoleId = context.JobRoles.First(j => j.JobTitle == "Developer").Id,
                PasswordHash = devPasswordHash,
                PasswordSalt = devPasswordSalt
            };


            context.Employees.Add(adminEmployee);
            context.Employees.Add(managerEmployee);
            context.Employees.Add(hrEmployee);
            context.Employees.Add(teamLeadEmployee);
            context.Employees.Add(developerEmployee);
            context.SaveChanges();
        }

        

    }
    // Private helper methods for password hashing can be added here if needed
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}