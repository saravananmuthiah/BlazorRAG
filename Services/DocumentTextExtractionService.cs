using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using System.Text;

namespace BlazorRAG.Services
{
    public class DocumentTextExtractionService
    {
        public async Task<List<string>> ExtractTextChunksAsync(Stream fileStream, string fileName = null)
        {
            var result = new List<string>();
            if (fileName != null && fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                // PDF extraction
                using var pdf = PdfDocument.Open(fileStream);
                foreach (var page in pdf.GetPages())
                {
                    var text = page.Text;
                    if (!string.IsNullOrWhiteSpace(text))
                        result.Add(text);
                }
                return result;
            }
            else if (fileName != null && (fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".doc", StringComparison.OrdinalIgnoreCase)))
            {
                // Word extraction
                using var mem = new MemoryStream();
                await fileStream.CopyToAsync(mem);
                mem.Position = 0;
                using var wordDoc = WordprocessingDocument.Open(mem, false);
                var sb = new StringBuilder();
                foreach (var text in wordDoc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    sb.AppendLine(text.Text);
                }
                var docText = sb.ToString();
                if (!string.IsNullOrWhiteSpace(docText))
                    result.Add(docText);
                return result;
            }
            else
            {
                // CSV extraction (default)
                using var reader = new StreamReader(fileStream);
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        result.Add(line);
                    }
                }
                return result;
            }
        }
    }
}
