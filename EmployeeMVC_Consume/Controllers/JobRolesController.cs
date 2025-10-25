using EmployeeMVC_Consume.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMVC_Consume.Controllers;

public class JobRolesController : Controller
{
    private readonly HttpClient _httpClient;
    public JobRolesController(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7164/api/"); // Replace with your API base address
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("JobRoles");
        if (response.IsSuccessStatusCode)
        {
            var jobRoles = await response.Content.ReadFromJsonAsync<IEnumerable<JobRoleVM>>();
            return View(jobRoles);
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
        return View(new List<JobRoleVM>());
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(JobRoleVM jobRole)
    {
        if (ModelState.IsValid)
        {
            var response = await _httpClient.PostAsJsonAsync<JobRoleVM>("JobRoles", jobRole);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        return View(jobRole);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"JobRoles/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jobRole = await response.Content.ReadFromJsonAsync<JobRoleVM>();
            return View(jobRole);
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, JobRoleVM jobRole)
    {
        if (id != jobRole.Id)
        {
            return BadRequest();
        }
        if (ModelState.IsValid)
        {
            var response = await _httpClient.PutAsJsonAsync<JobRoleVM>($"JobRoles/{id}", jobRole);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        return View(jobRole);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"JobRoles/{id}");
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
