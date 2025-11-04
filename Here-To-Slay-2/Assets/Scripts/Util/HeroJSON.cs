namespace Util
{
    [System.Serializable]
    /// <summary>
    /// Hero card data structure for JSON deserialization
    /// </summary>
    public class HeroJSON
    {
        public int id;
        public string type;
        public string name;
        public string img; // Image path
        public string heroClass;
        public int minRoll;
        public string description;
    }
}

