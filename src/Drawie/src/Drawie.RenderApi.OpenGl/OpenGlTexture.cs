using Silk.NET.OpenGL;

namespace Drawie.RenderApi.OpenGL;

public class OpenGlTexture : IOpenGlTexture, IDisposable
{
    public uint TextureId { get; }

    private GL Api { get; set; }
    
    public OpenGlTexture(uint textureId, GL api)
    {
        TextureId = textureId;
        Api = api;
    }

    public unsafe OpenGlTexture(GL api, int width, int height)
    {
        TextureId = api.GenTexture();

        Api = api;
        Activate(0);
        Bind();

        Api.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)width, (uint)height, 0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte, null);

        int repeat = (int)GLEnum.Repeat;
        int nearest = (int)GLEnum.Nearest;
        Api.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, in repeat);
        Api.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, in repeat);
        Api.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, in nearest);
        Api.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, in nearest);
    }

    public void Bind()
    {
        Api.BindTexture(TextureTarget.Texture2D, TextureId);
    }

    public void Activate(int textureUnit)
    {
        Api.ActiveTexture(TextureUnit.Texture0 + textureUnit);
    }

    public void Dispose()
    {
        Api.DeleteTexture(TextureId);
    }
}
