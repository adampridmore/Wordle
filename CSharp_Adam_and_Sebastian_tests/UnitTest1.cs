namespace CSharp_Adam_and_Sebastian_tests;

public class Tests
{
    [Test]
    public void NoMatch()
    {
        var guess = "WORDS";
        var actualWord = "ZZZZZ";
        var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

        Assert.AreEqual("     ", result);
    }

    [Test]
    public void FullMatch()
    {
        var guess = "WORDS";
        var actualWord = "WORDS";
        var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

        Assert.AreEqual("GGGGG", result);
    }

    [Test]
    public void OneYellowMatch()
    {
        var guess = "A     ";
        var actualWord = "BBBBA";
        var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

        Assert.AreEqual("Y    ", result);
    }

    [Test]
    public void MixMatches(){
        var guess = "ABBZZ";
        var actualWord = "ABCDE";
        var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

        Assert.AreEqual("GGY  ", result);
    }


    [Test]
    public void GetNextGuess(){
        var solver = new Solver(File.ReadAllLines("words.txt").ToList());

        var nextGuess = solver.GetNextGuess("words", "G  GG");

        Assert.AreEqual("winds", nextGuess);
    }

    // [Test]
    // public void FilterWordList() {
    //     var words = File.ReadAllLines("words.txt");

    //     var guess = "words"; // wards
    //     var actualResult = "G  GG";

    //     Console.WriteLine($"First Word: [{words.First()}]");

    //     var filteredWords = words.Where(word => Solver.ScoreGuessAgainstWord(guess, word) == actualResult);

    //     Console.WriteLine($"Filtered {filteredWords.Count()}");
    //     foreach(var filteredWord in filteredWords) {
    //         Console.WriteLine(filteredWord);
    //     }
    // }
}
