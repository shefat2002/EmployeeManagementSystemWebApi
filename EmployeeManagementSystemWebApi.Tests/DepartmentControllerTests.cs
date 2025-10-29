using EmployeeManagementSystemWebApi.Controllers;
using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagementSystemWebApi.Tests;

public class DepartmentControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
    private readonly DepartmentsController _controller;
    public DepartmentControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDepartmentRepository = new Mock<IDepartmentRepository>();
        _mockUnitOfWork.Setup(uow => uow.Departments).Returns(_mockDepartmentRepository.Object);
        _controller = new DepartmentsController(_mockUnitOfWork.Object);
    }
    [Fact]
    public async Task GetDepartment_ReturnsOk_WhenDepartmentExists()
    {
        int testId = 1;
        var fakeDepartment = new Department { Id = testId, Name = "Test Department", Description = "Test Description" };
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(fakeDepartment);

        var actionResult = await _controller.GetDepartment(testId);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedDepartment = Assert.IsType<Department>(okResult.Value);

        Assert.Equal(testId, returnedDepartment.Id);
        Assert.Equal("Test Department", returnedDepartment.Name);
    }

    [Fact]
    public async Task GetDepartment_ReturnsNotFound_WhenDepartment_DoesNotExist()
    {
        int testId = 99;
        _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync((Department?)null);
        var actionResult = await _controller.GetDepartment(testId);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task PostDepartment_ReturnsCreatedAtAction_AndCallsSave()
    {
        var newDepartment = new Department { Name = "New Department", Description = "New Description" };
        _mockDepartmentRepository.Setup(repo => repo.AddAsync(newDepartment)).Callback<Department>(d => d.Id = 1);
        var actionResult = await _controller.PostDepartment(newDepartment);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        
        Assert.Equal("GetDepartment", createdAtActionResult.ActionName);
        Assert.Equal(1, createdAtActionResult.RouteValues!["id"]);
        _mockDepartmentRepository.Verify(repo => repo.AddAsync(newDepartment), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    [Fact]
    public async Task PutDepartment_ReturnsBadRequest_WhenIdMismatch()
    {
        int testId = 1;
        var departmentToUpdate = new Department { Id = 2, Name = "Updated Department", Description = "Updated Description" };
        var actionResult = await _controller.PutDepartment(testId, departmentToUpdate);
        Assert.IsType<BadRequestResult>(actionResult);
        _mockDepartmentRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Department>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

}
