# journey121-zipCount

### Created By: 
* Andrew Duran, duranajd@gmail.com
* Last Updated: 03/12/2024@23:41CDT

# ZipCount

ZipCount is a C# console application that downloads CSV files containing customer data, extracts ZIP codes from each file, and counts the number of customers per ZIP code. It then generates a sorted list of ZIP codes with their respective customer counts.

## Features

- Downloads CSV files from URLs specified in a list file.
- Extracts ZIP codes from customer data in CSV files.
- Counts the number of customers per ZIP code.
- Generates a sorted list of ZIP codes with their respective customer counts.
- Outputs the result to a text file.

## Getting Started

To use ZipCount, follow these steps:

1. Clone this repository to your local machine.
2. Open the project in Visual Studio or your preferred C# IDE.
3. Build the project to compile the code.
4. Run the executable to start the application.

## Usage

Upon running the application, ZipCount will:

1. Download the list of CSV files from the specified URL (provided via email).
2. Download each CSV file containing customer data.
3. Process each file, extract ZIP codes, and count the number of customers per ZIP code.
4. Generate a sorted list of ZIP codes with their respective customer counts.
5. Write the result to an output text file named "output.txt".

## Dependencies

- .NET Core SDK
- HttpClient package


