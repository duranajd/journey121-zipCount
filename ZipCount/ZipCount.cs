using System;
class ZipCount
{
    static async Task DownloadFileViaHttpClient(string url, string filePath)    
    {
        Console.WriteLine($"Attempting download - {url} - to {filePath}");
        if (!File.Exists(filePath)) // if exists, don't redownload
        {
            try // download
            {
                using (HttpClient client = new HttpClient())
                {
                    // GET request to file (download)
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // read content stream - save to file
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (FileStream fileStream = File.Create(filePath))
                            {
                                await contentStream.CopyToAsync(fileStream);
                            }
                        }
                        Console.WriteLine($"File download success - {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"Error downloading file: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex) // error download
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }
        else // file exists
        {
            Console.WriteLine($"File path {filePath} already exists!");
        }
    }

   
    static string ExtractZipCode(string line) 
    {
        string[] fields = line.Split(',');
        // CustomerID,FirstName,Lastname,Phone,Address01,Address02,City,State,ZipCode,ZipPlus4
        // ZipCode = line[8]
        if (fields.Length >= 9)
        {
            return fields[8].Trim();
        } 
        else
        {
            return "-1";
        }
    }


    static void ScanFile(string filePath, Dictionary<string, int> dict)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines.Skip(1))
            {
                string zipCode = ExtractZipCode(line);
                if (dict.ContainsKey(zipCode))
                {
                    dict[zipCode]++;
                }
                else
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
        Console.WriteLine("BEGIN RUN");

        string filePath = "downloads/list.csv";
        int fileIndex = 0;

        // download the initial file - list.csv
        await DownloadFileViaHttpClient("https://journeyblobstorage.blob.core.windows.net/sabpublic/list", filePath);

        // Read and process the downloaded file
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null) // download 1 file per line
                {
                    Console.WriteLine(line);
                    await DownloadFileViaHttpClient(line, $"downloads/populationFiles/File_{fileIndex}.csv");
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
