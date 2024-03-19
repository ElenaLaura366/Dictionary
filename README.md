# Explanatory Dictionary WPF Application

This is the documentation for the Explanatory Dictionary application, developed in C# using the Windows Presentation Foundation (WPF) on the .NET Framework platform. The application comprises three main modules: Administrative, Word Search, and Entertainment.

## Installation

Follow these steps to run the application:

1. Ensure you have the [.NET Framework](https://dotnet.microsoft.com/en-us/download/dotnet-framework) installed on your machine.
2. Download the source code of the application from GitHub.
3. Open the solution file (.sln) in Visual Studio.
4. Build the solution by selecting **Build > Build Solution** from the menu.
5. Start the application by pressing **F5** or selecting **Debug > Start Debugging**.

## Usage

### Administrative Module

Access: Requires authentication with a predefined username and password.

Functions:

**Add Words**: Insert new words into the dictionary complete with descriptions, categories, and optional images.  
**Edit Words**: Update existing word entries, modifying their descriptions, categories, or images.  
**Delete Words**: Remove word entries from the dictionary.

### Word Search Module

This module enables users to search for words based on categories or freely. Features include:

**Category Selection**: Users can filter words by selecting a specific category.  
**Autocomplete Search**: As users type in the search box, suggestions will appear, enhancing the user experience.  
**Word Display**: Upon selection, the application displays the word, its description, category, and an associated image (if available).

### Entertainment Module

This interactive game challenges users to guess words based on given descriptions or images. Features include:

**Random Word Selection**: Each game round selects five random words from the dictionary.  
**Clues**: Users receive either a description or an image as a clue for the word.  
**Feedback**: After each guess, users are informed if their answer was correct or incorrect, with the correct answer provided for incorrect guesses.

## Authentication and Registration

### Registration

Before using the Explanatory Dictionary application, new users need to register. Here’s how:

1. Navigate to the registration page from the application's welcome screen.  
2. Fill in the required fields with your details.  
3. Submit your registration details by clicking the ‘Register’ button.

After registering, you can now log in using your credentials.

### Authentication

To access the full features of the Explanatory Dictionary, you must log in. Follow these steps:

1. Open the application and navigate to the login screen.  
2. Enter your registered username and password.  
3. Click the ‘Login’ button to authenticate.
