# journey121-zipCount

### Created By: 
* Andrew Duran, duranajd@gmail.com
* Last Updated: 03/13/2024@21:33CDT

# ZipCount

ZipCount is a C# console application that downloads CSV files containing customer data, extracts ZIP codes from each file, and counts the number of customers per ZIP code. It then generates a sorted list of ZIP codes with their respective customer counts.

## Installation

To use ZipCount, follow these steps:

1. Clone this repository to your local machine.
2. Open the project in Visual Studio or your preferred C# IDE.
4. Build the project to compile the code solution.
5. Before running the project, clean downloaded files by running the following commands:
   * cd ZipCount/
   * ./delete_files.sh
6. Run the program to start the demonstration, and the results will be stored in ZipCount/output.txt.

## Workflow

- Downloads CSV files from URLs specified in a list file (link provided via email).
  - Built with HttpClient package
- Extracts ZIP codes from customer data in each CSV file.
  - Built with Stream/FileStream I/O
- Counts the number of customers per ZIP code.
- Generates a list of ZIP codes, sorted by their respective customer counts.
- Outputs the result to a text file 'output.txt', generated on completion.

## Dependencies

- .NET Core SDK
- HttpClient package


