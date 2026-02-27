using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using ArcTexel.Models.Services.NewsFeed;
using ArcTexel.OperatingSystem;

namespace ArcTexel.Views.NewsFeed;

internal partial class NewsItem : UserControl
{
    public static readonly StyledProperty<News> NewsProperty =
        AvaloniaProperty.Register<NewsItem, News>(
        nameof(News));

    public News News
    {
        get { return (News)GetValue(NewsProperty); }
        set { SetValue(NewsProperty, value); }
    }
    
    public NewsItem()
    {
        InitializeComponent();
    }

    private void CoverImageClicked(object sender, PointerPressedEventArgs e)
    {
        IOperatingSystem.Current.OpenUri(News.Url);
    }
}

