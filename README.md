# Trusty Manager

## 📖 Description
Trusty Manager is an open-source template project developed in C# using WinForms, designed to help developers build custom solutions for automating table-based workflows similar to Excel through a DataGridView.

The main goal of this project is to simplify repetitive data entry tasks by allowing developers to define automated behaviors between columns, such as creating custom column relationships and triggering specific actions when a value is entered into a given column.
More details can be found in the [Features](##✨Features) section.

Trusty-Manager serves as a flexible foundation for creating automation tools, administrative systems, and data-processing applications without relying directly on Microsoft Excel or Word.

## Example of a Derived Project – Trusty Patient Manager
![Patient-Manager](https://github.com/user-attachments/assets/a3c28e20-0de7-4bb9-b77a-0477b1cbfa38)
## ✨ Generic Features

### ✔️ Basic Table Operations

* Create new rows.

* Create new columns.

* Delete existing rows or columns.

* Direct manipulation of tabular data through the UI.

### ✔️ File Management

* Create a new file from scratch.

* Navigate between multiple opened files.

* Delete existing files.

* Open the file location directly from the application.

### ✔️ Undo System (Stack-Based)

* Implements an undo functionality using a stack structure.

* Tracks user actions to allow step-by-step rollback.

* Improves safety when experimenting with data changes.

### ✔️ DOCX and XLSX Support

* Supports .docx and .xlsx formats.

* Does not require Microsoft Office to be installed.

* Allows exporting and working with spreadsheet-like documents.
  
## Trusty Manager 
![Trusty-Manager](https://github.com/user-attachments/assets/ade797fb-5f9b-4d7b-87c4-532097bc5b22)

### Dependencies

**Framework**
- WinForms

**Package Manager**
- NuGet

**NuGet Packages**
- ClosedXML — Used for creating and manipulating Excel files.
- DocX — Used for generating and editing Word documents.


## 🛠️ Installation

Before installing, please read the [Disclaimer](#disclaimer) section below.

1. [Download](https://github.com/TrusTheDev/Trusty-Manager/releases/tag/Windows) the latest version (Windows only).
2. Extract the downloaded archive to a directory of your choice.
3. Run `setup.exe` and follow the on-screen instructions.

Installation completed.

## ⚠️ Disclaimer

Trusty Manager is still under development. The current version may contain bugs or unexpected behavior.

Use this software at your own risk. The author is not responsible for any loss of data or information. It is strongly recommended to create backups of your files before and after each use of the program.

## 🚩 Useful Information

- Windows Defender or other security software may block the application. If this happens, add the program to your exclusions list.
- If you accidentally delete a file, you can recover it from the Recycle Bin.
- You can uninstall with ease using add or remove on windows settings, make sure to delete C:\Users\User\Documents\Patient Manager
