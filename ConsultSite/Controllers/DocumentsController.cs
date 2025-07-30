using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Application.DTOs.Documents;
using Microsoft.AspNetCore.Authorization;

namespace RealEstate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IWebHostEnvironment _env;

        public DocumentsController(IDocumentService documentService, IWebHostEnvironment env)
        {
            _documentService = documentService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Console.WriteLine("[DocumentsController][GetAll] Старт запроса");

                var docs = await _documentService.GetAllDocumentsAsync();

                Console.WriteLine($"[DocumentsController][GetAll] Получено документов: {docs?.Count() ?? 0}");

                return Ok(docs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DocumentsController][GetAll] Ошибка: {ex.Message}");
                Console.WriteLine($"[DocumentsController][GetAll] StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Ошибка при получении списка документов.");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var doc = await _documentService.GetDocumentByIdAsync(id);
                if (doc == null)
                    return NotFound($"Документ с ID {id} не найден.");

                return Ok(doc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DocumentsController][GetById]", DateTime.Now, ex.Message);
                return StatusCode(500, "Ошибка при получении документа.");
            }
        }

        [HttpGet("topic/{topic}")]
        public async Task<IActionResult> GetByTopic([FromRoute] string topic)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _documentService.GetDocumentsByTopicAsync(topic);
                if (result == null)
                    return NotFound($"Документы с темой '{topic}' не найдены");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DocumentsController][GetByTopic]", DateTime.Now, ex.Message);
                return StatusCode(500, "Ошибка при получении документов по теме");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _documentService.SearchDocumentsAsync(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DocumentsController][Search]", DateTime.Now, ex.Message);
                return StatusCode(500, "Ошибка при поиске документов.");
            }
        }

        [HttpGet("{id}/file")]
        public async Task<IActionResult> DownloadTemplate(int id)
        {
            var result = await _documentService.GetTemplateFileAsync(id);
            if (result == null)
                return NotFound("Файл шаблона не найден или документ не является шаблоном.");

            var (file, fullPath) = result.Value;

            var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            var filename = Path.GetFileName(fullPath);

            return File(bytes, file.MimeType, filename);
        }

    }
}
