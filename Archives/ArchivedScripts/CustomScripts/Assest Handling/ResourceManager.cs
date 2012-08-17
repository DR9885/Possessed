using UnityEngine;

public class ResourceManager
{
    private static ResourceManager _Instance = new ResourceManager();


    [SerializeField]
    private FontResources _FontResources;
    public static FontResources FontResources
    {
        get
        {
            if (_Instance._FontResources == null)
                _Instance._FontResources = new FontResources();
            return _Instance._FontResources;
        }
    }

    [SerializeField]
    private GUISkinResources _GUISkinResources;
    public static GUISkinResources GUISkinResources
    {
        get
        {
            if (_Instance._GUISkinResources == null)
                _Instance._GUISkinResources = new GUISkinResources();
            return _Instance._GUISkinResources;
        }
    }

    [SerializeField]
    private TextureResources _TextureResources;
    public static TextureResources TextureResources
    {
        get
        {
            if (_Instance._TextureResources == null)
                _Instance._TextureResources = new TextureResources();
            return _Instance._TextureResources;
        }
    }

    [SerializeField]
    private ParticleResources _ParticleResources;
    public static ParticleResources ParticleResources
    {
        get
        {
            if (_Instance._ParticleResources == null)
                _Instance._ParticleResources = new ParticleResources();
            return _Instance._ParticleResources;
        }
    }

    [SerializeField]
    private AnimationResources _AnimationResources;
    public static AnimationResources AnimationResources
    {
        get
        {
            if (_Instance._AnimationResources == null)
                _Instance._AnimationResources = new AnimationResources();
            return _Instance._AnimationResources;
        }
    }


    [SerializeField]
    private MovieTextureResources _MovieTextureResources;
    public static MovieTextureResources MovieTextureResources
    {
        get
        {
            if (_Instance._MovieTextureResources == null)
                _Instance._MovieTextureResources = new MovieTextureResources();
            return _Instance._MovieTextureResources;
        }
    }

    //ResourceManager()
    //{
    //    if (AnimationResources != null) { }
    //    if (ParticleResources != null) { }
    //    if (TextureResources != null) { }
    //    if (FontResources != null) { }
    //    if (AnimationResources != null) { }
    //}


}