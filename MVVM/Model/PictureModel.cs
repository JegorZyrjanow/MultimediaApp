using System;

namespace MultimediaApp;

[Serializable]
public class PictureModel
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public string Path { get; set; }
    public int Id { get; set; }

    private PictureModel() { }

    internal PictureModel(string name, string category, string path)
    {
        Name = name;
        Tag = category;
        Path = path;
    }
}
