using System;
class ZipCount
{
    // GET request to download file at URL, stores locally from /ZipCount/filePath
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

    // split line into column fields and return Zip Code
    static string ExtractZipCode(string line) 
    {
        string[] fields = line.Split(',');

        // file headers
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

    // scan file using ExtractZipCode and add to/update dictionary
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
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Finished reading file {filePath}");
        }
    }

    static async Task Main(string[] args)
    {
        string filePath = "downloads/list.csv";
        int fileIndex = 0; // keep count of # downloads

        // download the initial file - list.csv
        // bad style to hard-code URLs, but works for this challenge
        string listUrl = "https://journeyblobstorage.blob.core.windows.net/sabpublic/list";
        await DownloadFileViaHttpClient(listUrl, filePath);

        // read & process the downloaded file
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null) // download 1 file per line
                {
                    // match given naming convention
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
        // dictionary - key value pairs (k=zipCode, v=customerCount)
        Dictionary<string, int> zipCodeCounts = new Dictionary<string, int>();
        for (int i = 0; i < fileIndex; i++) 
        {
            ScanFile($"downloads/populationFiles/File_{i}.csv", zipCodeCounts);
        }

        // list output from greatest customer population (value) to least
        var sortedDict = zipCodeCounts.OrderByDescending(kv => kv.Value);

        try
        {
            // create output file with sorted dictionary content
            filePath = "output.txt";
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var kvp in sortedDict)
                {
                    sw.WriteLine($"Zip Code: {kvp.Key}, Customer Count: {kvp.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }

         Console.WriteLine($"Results written to file: {filePath}");
    }

}
