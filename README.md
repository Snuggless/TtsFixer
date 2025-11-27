# TTS Fixer

A Blazor WebAssembly application for normalizing and cleaning text before Text-To-Speech (TTS) output. Create custom transformation rules to fix pronunciation, handle abbreviations, and process text patterns.

## Features

- **Custom Text Transformation Rules**: Create, edit, and manage rules to transform text patterns
- **Pattern Matching**: Support for both literal text and case-sensitive/case-insensitive matching
- **Rule Priority**: Set priorities to control the order in which rules are applied
- **Built-in Normalization**: Integrates with text normalization rules including:
  - Emoji handling
  - Currency formatting
  - Abbreviation expansion
  - Number normalization
  - URL processing
  - Whitespace cleanup
  - Excessive punctuation removal
  - Letter repetition handling
- **File Processing**: Upload text files and download normalized output
- **Persistent Storage**: Rules are stored locally in IndexedDB

## Tech Stack

- **.NET 9.0** - Target framework
- **Blazor WebAssembly** - Client-side web framework
- **MudBlazor** - Material Design component library for Blazor
- **Agash.TTSTextNormalization** - Text normalization library
- **IndexedDB** - Client-side storage via JavaScript interop

## Project Structure

```
TtsFixer/
├── src/
│   ├── TtsFixer.Core/           # Core library
│   │   ├── Abstractions/        # Interfaces (IRuleRepository)
│   │   ├── Services/            # Service implementations
│   │   ├── Extensions/          # DI extensions
│   │   └── CustomRule.cs        # Rule entity model
│   │
│   └── TtsFixer.WebApp/         # Blazor WebAssembly app
│       ├── Components/          # Razor components
│       │   ├── Pages/           # Page components
│       │   ├── Dialogs/         # Dialog components
│       │   └── Layout/          # Layout components
│       ├── Services/            # App services
│       └── wwwroot/             # Static assets
│
├── TtsFixer.sln                 # Solution file
└── Directory.Build.props        # Shared build configuration
```

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later

### Build and Run

```bash
# Clone the repository
git clone https://github.com/Snuggless/TtsFixer.git
cd TtsFixer

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the web application
dotnet run --project src/TtsFixer.WebApp
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Usage

1. **Add Rules**: Click "Add Rule" to create a new text transformation rule
2. **Configure Rule**:
   - **Name**: A descriptive name for the rule
   - **Description**: Explanation of what the rule does
   - **Priority**: Order of execution (lower values run first)
   - **Pattern**: The text pattern to match
   - **Replacement**: The text to replace matches with
   - **Case Sensitive**: Enable for case-sensitive matching
3. **Process Text**: Upload a `.txt` file and click "Run" to apply all enabled rules
4. **Download**: The normalized text file will be automatically downloaded

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE.txt](LICENSE.txt) file for details.

## Author

**Snuggless**