using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using ArcTexel.Helpers.Converters.JsonConverters;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Models.Services.NewsFeed;

[JsonConverter(typeof(DefaultUnknownEnumConverter<NewsType>))]
internal enum NewsType
{
    [Description(ArcPerfectIcons.Download)]
    NewVersion,
    [Description(ArcPerfectIcons.Youtube)]
    YtVideo,
    [Description(ArcPerfectIcons.Write)]
    BlogPost,
    [Description(ArcPerfectIcons.Message)]
    OfficialAnnouncement,
    [Description(ArcPerfectIcons.Info)]
    Misc = -1
}

internal record News
{
    public string Title { get; init; } = string.Empty;
    public NewsType NewsType { get; init; } = NewsType.Misc;
    public string Url { get; init; }
    public DateTime Date { get; init; }
    public string CoverImageUrl { get; init; } = string.Empty;

    [JsonIgnore]
    public string ResolvedIcon => NewsType.GetDescription();

    [JsonIgnore]
    public bool IsNew { get; set; } = false;

    public int GetIdentifyingNumber()
    {
        MD5 md5Hasher = MD5.Create();
        string data = Title + Url + Date.ToString(CultureInfo.InvariantCulture) + CoverImageUrl;
        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToInt32(hashed, 0);
    }
}
