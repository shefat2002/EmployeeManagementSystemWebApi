using EmployeeMVC_Consume.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMVC_Consume.Controllers;

public class DepartmentsController : Controller
{
   private readonly HttpClient _httpClient;
   public DepartmentsController(IHttpClientFactory httpClient)
   {
      _httpClient = httpClient.CreateClient();
      _httpClient.BaseAddress = new Uri("https://localhost:7164/api/"); // Replace with your API base address
      _httpClient.DefaultRequestHeaders.Accept.Clear();
      _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
   }
   public async Task<IActionResult> Index()
   {
      var response = await _httpClient.GetAsync("Departments");
      if (response.IsSuccessStatusCode)
      {
         var departments = await response.Content.ReadFromJsonAsync<IEnumerable<DepartmentVM>>();
         return View(departments);
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
   public async Task<IActionResult> Create(DepartmentVM department)
   {
      if (ModelState.IsValid)
      {
         var response = await _httpClient.PostAsJsonAsync<DepartmentVM>("Departments", department);
         if (response.IsSuccessStatusCode)
         {
            return RedirectToAction("Index");
         }
         else
         {
            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
         }
      }
      return View(department);
   }
   public async Task<IActionResult> Edit(int id)
   {
      var response = await _httpClient.GetAsync($"Departments/{id}");
      if (response.IsSuccessStatusCode)
      {
         var department = await response.Content.ReadFromJsonAsync<DepartmentVM>();
         return View(department);
      }
      else
      {
         ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
         return RedirectToAction("Index");
      }
   }
   [HttpPost]
   public async Task<IActionResult> Edit(DepartmentVM department)
   {
      if (ModelState.IsValid)
      {
         var response = await _httpClient.PutAsJsonAsync<DepartmentVM>($"Departments/{department.Id}", department);
         if (response.IsSuccessStatusCode)
         {
            return RedirectToAction("Index");
         }
         else
         {
            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
         }
      }
      return View(department);
   }
   public async Task<IActionResult> Delete(int id)
   {
      var response = await _httpClient.DeleteAsync($"Departments/{id}");
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
