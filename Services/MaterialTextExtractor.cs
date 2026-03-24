using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig;

namespace MandarinQuest.Services
{
    public class MaterialTextExtractor
    {
        public string ExtractCombinedText(IEnumerable<string> filePaths)
        {
            StringBuilder sb = new StringBuilder();

            if (filePaths == null)
                return string.Empty;

            foreach (string filePath in filePaths)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    continue;

                string absolutePath = ResolveAbsolutePath(filePath);

                if (!File.Exists(absolutePath))
                {
                    sb.AppendLine("----- FILE NOT FOUND -----");
                    sb.AppendLine(filePath);
                    sb.AppendLine();
                    continue;
                }

                string extension = Path.GetExtension(absolutePath).ToLowerInvariant();
                string extractedText = string.Empty;

                try
                {
                    switch (extension)
                    {
                        case ".txt":
                            extractedText = ExtractFromTxt(absolutePath);
                            break;

                        case ".pdf":
                            extractedText = ExtractFromPdf(absolutePath);
                            break;

                        default:
                            extractedText = $"[Unsupported file type: {extension}]";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    extractedText = $"[Failed to extract text: {ex.Message}]";
                }

                sb.AppendLine("----- MATERIAL START -----");
                sb.AppendLine("File: " + Path.GetFileName(absolutePath));
                sb.AppendLine(extractedText);
                sb.AppendLine("----- MATERIAL END -----");
                sb.AppendLine();
            }

            return sb.ToString().Trim();
        }

        private string ResolveAbsolutePath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
                return filePath;

            string cleanPath = filePath.Replace("/", "\\").TrimStart('\\');
            return System.Web.Hosting.HostingEnvironment.MapPath("~/" + cleanPath);
        }

        private string ExtractFromTxt(string filePath)
        {
            return File.ReadAllText(filePath, Encoding.UTF8);
        }

        private string ExtractFromPdf(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }

            return sb.ToString();
        }
    }
}