using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;
using ArcTexel.Helpers;
using ArcTexel.Platform;

namespace ArcTexel.Models.Services.NewsFeed;

internal class NewsProvider
{
    private const int MaxNewsCount = 20;
    private const string FeedUrl = "https://raw.githubusercontent.com/ArcTexel/news-feed/main/";

    private List<int> _lastCheckedIds;

    public NewsProvider()
    {
        _lastCheckedIds = ArcTexelSettings.StartupWindow.LastCheckedNewsIds.AsList();
    }

    public async Task<List<News>?> FetchNewsAsync()
    {
        List<News> allNews = new List<News>();
        await FetchFrom(allNews, "shared.json");
        await FetchFrom(allNews, $"{IPlatform.Current.Id}.json");

        var sorted = allNews.OrderByDescending(x => x.Date).Take(MaxNewsCount).ToList();
        MarkNewOnes(sorted);
        return sorted;
    }

    private async Task FetchFrom(List<News> output, string fileName)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "ArcTexel");
        HttpResponseMessage response = await client.GetAsync($"{FeedUrl}{fileName}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            string content = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<News>>(content, JsonOptions.CasesInsensitive);
            output.AddRange(list);
        }
    }

    private void MarkNewOnes(List<News> list)
    {
        foreach (var news in list)
        {
            if (news.GetIdentifyingNumber() is var num && !_lastCheckedIds.Contains(num))
            {
                news.IsNew = true;
                _lastCheckedIds.Add(num);
                if (_lastCheckedIds.Count > MaxNewsCount)
                {
                    _lastCheckedIds.RemoveAt(0);
                }
            }
        }

        ArcTexelSettings.StartupWindow.LastCheckedNewsIds.Value = _lastCheckedIds;
    }
}
