using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GetAndSavePrice.CSVParser
{
    public class CompanyConfiguration
    {
        public byte IndexVendor { get; set; }
        public byte IndexNumber { get; set; }
        public byte IndexDescription { get; set; }
        public byte IndexPrice { get; set; }
        public byte IndexCount { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }

        /// <summary>
        /// у каждого поставщика своя конфигурация, каждому полю даем свой индекс (индексы начинаются с нуля)
        /// </summary>
        /// <param name="companyName">название компании</param>
        /// <param name="indexVendor">позиция поля "Бренд"</param>
        /// <param name="indexNumber">позиция поля "Каталожный номер"</param>
        /// <param name="indexDescription">позиция поля "Описание"</param>
        /// <param name="indexPrice">позиция поля "Цена"</param>
        /// <param name="indexCount">позиция поля "Наличие"</param>
        public CompanyConfiguration(string companyName, string companyEmail, byte indexVendor, byte indexNumber, byte indexDescription, byte indexPrice, byte indexCount)
        {
            CompanyName = companyName;
            CompanyEmail = companyEmail;
            IndexVendor = indexVendor;
            IndexNumber = indexNumber;
            IndexDescription = indexDescription;
            IndexPrice = indexPrice;
            IndexCount = indexCount;
        }

        public CompanyConfiguration() { }

        public void LoadFromXml(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode node = doc.SelectSingleNode("/CompanyConfig");

            IndexVendor = Convert.ToByte(node.SelectSingleNode("IndexVendor").InnerText);
            IndexNumber = Convert.ToByte(node.SelectSingleNode("IndexNumber").InnerText);
            IndexDescription = Convert.ToByte(node.SelectSingleNode("IndexDescription").InnerText);
            IndexPrice = Convert.ToByte(node.SelectSingleNode("IndexPrice").InnerText);
            IndexCount = Convert.ToByte(node.SelectSingleNode("IndexCount").InnerText);
            CompanyName = node.SelectSingleNode("CompanyName").InnerText;
        }
    }
}
