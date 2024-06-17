public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }

    public char _value;

    public TrieNode(char value = ' ')
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        _value = value;
    }

    public bool HasChild(char c)
    {
        return Children.ContainsKey(c);
    }
}

public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }
    
    // Search for a word in the trie
    public bool Search(string word){
        TrieNode current = root;

        // For each character in the word
        foreach (char c in word)
        {
            // If the current node does not have the character as a child
            if (!current.HasChild(c))
            {
                // The word does not exist in the trie
                return false;
            }
            // Move to the child node
            current = current.Children[c];
        }
        // Return true if the current node marks the end of the word
        return current.IsEndOfWord;
    }

    public bool Insert(string word)
    {
        TrieNode current = root;

        // For each character in the word
        foreach (char c in word)
        {
            // If the current node does not have the character as a child
            if (!current.HasChild(c))
            {
                // Add the character as a child
                current.Children[c] = new TrieNode(c);
            }
            // Move to the child node
            current = current.Children[c];
        }
        if (current.IsEndOfWord)
        {
            // Word already exists in the trie
            return false;
        }
        // Mark the end of the word
        current.IsEndOfWord = true;
        return true;
    }
    
    /// <summary>
    /// Retrieves a list of suggested words based on the given prefix.
    /// </summary>
    /// <param name="prefix">The prefix to search for.</param>
    /// <returns>A list of suggested words.</returns>
    public List<string> AutoSuggest(string prefix)
    {
        TrieNode currentNode = root;
        foreach (char c in prefix)
        {
            if (!currentNode.HasChild(c))
            {
                return new List<string>();
            }
            currentNode = currentNode.Children[c];
        }
        return GetAllWordsWithPrefix(currentNode, prefix);
    }

    private List<string> GetAllWordsWithPrefix(TrieNode root, string prefix)
    {
        List<string> words = new List<string>();

        if (root.IsEndOfWord)
        {
            words.Add(prefix);
        }

        foreach (var child in root.Children)
        {
            words.AddRange(GetAllWordsWithPrefix(child.Value, prefix + child.Key));
        }

        return words;
    }

    // Método para ayudar a eliminar una palabra del Trie.
    private bool DeleteHelper(TrieNode root, string word, int index)
    {
        // Si hemos llegado al final de la palabra
        if (index == word.Length)
        {
            // Si la palabra no está en el Trie, no hay nada que eliminar
            if (!root.IsEndOfWord)
            {
                return false;
            }
            // Marcar el nodo como no final de palabra
            root.IsEndOfWord = false;
            // Si el nodo no tiene hijos, debería ser eliminado
            return root.Children.Count == 0;
        }

        // Obtener el carácter en la posición actual
        char c = word[index];
        // Si el nodo no tiene un hijo correspondiente a este carácter, la palabra no está en el Trie
        if (!root.HasChild(c))
        {
            return false;
        }

        // Recursivamente intentar eliminar el resto de la palabra
        bool shouldDeleteCurrentNode = DeleteHelper(root.Children[c], word, index + 1);

        // Si el hijo debería ser eliminado
        if (shouldDeleteCurrentNode)
        {
            // Eliminar el hijo
            root.Children.Remove(c);
            // Si el nodo no tiene más hijos, debería ser eliminado
            return root.Children.Count == 0;
        }

        // Si llegamos a este punto, el nodo no debería ser eliminado
        return false;
    }

    public bool Delete(string word)
    {
        return DeleteHelper(root, word, 0);
    }

    public List<string> GetAllWords()
    {
        return GetAllWordsWithPrefix(root, "");
    }

    public void PrintTrieStructure()
    {
        Console.WriteLine("\nroot");
        _printTrieNodes(root);
    }

    private void _printTrieNodes(TrieNode root, string format = " ", bool isLastChild = true) 
    {
        if (root == null)
            return;

        Console.Write($"{format}");

        if (isLastChild)
        {
            Console.Write("└─");
            format += "  ";
        }
        else
        {
            Console.Write("├─");
            format += "│ ";
        }

        Console.WriteLine($"{root._value}");

        int childCount = root.Children.Count;
        int i = 0;
        var children = root.Children.OrderBy(x => x.Key);

        foreach(var child in children)
        {
            i++;
            bool isLast = i == childCount;
            _printTrieNodes(child.Value, format, isLast);
        }
    }

    public List<string> GetSpellingSuggestions(string word)
    {
        char firstLetter = word[0];
        List<string> suggestions = new();
        List<string> words = GetAllWordsWithPrefix(root.Children[firstLetter], firstLetter.ToString());
        
        foreach (string w in words)
        {
            int distance = LevenshteinDistance(word, w);
            if (distance <= 2)
            {
                suggestions.Add(w);
            }
        }

        return suggestions;
    }

    private int LevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        if (m == 0)
        {
            return n;
        }

        if (n == 0)
        {
            return m;
        }

        for (int i = 0; i <= m; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= n; j++)
        {
            d[0, j] = j;
        }

        for (int j = 1; j <= n; j++)
        {
            for (int i = 1; i <= m; i++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return d[m, n];
    }
}