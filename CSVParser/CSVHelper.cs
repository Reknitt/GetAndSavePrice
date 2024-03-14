using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAndSavePrice.CSVParser
{
    internal class CSVHelper
    {
        public List<PriceItem> ReadFromFile(string pathToFile, CompanyConfiguration companyConfiguration)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", // Установка разделителя на точку с запятой
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                //Escape = ';',
                LineBreakInQuotedFieldIsBadData = false,
                BadDataFound = null,
                
            };



            using (var reader = new StreamReader(pathToFile))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    
                    csv.Read();
                    csv.ReadHeader();

                    List<PriceItem> priceItems = new List<PriceItem>();

                    while (csv.Read())
                    {
                        string vendor = csv.GetField<string>(companyConfiguration.IndexVendor).Trim();
                        string number = csv.GetField<string>(companyConfiguration.IndexNumber).Trim();

                        string description = csv.GetField<string>(companyConfiguration.IndexDescription).Trim();
                        description = description.Substring(0, Math.Min(description.Length, 512));

                        decimal price = 0;
                        int count = 0;
                        try
                        {
                            price = csv.GetField<decimal>(companyConfiguration.IndexPrice);
                            count = ParseCountField(csv.GetField<string>(companyConfiguration.IndexCount));
                        } 
                        catch (TypeConverterException) 
                        {
                            price = 0; 
                        }
                        catch (FormatException)
                        {
                            count = 0;
                        }

                        string searchVendor = new string(vendor.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
                        string searchNumber = new string(number.Where(char.IsLetterOrDigit).ToArray()).ToUpper();

                        PriceItem item = new PriceItem
                        {
                            Vendor = vendor,
                            Description = description,
                            Number = number,
                            SearchVendor = searchVendor,
                            SearchNumber = searchNumber,
                            Price = price,
                            Count = count
                        };
                        priceItems.Add(item);
                    }

                    return priceItems;
                }
            }

        }

        public int ParseCountField(string countField)
        {
            if (countField.StartsWith(">") || countField.StartsWith("<")) return int.Parse(countField.Substring(1)); // просто убираем лишний символ и парсим

            else if (countField.Contains("-")) return int.Parse(countField.Split('-')[1]); // парсим второе число после знака "-"

            else return int.Parse(countField); // если нет лишних символов, просто парсим
        }
    }
}
