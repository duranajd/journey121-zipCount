# journey121-zipCount

### Created By: 
* Andrew Duran, duranajd@gmail.com
* Last Updated: 03/13/2024@14:41CDT

# ZipCount

ZipCount is a C# console application that downloads CSV files containing customer data, extracts ZIP codes from each file, and counts the number of customers per ZIP code. It then generates a sorted list of ZIP codes with their respective customer counts.

## Features

- Downloads CSV files from URLs specified in a list file (provided via email).
  - Built with HttpClient package
- Extracts ZIP codes from customer data in each CSV file.
  - Built with Stream/FileStream/StreamReader I/O
- Counts the number of customers per ZIP code.
- Generates a list of ZIP codes, sorted by their respective customer counts.
- Outputs the result to a text file 'output.txt', generated on completion.

## Getting Started

To use ZipCount, follow these steps:

1. Clone this repository to your local machine.
2. Open the project in Visual Studio or your preferred C# IDE.
3. Build the project to compile the code.
4. Run the executable to start the application.
5. After running the project, you can clean the workspace with ./delete_files.sh

## Dependencies

- .NET Core SDK
- HttpClient package


