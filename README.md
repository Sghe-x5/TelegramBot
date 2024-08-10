## Project: Telegram Bot for CSV and JSON Processing

### Overview

This project aims to extend previous knowledge and experience by applying newly learned concepts, such as serialization formats and LINQ, to a prior assignment. The task involves developing a Telegram bot that processes CSV files, applying various operations such as reading, writing, filtering, and sorting the data. The bot will also support JSON file processing. The specific CSV file to be processed and the operations required are detailed in the individual variant.

### Project Structure

1. **Class Library Requirements**
   - **Class `MyType`**: Represents objects based on the data in the CSV file. The class should have a constructor to initialize its fields, following Microsoft's naming conventions.
   - **Class `CSVProcessing`**: 
     - `Write`: Takes a collection of `MyType` objects and returns a `Stream` object that the bot can use to send the CSV file.
     - `Read`: Takes a `Stream` object from the bot containing a CSV file and returns a collection of `MyType` objects.
   - **Class `JSONProcessing`**:
     - `Write`: Takes a collection of `MyType` objects and returns a `Stream` object that the bot can use to send the JSON file.
     - `Read`: Takes a `Stream` object from the bot containing a JSON file and returns a collection of `MyType` objects.

2. **OOP and Architectural Principles**
   - Classes should adhere to principles like Open/Closed, Single Responsibility, Liskov Substitution, and Dependency Inversion.
   - Ensure encapsulation and proper relationships between types.
   - Classes should be accessible outside of their assembly.
   - Non-static classes must have parameterless constructors or equivalent.
   - Do not alter the dataset for `MyType` beyond what's specified in the CSV file.
   - It's acceptable to extend open behavior or add private members as needed.

3. **Telegram Bot Interface Requirements**
   - The bot should be developed using the `TelegramBots` NuGet package. Key functionalities include:
     - **Uploading a CSV file** for processing.
     - **Filtering data** based on a specified field.
     - **Sorting data** based on a specified field.
     - **Downloading the processed file** in either CSV or JSON format.
     - **Uploading a JSON file** for processing.

4. **General Requirements**
   - Follow the discipline's coding standards, including meaningful comments.
   - All code must be written in C# with .NET 6.0.
   - Ensure that text data, including Russian text, is correctly decoded and human-readable.
   - Handle resources properly, especially when working with files.
   - Maintain the CSV structure consistent with the example file.
   - Validate user input and handle exceptional situations robustly.

### Individual Variant Specifications

- **CSV File**: `electrocar-power.csv`
- **Fields for Filtering**:
  - `AdmArea`
  - `District`
  - `AdmArea` combined with `Longitude_WGS84` and `Latitude_WGS84`
- **Fields for Sorting**:
  - `AdmArea` in ascending alphabetical order
  - `AdmArea` in descending alphabetical order

### Instructions for Use

1. **Setting Up the Bot**: 
   - Install the required `TelegramBots` NuGet package.
   - Follow the quickstart guide to set up and configure your bot.

2. **Processing CSV/JSON Files**:
   - Use the bot commands to upload, filter, sort, and download files.
   - Ensure that the bot's responses and file handling meet the outlined requirements.

3. **Code Structure**:
   - Follow the object-oriented principles and ensure that your code is well-documented.
   - Test the bot thoroughly to ensure it handles all edge cases, particularly in file processing and user input.

### Conclusion

This project challenges you to integrate various programming concepts into a practical application. By adhering to the outlined requirements and principles, you'll develop a robust Telegram bot capable of handling complex file processing tasks.
