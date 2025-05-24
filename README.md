# BlazorRAG

BlazorRAG is a .NET Blazor Server App that enables users to:
- Upload CSV files containing textual content (FAQs, docs, notes, etc.)
- Use OpenAI embeddings (text-embedding-3-small) to convert content into vector embeddings
- Store embeddings in a FAISS vector database via a Python API (HTTP)
- Query the knowledge base with natural language and get answers from OpenAI GPT-4

## Features
- Razor components and Bootstrap UI
- Backend-only embeddings (no client-side processing)
- HttpClient for all API calls
- Dependency Injection setup for services
- Bonus: query caching, file/embedding management, token counting/trimming

## Getting Started
1. Run the BlazorRAG app:
   ```powershell
   dotnet run
   ```
2. Ensure the Python FAISS API is running and accessible.
3. Configure your OpenAI API key and FAISS API endpoint in `appsettings.json`.

## Project Structure
- `Pages/` - Main Razor pages (upload, query, results)
- `Services/` - API and utility services
- `Components/` - UI components
- `.github/copilot-instructions.md` - Copilot custom instructions

## Requirements
- .NET 9 SDK (latest recommended)
- Python 3.8+ (for FAISS API)
- OpenAI API key

---
This project is a starting point. Extend as needed for your use case.
