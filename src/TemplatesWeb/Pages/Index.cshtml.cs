﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TemplatesShared;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace TemplatesWeb.Pages {
    public class IndexModel : PageModel {
        private TemplateWebConfig _config { get; set; }
        public IList<Template> _templates { get; set; }
        private string _baseUrl { get; set; }

        [BindProperty]
        public string SearchText { get; set; }

        public IndexModel(IOptions<TemplateWebConfig> config) {
            _config = config.Value;
            _templates = new List<Template>();

            _baseUrl = _config.TemplatesApiBaseUrl;
            if (string.IsNullOrWhiteSpace(_baseUrl) || _baseUrl.StartsWith(@"D:\")) {
                _baseUrl = @"http://dotnetnew-api.azurewebsites.net/api/";
            }
        }
        public void OnGet() {
            var url = new Uri(new Uri(_baseUrl), "search").AbsoluteUri;
            ViewData["url"] = _baseUrl;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<Template>>(json);
            _templates = result;
        }

        public IActionResult OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            return Redirect($"/Search/{SearchText}");
        }
    }
}
