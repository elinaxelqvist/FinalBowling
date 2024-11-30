using System.Collections.Generic;
using System.Linq;

public class Score : IEnumerable<int>
{
    private List<int> throwScores = new List<int>();
    public int FrameCount => throwScores.Count / 2;  // Antal färdiga frames

    public IEnumerator<int> GetEnumerator()
    {
        return throwScores.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddPoints(int points)
    {
        throwScores.Add(points);
    }

    public int GetTotalScore() 
    {
        return throwScores.Sum();
    }

    // Ny metod för att få poäng per frame
    public IEnumerable<(int frameNumber, int firstThrow, int secondThrow, int frameTotal)> GetFrameScores()
    {
        for (int i = 0; i < throwScores.Count - 1; i += 2)
        {
            int frameNumber = (i / 2) + 1;
            int firstThrow = throwScores[i];
            int secondThrow = throwScores[i + 1];
            yield return (frameNumber, firstThrow, secondThrow, firstThrow + secondThrow);
        }
    }
}