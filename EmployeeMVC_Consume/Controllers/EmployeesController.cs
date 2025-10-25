using EmployeeMVC_Consume.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace EmployeeMVC_Consume.Controllers;


public class EmployeesController : Controller
{
   private readonly HttpClient _httpClient;
   public EmployeesController(IHttpClientFactory httpClient)
   {
      _httpClient = httpClient.CreateClient();
      _httpClient.BaseAddress = new Uri("https://localhost:7164/api/"); // Replace with your API base address
      _httpClient.DefaultRequestHeaders.Accept.Clear();
      _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
   }
    public async Task<IActionResult> Index()
   {

      var response = await _httpClient.GetAsync("Employees");
      if (response.IsSuccessStatusCode)
      {
         var employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeVM>>();
         return View(employees);
      }
      else
      {
         ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
      }
      return View();
   }
   public IActionResult Create()
   {
      return View();
   }
   [HttpPost]
    public async Task<IActionResult> Create(EmployeeVM employee)
   {
      if (ModelState.IsValid)
      {

         var response = await _httpClient.PostAsJsonAsync<EmployeeVM>("Employees", employee);
         
         if (response.IsSuccessStatusCode)
         {
            return RedirectToAction("Index");
         }
         else
         {
            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
         }
      }
      else
      {
           ModelState.AddModelError(string.Empty, "Model state is invalid.");
         return RedirectToAction("Create");
      }
      return View(employee);
   }
   public async Task<IActionResult> Edit(int id)
   {
      var response = await _httpClient.GetAsync($"Employees/{id}");
      if (response.IsSuccessStatusCode)
      {
         var employee = await response.Content.ReadFromJsonAsync<EmployeeVM>(); 
         
         return View(employee);
      }
      else
      {
         ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
         return RedirectToAction("Index");
      }
   }
   [HttpPost]
    public async Task<IActionResult> Edit(int id, int departmentId, int jobRoleId, EmployeeVM employee)
   {
      if (!ModelState.IsValid)
    {
        return View(employee);
    }
      employee.DepartmentId = departmentId;
      employee.JobRoleId = jobRoleId;
    var updateResponse = await _httpClient.PutAsJsonAsync<EmployeeVM>($"Employees/{id}", employee);
    
    if (updateResponse.IsSuccessStatusCode)
    {
        return RedirectToAction("Index");
    }
    else
    {
        ModelState.AddModelError(string.Empty, $"Error: {updateResponse.StatusCode} - {updateResponse.ReasonPhrase}");
        return View(employee);
    }
}
   public async Task<IActionResult> Delete(int id)
   {
      var response = await _httpClient.DeleteAsync($"Employees/{id}");
      if (response.IsSuccessStatusCode)
      {
         return RedirectToAction("Index");
      }
      else
      {
         ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
         return RedirectToAction("Index");
      }
   }
}
