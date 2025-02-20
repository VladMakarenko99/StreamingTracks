namespace Application.Abstractions;

public class FormFileAbstraction : TagLib.File.IFileAbstraction
{
    private readonly Stream _stream;

    public FormFileAbstraction(string name, Stream stream)
    {
        Name = name;
        _stream = stream;
    }

    public string Name { get; }

    public Stream ReadStream => _stream;
    public Stream WriteStream => _stream;

    public void CloseStream(Stream stream)
    {
        stream.Close();
    }
}