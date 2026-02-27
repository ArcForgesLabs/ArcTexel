using Microsoft.Extensions.DependencyInjection;
using ArcTexel.Extensions.Commands;
using ArcTexel.Extensions.CommonApi.Commands;
using ArcTexel.Extensions.CommonApi.IO;
using ArcTexel.Extensions.CommonApi.Logging;
using ArcTexel.Extensions.CommonApi.Palettes;
using ArcTexel.Extensions.CommonApi.Ui;
using ArcTexel.Extensions.CommonApi.User;
using ArcTexel.Extensions.CommonApi.UserPreferences;
using ArcTexel.Extensions.CommonApi.Windowing;
using ArcTexel.Extensions.IO;

namespace ArcTexel.Extensions;

public class ExtensionServices
{
    public IServiceProvider Services { get; private set; }
    public IWindowProvider? Windowing => Services.GetService<IWindowProvider>();
    public IFileSystemProvider? FileSystem => Services.GetService<IFileSystemProvider>();
    public IPreferences? Preferences => Services.GetService<IPreferences>();
    public ICommandProvider? Commands => Services.GetService<ICommandProvider>();
    public IPalettesProvider? Palettes => Services.GetService<IPalettesProvider>();
    public IDocumentProvider Documents => Services.GetService<IDocumentProvider>();
    public ICommandSupervisor CommandSupervisor => Services.GetService<ICommandSupervisor>();
    public IVisualTreeProvider VisualTree => Services.GetService<IVisualTreeProvider>();
    public IUserDataProvider UserDataProvider => Services.GetService<IUserDataProvider>();
    public ILogger Logger => Services.GetService<ILogger>();

    public ExtensionServices(IServiceProvider services)
    {
        Services = services;
    }
}
