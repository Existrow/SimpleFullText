using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SFullText.Engine.Utils;
using SFullText.Interfaces;

var ruchars = new char[] { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'э', 'ю', 'я' };
Console.WriteLine("Try it!");

var DopStreeetsCount = 60000;

var streets = GetStreets().ToDictionary(sp => sp.Id);

var storage = Indexer
    .ConfigureIndex()
    .CreateNgrammIndex(streets.Values);

storage.TrimStorage();

var searchConfiguration = Searcher
    .ConfigureSearchQuery();

var req = string.Empty;
while(!string.IsNullOrEmpty(req = GetUserRequest()))
{
    Search(() => searchConfiguration.SetSearchQuery(req).Search(storage).ToList());
    Search(streets.Values.Where(street => street.Name.Contains(req, StringComparison.OrdinalIgnoreCase)).ToList);

    Console.WriteLine(string.Empty);
}

void Search(Func<List<SearchModel>> func)
{
    var stopwatch = Stopwatch.StartNew();
    var result = func();
    Console.WriteLine($"----------Perfomance ms: {stopwatch.ElapsedMilliseconds}----------");
    foreach (var street in result)
    {
        Console.WriteLine(street.Name);
    }
}

IEnumerable<SearchModel> GetStreets()
{
    var regex = new Regex("<a class=.link_to_street.+>(.+)</a>", RegexOptions.Compiled | RegexOptions.Multiline);

    var html = new WebClient().DownloadString("https://bestmaps.ru/city/sankt-peterburg/street");
    uint id = 1;

    foreach(var street in regex.Matches(html).Cast<Match>())
    {
        for(int i = 0; i < DopStreeetsCount; i++)
        {
            var name = GetRandomName();

            yield return new()
            {
                Id = id++,
                Name = name,
                SearchTerms = new[] { name }
            };
        }


        yield return new()
        {
            Id = id++,
            Name = street.Groups[1].Value,
            SearchTerms = street.Groups[1].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
        };
    }
}

string? GetUserRequest()
{
    Console.Write("Запрос: ");

    return Console.ReadLine();
}

string GetRandomName()
{
    var strBulder = new StringBuilder();

    foreach (var letrerPositib in Enumerable.Range(5, Random.Shared.Next(20)))
        strBulder.Append(ruchars[Random.Shared.Next(ruchars.Length)]);

    return strBulder.ToString();
}

class SearchModel : ISearchModel
{
    public uint Id { get; init; }

    public string Name { get; init; }

    public IEnumerable<string> SearchTerms { get; init; }

    public IEnumerable<string> GetSearchTerms(string groupKey)
    {
        yield return Name;
        foreach(var name in SearchTerms) yield return name;
    }
}
