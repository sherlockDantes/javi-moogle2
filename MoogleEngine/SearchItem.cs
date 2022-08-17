namespace MoogleEngine;

public class SearchItem
{
    public SearchItem(string title, string filePath, string snippet, double score)
    {
        this.Title = title;
        this.FilePath = filePath;
        this.Snippet = snippet;
        this.Score = score;
    }

    public string Title { get; private set; }

    public string Snippet { get; set; }

    public double Score { get; set; }

    public string FilePath { get; private set; }
}
