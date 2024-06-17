namespace TrieDictionaryTest;

[TestClass]
public class TrieTest
{
    // Test that a word is inserted into the trie
    [TestMethod]
    public void InsertWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        trie.Insert(word);

        // Assert
        Assert.IsTrue(trie.Search(word));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void DeleteWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        trie.Insert(word);
        trie.Delete(word);

        // Assert
        Assert.IsFalse(trie.Search(word));
    }

    // Test that a word is not inserted twice in the trie
    [TestMethod]
    public void InsertDuplicateWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        trie.Insert(word);

        // Assert
        Assert.IsFalse(trie.Insert(word));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void DeleteNonExistentWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";

        // Act
        trie.Insert(word);

        // Assert
        Assert.IsFalse(trie.Delete("world"));
        Assert.IsTrue(trie.Search(word));
    }

    // Test that a word is not deleted from the trie if it is not present
    [TestMethod]
    public void DeletePrefixWord()
    {
        // Arrange
        var trie = new Trie();
        var word = "hello";
        var prefix = "hell";

        // Act
        trie.Insert(word);
        trie.Insert("hello world");

        // Assert
        Assert.IsFalse(trie.Delete(prefix));
        Assert.IsFalse(trie.Search(prefix));
        Assert.IsTrue(trie.Search(word));
    }

    // Test that a word is deleted from the trie if it is a prefix of another word
    [TestMethod]
    public void AutoSuggest()
    {
        // Arrange
        var trie = new Trie();
        var words = new[] { "catastrophe", "catatonic", "caterpillar" };
        var prefix = "cat";

        // Act
        foreach (var word in words)
        {
            trie.Insert(word);
        }

        // Assert
        var suggestions = trie.AutoSuggest(prefix);
        Assert.AreEqual(3, suggestions.Count);
        Assert.IsTrue(suggestions.Contains("catastrophe"));
        Assert.IsTrue(suggestions.Contains("catatonic"));
        Assert.IsTrue(suggestions.Contains("caterpillar"));
    }

    // Test AutoSuggest for the prefix "cat" not present in the 
    // trie containing "catastrophe", "catatonic", and "caterpillar"
    [TestMethod]
    public void AutoSuggestPrefixNotPresent()
    {
        // Arrange
        var trie = new Trie();
        var words = new[] { "catastrophe", "catatonic", "caterpillar" };
        var prefix = "cat";

        // Act
        foreach (var word in words)
        {
            trie.Insert(word);
        }

        // Assert
        var suggestions = trie.AutoSuggest(prefix);
        Assert.AreEqual(3, suggestions.Count);
        Assert.IsTrue(suggestions.Contains("catastrophe"));
        Assert.IsTrue(suggestions.Contains("catatonic"));
        Assert.IsTrue(suggestions.Contains("caterpillar"));
    }

    // Test GetSpellingSuggestions for a word not present in the trie
    [TestMethod]
    public void TestGetSpellingSuggestions()
    {
        Trie dictionary = new Trie();
        dictionary.Insert("cat");
        dictionary.Insert("caterpillar");
        dictionary.Insert("catastrophe");
        List<string> suggestions = dictionary.GetSpellingSuggestions("caterpiller");
        Assert.AreEqual(1, suggestions.Count);
        Assert.AreEqual("caterpillar", suggestions[0]);
    }
    


}