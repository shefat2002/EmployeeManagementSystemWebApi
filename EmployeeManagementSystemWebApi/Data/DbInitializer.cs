//namespace EmployeeManagementSystemWebApi.Data;

//public class DbInitializer
//{
//    public static void Initialize(EmployeeDbContext context)
//    {
//        context.Database.EnsureCreated();

//        // Department
//        if (!context.Departments.Any())
//        {
//            var departments = new[]
//            {
//                new Models.Department { Name = "Human Resources", Description = "Handles employee relations and benefits" },
//                new Models.Department { Name = "Development", Description = "Responsible for software development" },
//                new Models.Department { Name = "Testing", Description = "Ensures software quality through testing" }
//            };
//            context.Departments.AddRange(departments);
//            context.SaveChanges();
//        }

//        // JobRole
//        if (!context.JobRoles.Any())
//        {
//            var jobRoles = new[]
//            {
//                new Models.JobRole { JobTitle = "Administrator", Description = "Manages system settings and user access", BaseSalary = 260000 },
//                new Models.JobRole { JobTitle = "Manager", Description = "Oversees team operations and performance", BaseSalary = 240000 },
//                new Models.JobRole { JobTitle = "HR", Description = "Handles employee relations and benefits", BaseSalary = 200000 },
//                new Models.JobRole { JobTitle = "Team Lead", Description = "Leads a team of developers", BaseSalary = 220000 },
//                new Models.JobRole { JobTitle = "Developer", Description = "Writes and maintains code", BaseSalary = 180000 },
//                new Models.JobRole { JobTitle = "Tester", Description = "Tests software for bugs and issues", BaseSalary = 160000 }
//            };
//            context.JobRoles.AddRange(jobRoles);
//            context.SaveChanges();
//        }

//        // Employee
//        if (!context.Employees.Any())
//        {
//            var adminRole = context.JobRoles.FirstOrDefault(j => j.JobTitle == "Administrator");
//            var adminEmployee = new Models.Employee
//            {
//                Name = "Admin User",
//                Email = "admin@email.com",
//                DepartmentId = context.Departments.First().Id,
//                JobRoleId = context.JobRoles.First().Id,
                
//            };
//            context.Employees.Add(adminEmployee);
//            context.SaveChanges();
//        }
