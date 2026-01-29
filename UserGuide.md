# Getting Started

## Download Release Version (Windows)

1. Go to the **Releases** page:  
   https://github.com/TrusTheDev/Trusty-Manager/releases/tag/Windows
2. Download **Trusty-Manager.rar**
3. Extract the file
4. Run **setup.exe**
5. Once installed, you can:
   - Create a new **.docx** or **.xlsx** file  
   - Or import an existing one

That’s it. Trusty Manager is ready to use.

---

# Features Overview

## File Management

### **Abrir**
Opens the default directory:


This allows quick access to all managed files directly from the Windows File Explorer.

---

### **Guardar**
Saves the current state of the opened file, regardless of whether changes were made or not.

Useful to:
- Persist edits
- Ensure data consistency

---

### **Borrar**
Deletes the currently opened file.

> Note: Files are sent to the **Recycle Bin**, not permanently removed.

---

### **Agregar**
Creates a new file and adds it to the current workspace.

You can create:
- Word documents (`.docx`)
- Excel documents (`.xlsx`)

---

## Navigation Between Files

### **Anterior**
Moves focus to the **previous file** in the file list.

---

### **Siguiente**
Moves focus to the **next file** in the file list.

This allows fast navigation without reopening the file explorer.

---

## Table Handling (DataGridView)

Trusty Manager uses **WinForms DataGridView** for spreadsheet-like data manipulation.

---

### **Columna +**
Adds a new column at the **end of the table**.

- The column is automatically initialized
- Header text is editable

---

### **Columna -**
Deletes the **currently selected column**.

> If no column is selected, the action will not be executed.

---

### **Fila +**
Adds a new row:
- At the **bottom** of the table
- Or **below the currently selected row**

This behavior depends on selection state.

---

### **Fila -**
Deletes the **selected row** from the table.

---

## Data Integrity

The application automatically detects when a file has been modified.

Before closing or switching files, the user is prompted to choose whether to:
- Save the changes
- Discard the changes

This ensures that no modifications are lost unintentionally.


## Sorting (SortBy)

You can sort table data directly using column headers.

### How it works:
- Click on a column header to sort rows **ascending**
- Click again to sort **descending**

### Supported sorting:
- Numeric values
- Text values
- Mixed content (handled automatically by DataGridView)

This uses the built-in `DataGridView` sorting mechanism.

---

## Column Resizing (Expand)

Columns can be resized manually to improve readability.

### Options:
- Drag column borders to resize manually
- Expand columns to fit content
- Adjust width based on user preference

This allows better visualization for long text or large values.

---

## Notes on DataGridView Behavior

- Supports in-place editing
- Automatically updates bound data
- Handles selection, sorting, and resizing natively
- Optimized for Excel-like usage inside WinForms
