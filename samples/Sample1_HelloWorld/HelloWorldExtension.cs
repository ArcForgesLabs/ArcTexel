namespace HelloWorld;

using ArcTexel.Extensions.Sdk;

public class HelloWorldExtension : ArcTexelExtension
{
    /// <summary>
    ///     This method is called when extension is loaded.
    ///  All extensions are first loaded and then initialized. This method is called before <see cref="OnInitialized"/>.
    /// </summary>
    public override void OnLoaded()
    {
    }

    /// <summary>
    ///     This method is called when extension is initialized. After this method is called, you can use Api property to access ArcTexel API.
    /// </summary>
    public override void OnInitialized()
    {
        Api.Logger.Log("Hello World!");
    }
}