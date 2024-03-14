using GetAndSavePrice;
using GetAndSavePrice.CSVParser;
using GetAndSavePrice.DBAdapter;
using GetAndSavePrice.ImapAdapter;
using System;
using System.Configuration;
using System.Text;
using System.Xml;


List<CompanyConfiguration> LoadCompaniesFromXml(string filePath)
{
    List<CompanyConfiguration> companies = new List<CompanyConfiguration>();

    XmlDocument doc = new XmlDocument();
    doc.Load(filePath);

    XmlNodeList nodes = doc.SelectNodes("/Companies/CompanyConfig");

    foreach (XmlNode node in nodes)
    {
        CompanyConfiguration config = new CompanyConfiguration();
        config.CompanyName = node.SelectSingleNode("CompanyName").InnerText;
        config.CompanyEmail = node.SelectSingleNode("CompanyEmail").InnerText;
        config.IndexVendor = Convert.ToByte(node.SelectSingleNode("IndexVendor").InnerText);
        config.IndexNumber = Convert.ToByte(node.SelectSingleNode("IndexNumber").InnerText);
        config.IndexDescription = Convert.ToByte(node.SelectSingleNode("IndexDescription").InnerText);
        config.IndexPrice = Convert.ToByte(node.SelectSingleNode("IndexPrice").InnerText);
        config.IndexCount = Convert.ToByte(node.SelectSingleNode("IndexCount").InnerText);

        companies.Add(config);
    }
    return companies;
}

List<CompanyConfiguration> companies = LoadCompaniesFromXml("companiesConfig.xml");
int selected = 0;

Console.WriteLine("Выберите нужную компанию.");

for (int i = 0; i < companies.Count(); i++)
{
    Console.Write("["+i+"] ");
    Console.WriteLine($"CompanyName: {companies[i].CompanyName}");
    Console.WriteLine($"    CompanyEmail: {companies[i].CompanyEmail}");
}

if (selected < 0 || selected > companies.Count() - 1)
{
    Console.WriteLine("Введен неверный индекс");
    return;
}


Console.Write("Введите нужный индекс: ");
selected = Convert.ToInt32(Console.ReadLine());

CompanyConfiguration companyConfiguration = companies[selected];

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

AENetMailService aeNetMailService = new AENetMailService();

string hostname     = ConfigurationManager.AppSettings["hostname"] ?? "default_hostname";
string login        = ConfigurationManager.AppSettings["login"] ?? "default_login";
string password     = ConfigurationManager.AppSettings["password"] ?? "default_password";

int port            = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
bool useSsl         = Convert.ToBoolean(ConfigurationManager.AppSettings["useSsl"]);



ImapAdapter imapAdapter = new ImapAdapter(aeNetMailService, hostname, port, useSsl, login, password);

try
{
    imapAdapter.SaveAttachment(companyConfiguration.CompanyEmail);
} catch
{
    Console.WriteLine("Вложение не сохранено");
    return;
}


CSVHelper csvHelper = new CSVHelper();

List<PriceItem> priceItems = csvHelper.ReadFromFile("downloadedData.csv", companyConfiguration);

MySQLAdapter sqlAdapter = new MySQLAdapter(ConfigurationManager.ConnectionStrings["mysql"].ConnectionString);
sqlAdapter.SaveDataToDB(priceItems);