using System;
class ZipCount
{
    static async Task DownloadFileViaHttpClient(string url, string filePath)    
    {
        Console.WriteLine($"Attempting download - {url} - to {filePath}");
        if (!File.Exists(filePath)) // prevent overwriting by checking filePath
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // GET request to file (download)
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode) // successful download
                    {
                        // read content stream - save to file
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (FileStream fileStream = File.Create(filePath))
                            {
                                await contentStream.CopyToAsync(fileStream); // must be async
                            }
                        }
                        Console.WriteLine($"File download success - {filePath}");
                    }
                    else  // unsuccessful download
                    {
                        Console.WriteLine($"Error downloading file: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex) // error download - general
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }
        else // file exists already
        {
            Console.WriteLine($"File path {filePath} already exists!");
        }
    }

    static string ExtractZipCode(string line) 
    {
        string[] fields = line.Split(',');

        // CustomerID,FirstName,Lastname,Phone,Address01,Address02,City,State,ZipCode,ZipPlus4
        // ZipCode = line[8]

        if (fields.Length >= 9) // ensure line has proper/enough fields
        {
            return fields[8].Trim();
        } 
        else
        {
            return "-1"; // function check: record -1 ZIP code on failure
        }
    }


    static void ScanFile(string filePath, Dictionary<string, int> dict)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines.Skip(1)) // skip header row on open
            {
                string zipCode = ExtractZipCode(line);
                if (dict.ContainsKey(zipCode)) // if exists, increment dictionary index
                {
                    dict[zipCode]++;
                }
                else // add to dictionary if new
                {
                    dict[zipCode] = 1;
                }
               
            }

        }
        catch (Exception ex) 
        {
            Console.WriteLine($"StreamReader Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Finished reading file {filePath}");
        }
    }

    static async Task Main(string[] args)
    {
<<<<<<< HEAD
=======

>>>>>>> 82f2cd41aea4ec933281bda987aa7db0e97109e9
        string filePath = "downloads/list.csv";
        int fileIndex = 0; // keep count of # downloads

        // download the initial file - list.csv
        string listUrl = "https://journeyblobstorage.blob.core.windows.net/sabpublic/list";
        await DownloadFileViaHttpClient(listUrl, filePath);

        // Read and process the downloaded file
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null) // download 1 file per line
                {
<<<<<<< HEAD
                    // match given naming convention
                    await DownloadFileViaHttpClient(line, $"downloads/populationFiles/File_{fileIndex}.csv"); 
=======
                    // Console.WriteLine(line);
                    await DownloadFileViaHttpClient(line, $"downloads/populationFiles/File_{fileIndex}.csv");
>>>>>>> 82f2cd41aea4ec933281bda987aa7db0e97109e9
                    fileIndex++;
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"StreamReader Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Finished downloading {fileIndex} files");
        }

        // open each file, read lines & record
        Dictionary<string, int> zipCodeCounts = new Dictionary<string, int>();
        for (int i = 0; i < fileIndex; i++) 
        {
            ScanFile($"downloads/populationFiles/File_{i}.csv", zipCodeCounts);
        }

        var sortedDict = zipCodeCounts.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);

        try
        {
            // Create or overwrite the file with the sorted dictionary content
            filePath = "output.txt";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var kvp in sortedDict)
                {
                    writer.WriteLine($"Zip Code: {kvp.Key}, Customer Count: {kvp.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }
    }

}
