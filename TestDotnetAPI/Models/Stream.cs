using System.Data;
using ErrorOr;
using TestDotnetAPI.Contracts.Event;
using TestDotnetAPI.ServiceErrors;

namespace TestDotnetAPI.Models;

public class Stream
{
    public Guid Id { get; }
    public string Name { get; }
    public string Url { get; }
    public string File { get; }
    
    public const int MAX_NAME_LENGTH = 50;
    public const int MAX_URL_LENGTH = 100;

    private Stream(
        Guid id,
        string name,
        string url,
        string file
    )
    {
        Id = id;
        Name = name;
        Url = url;
        File = file;
    }

    private static ErrorOr<Stream> Create(string name, string url, string file, Guid? id = null)
    {
        List<Error> errors = new List<Error>();
        if (name.Length is > MAX_NAME_LENGTH)
            errors.Add(Errors.Stream.InvalidName);
        if (url.Length is > MAX_URL_LENGTH)
            errors.Add(Errors.Stream.InvalidUrl);
        if (file.Length is > MAX_URL_LENGTH)
            errors.Add(Errors.Stream.InvalidUrl);
        if (errors.Count > 0)
            return errors;

        return new Stream(
            id ?? Guid.NewGuid(),
            name,
            url, 
            file
        ); 
    }

    public static ErrorOr<Stream> From(StreamResponse response)
    {
        return Create(response.Name, response.Link, response.File);
    }
    public static ErrorOr<List<Stream>>From(List<StreamResponse> responses)
    {
        var streams = new List<Stream>() { };
        foreach (StreamResponse s in responses)
        {
            var stream = From(s);
            if (stream.IsError) return stream.Errors;
            streams.Add(stream.Value);
        }
        return streams;
    }
    public static ErrorOr<List<Stream>> From(DataTable table)
    {
        var streams = new List<Stream>() { };
        foreach (DataRow row in table.Rows)
        {
            var stream = Create(
                row["name"].ToString(),
                row["url"].ToString(),
                row["file"].ToString(),
                Guid.Parse(row["id"].ToString())
            );
            if (stream.IsError) return stream.Errors;
            streams.Add(stream.Value);
        }
        return streams;
    }
}