// CMPT 306
// ASSIGNMENT 1
// KODY MANSTYRSKI
// KOM607
// 11223681


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class Generator : MonoBehaviour{

    // handed in values from unity
    public int width, height;
    public bool useSeed;
    public string seed;
    public GameObject[] sprites;
    public String rules_file_path;

    // values used for displaying in unity
    private string[,] tiles;
    private GameObject[,] map;
    
    // values used to perform algorithms
    private string[] alphabet;
    private Dictionary<String, int> letter_tile;
    private Dictionary<string, string[][,]> words;
    private System.Random rand;
    Tree map_generator;
    
    // Start is called before the first frame update
    void Start() {
        ReadFile(); // read in alphabet and production rules
        // initialze the random generator 

        if (useSeed) {
            rand = new System.Random(seed.GetHashCode());
        }
        else {
            rand = new System.Random();
        }
        map = new GameObject[width, height];
        map_generator = new Tree(0, width, 0, height, (int)(0.33f * (float)(width*height)), true);
        map_generator.GenerateMap(new ThreadArgs(alphabet, words, rand));
        tiles = map_generator.getMap();
        Display();
    }

    /**
    A method to instantiate the tiles of the map and store them in the event they need to be destroyed.
    @Preconditions: A set of sprites for each tile is defined, the tiles array has been filled in some way with 
    values from the alphabet available, and map has been initialized.
    @Postconditions: Map is filled with freshly instantiated gameobject references
    **/
    void Display(){
        for (int i = 0; i < width; i += 1) {
            for (int j = 0; j < height; j += 1) {
                if (alphabet[0] != tiles[i, j] | null != tiles[i, j]) {
                    Vector4 pos = new Vector4((((float)i / (float)width) - 0.5f) * 7.50f, (((float)j / (float)height) - 0.495f)* 7.50f, 2.0f, 1.0f); // create a position for the current tile
                    if (null != tiles[i, j]) {
                        Destroy(map[i, j]); // if the current map position already has a tile, delete it to save memory
                        map[i, j] = Instantiate(sprites[letter_tile[tiles[i,j]]], pos, Quaternion.identity); // create new gameobject and store it in the map
                    }
                    else{
                        map[i,j] = Instantiate(sprites[0],pos, Quaternion.identity);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {        
    }

    /**
    A method to read the ruleset of the grammar from a plaintext file
    @Preconditions: The path to the rules file is valid, and the file is organized as follows:
        First line: two numbers defining the dimensions of any production rule seperated by a space. 
        E.G. 3 3 says any production rule will be 3x3
        Second line: a set of words where the first is the null placeholder, the second is the starter placeholder, and the last is the terminal (unused)
        Third line: a mapping between letter and sprite given as a space seperated string of numbers which refer to the indices of the sprite array handed into this program.
        E.G. If the alphabet is n s a b c t, and 4 tiles are handed in, then the input 0 0 1 2 3 0 maps n, s and t to the 0th tile, a to the first, b to the second, and c to 
        the third.
        All following lines: words to production rule sets. Words are seperated from their production rules by a colon (:), production rules are seperated by commas (,)
        and lines of a production rule are seperated by spaces. The production rules must fit the dimensions defined in the First line
        E.G. (with the above first line) a: x x x  x x x  x x x, y y y  y y y  y y y
    @Postconditions: the alphabet set is filled, and the words dicitonary is filled, both in accordance with the above definitions.
    **/
    void ReadFile(){
        words = new Dictionary<String, String[][,]>();
        System.String[] input = System.IO.File.ReadAllLines(rules_file_path); // read file into an array
        int size_x, size_y;
        try{

            // attempt to obtain the dimensions of production rules
            String[] sizes = input[0].Split();
            size_x = Int32.Parse(sizes[0]);
            size_y = Int32.Parse(sizes[1]);
        }
        catch(FormatException){
            print("Failed to parse size input");
            size_x = 0;
            size_y = 0;
        }
        String[] temp_alphabet = input[1].Split(new String[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        alphabet = new String[temp_alphabet.Length]; // obtain the alphabet
        String[] temp_letter_tile = input[2].Split(new String[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        letter_tile = new Dictionary<String, int>();
        for(int i = 0; i < temp_letter_tile.Length; i +=1){
            try{
                alphabet[i] = temp_alphabet[i];
                int index = Int32.Parse(temp_letter_tile[i]);
                letter_tile.Add(alphabet[i], index);
            }
            catch(FormatException){
                print("Failed to parse index input for index " + i);
            }
        }

        Array.Copy(temp_alphabet, alphabet, temp_alphabet.Length);
        for(int i = 3; i < input.Length; i += 1){

            string[] line = input[i].Split(new String[]{":"}, StringSplitOptions.RemoveEmptyEntries); // splits letter from rules

            string referent = line[0].ToString().Trim(); // stores letter
            
            string[] productions = line[1].Split(new String[]{","}, StringSplitOptions.RemoveEmptyEntries); // splits different rules appart i.e. 'a b c  d e f  g h i, z y x  w u v  t s r' -> ['a b c  d e f  g h i', 'z y x  w u v  t s r']
            
            int rule_count = productions.Length;

            words.Add(referent, new String[rule_count][,]);

            for(int subset = 0; subset < rule_count; subset += 1){
                words[referent][subset] = new String[size_x, size_y];
                String[,] word_production = words[referent][subset];

                String[] rule = productions[subset].Split(new String[]{" ", "  "}, StringSplitOptions.RemoveEmptyEntries);
                for(int x = 0; x < size_x; x +=1){
                    for(int y = 0; y < size_y; y += 1){
                        word_production[x,y] = rule[(3*x)+y];
                    }
                }
            }
        }
    }


    void GenerateNewMap(){
        Array.Clear(map, 0, map.Length);
        Array.Clear(tiles, 0, tiles.Length);
        map_generator.Clear();
        map_generator.GenerateMap(new ThreadArgs(alphabet, words, rand));
        tiles = map_generator.getMap();
        Display();
    }

    void Regenerate(){
        Array.Clear(map, 0, map.Length);
        Array.Clear(tiles, 0, tiles.Length);
        map_generator.Clear();
        if(seed != "" & seed != null){
            print(seed);
            rand = new System.Random(seed.GetHashCode());
        }
        else{
            int val = rand.Next();
            rand = new System.Random(val);
        }
        map_generator.GenerateMap(new ThreadArgs(alphabet, words, rand));
        tiles = map_generator.getMap();
        Display();
    }

    void ChangeSeed(){
        InputField seeder = GameObject.Find("Seeder").GetComponent(typeof(InputField)) as InputField;
        string input_value = seeder.text;
        this.seed = input_value;
        rand = new System.Random(seed.GetHashCode());
    }

    public String[,] getStringMap(){
        return tiles;
    }

    public GameObject[,] getObjectMap(){
        return map;
    }

}

/** A placeholder class for storing a tile and its coordinates**/
public class Tile {

    private string value;
    private int x, y;

    public Tile(String value, int x, int y) {
        this.x = x;
        this.y = y;
        this.value = value;
    }

    public String getValue() {
        return value;
    }

    public int getX() {
        return x;
    }

    public int getY() {
        return y;
    }
}

public class ThreadArgs{
    public string[] alphabet;
    public Dictionary<string, string[][,]> words;
    public System.Random rand;

    public ThreadArgs(string[] alpha, Dictionary<string, string[][,]> dict, System.Random random){
        alphabet = alpha;
        words = dict;
        rand = random;
    }
}

public class Tree{
    private int left_bound, right_bound, upper_bound, lower_bound;
    private int width, height;
    private Tree[] children;
    private String[,] map;
    private Queue<Tile> frontier;
    private int min_area;
    private bool vertical;
    /** 
    A constructor for a new tree, used to create map using subspace partitioning and applying L-system rules
    @Param left: the left bound of the array contained by this tree
    @Param right: the right bound of the array contained by this tree
    @Param upper: the upper bound of the array contained by this tree
    @Param lower: the lower bound of the array contained by this tree
    @Param threshold: the minimal size the map array can be
    @Param vertical: Whether or not to subdivide vertically
    **/
    public Tree(int left, int right, int upper, int lower, int threshold, bool vertical){
        
        left_bound = left;
        right_bound = right;
        upper_bound = upper;
        lower_bound = lower;

        width = right - left;
        height = lower - upper;
        
        map = new String[width, height];
        
        min_area = threshold;
        
        this.vertical = vertical;

        frontier = new Queue<Tile>();
    }


    public String[,] getMap(){
        return map;
    }

    public void Clear(){
        if(children != null){
            foreach(Tree child in children){
                child.Clear();
            }
        }
        map = new String[width, height];
        children = new Tree[2];
    }

    public override String ToString(){
        string output = "\nw: " + width + "\t:H: " + height;
        if(children != null){
            output += "\tChildren: " + children.Length + "\tChildren Trees";
            foreach(Tree child in children){
                output += child.ToString().Replace("\n", "\n\t");
            }
        }
        return output;
    }

    public bool isLeaf(){
        return (width * height) <= min_area;
    }

    /**
    A method to generate a new map using subspace partitioning and applying the given L-System rules.
    Note: This function is multi-threaded, creating threads for each child
    @Preconditions: The handed in parameters are not null, the map array is initialized, and the bounds
    are correctly ordered and formatted (left < right, upper < lower)
    @Param args: Should be passed in as a ThreadArgs object which holds parameters string[] alphabet, Dictionary<String, String[][,]> words, and System.Random rand
    @Postconditions: Map is filled
    **/
    public void GenerateMap(System.Object args){
        ThreadArgs arguments = (ThreadArgs)args;
        if(width * height > min_area & (width > min_area/2 & height > min_area/2)){
            children = new Tree[2];
            int division;
            if(vertical){
            
                division = left_bound + (arguments.rand.Next(width));
                if(division > width -(min_area/2) & width - (min_area/2) > min_area/2){
                    division = width - (min_area/2);
                }


                children[0] = new Tree(left_bound, division, upper_bound, lower_bound, min_area, false);
                children[1] = new Tree(division + 1, right_bound, upper_bound, lower_bound, min_area, false);

                if(children[0].isLeaf() & children[1].isLeaf()){
                    Thread child1 = new Thread(new ParameterizedThreadStart(children[0].GenerateMap));
                    Thread child2 = new Thread(new ParameterizedThreadStart(children[1].GenerateMap));

                    child1.Start(arguments);
                    child2.Start(arguments);

                    child1.Join();
                    child2.Join();
                }
                else{
                    children[0].GenerateMap(args);
                    children[1].GenerateMap(args);
                }

                String[,] c_map_1 = children[0].getMap();
                String[,] c_map_2 = children[1].getMap();


                for(int i = 0; i < c_map_1.GetLength(0); i += 1){
                    for(int j = 0; j < height; j +=1){
                        map[i,j] = (c_map_1[i,j]!=null)?c_map_1[i,j]:arguments.alphabet[0];
                    }
                }
                
                
                for(int i = division+1; i < width; i += 1){
                    for(int j = 0; j < height; j +=1){
                        map[i,j] = (c_map_2[i - (division+1),j]!=null)?c_map_2[i-(division+1),j]:arguments.alphabet[0];

                    }
                }
            }


            else{
                division = upper_bound + (arguments.rand.Next(height));
                if(division > height - (min_area/2) & height - (min_area/2) > min_area/2){
                    division = height - (min_area/2);
                }
            
                children[0] = new Tree(left_bound, right_bound, upper_bound, division, min_area, true);
                children[1] = new Tree(left_bound, right_bound, division + 1, lower_bound, min_area, true);
            
                Thread child1 = new Thread(new ParameterizedThreadStart(children[0].GenerateMap));
                Thread child2 = new Thread(new ParameterizedThreadStart(children[1].GenerateMap));

                child1.Start(arguments);
                child2.Start(arguments);

                child1.Join();
                child2.Join();

                String[,] c_map_1 = children[0].getMap();
                String[,] c_map_2 = children[1].getMap();


                for(int i = 0; i < width; i += 1){
                    for(int j = 0; j < division; j +=1){
                        map[i,j] = (c_map_1[i,j]!=null)?c_map_1[i,j]:arguments.alphabet[0];
                    }
                }
            

                
                for(int i = 0; i < width; i += 1){
                    for(int j = division + 1; j < c_map_2.GetLength(1); j +=1){
                        map[i,j] = (c_map_2[i,j - (division+1)]!=null)?c_map_2[i, j-(division+1)]:arguments.alphabet[0];
                    }
                }
            }
        }
        else{
            init_tiles(arguments.alphabet, arguments.rand);
            Grow(arguments.alphabet, arguments.words, arguments.rand);
        }
    }

    /**
    A method to initialize the algorithm variables used in map generation
    @Preconditions: nexts and map are initialized
    @Param alphabet: An array of the possible strings in a given map
    @Param words: A dictionary mapping alphabet entries to production rule sets
    @Param rand: A random number generator
    @Postconditions: the above noted variables are initialized for the current map
    **/
    private void init_tiles(String[] alphabet, System.Random rand) {
        int x = rand.Next() % width;
        int y = rand.Next() % height;
        map[x, y] = alphabet[1]; // place a starting tile at the center of the board
        frontier.Enqueue(new Tile(alphabet[1], x, y)); // enqueue the starting tile for use with grow
    }

    /**
    A method to automate the swapping of map tiles
    @Preconditions: frontier has been initialized and has a starting tile in it, the map array has been initialized, an alphabet is available for
    reference, and a set of production rules is available.
    @Param alphabet: An array of the possible strings in a given map
    @Param words: A dictionary mapping alphabet entries to production rule sets
    @Param rand: A random number generator
    @Postconditions: The map array is filled, and the nexts queue is emptied
    **/
    void Grow(string[] alphabet, 
        Dictionary<string, string[][,]> words, 
        System.Random rand) {
        while (frontier.Count > 0) {
            Tile current = frontier.Dequeue(); // obtain the oldest tile
            Swap(current, alphabet, words, rand); // grow that tile
        }
    }

    /**
    A method to replace an mxn space within the tile array with a random corresponding production rule for the centered value
    @Preconditions: the tile array is initialized, the central value at the least has a starting character, the nexts queue has been initialized,
    and production rules are available,
    @Parameter tile: A tile data type defined below
    @Param alphabet: An array of the possible strings in a given map
    @Param words: A dictionary mapping alphabet entries to production rule sets
    @Param rand: A random number generator
    @Postconditions: Supposing the area surrounding the central 'start' tile is completely empty (holds null placeholder strings), the area is filled
    in accordance with the production rule, adjusting for array bounds. If a tile is not a null tile, it is left alone. Finally the nexts queue has a
    new value enqueued for each newly placed tile
    **/
    void Swap(Tile tile, string[] alphabet, 
        Dictionary<string, string[][,]> words, 
        System.Random rand) {

        int x = tile.getX(); // get the coordinates of the current tile
        int y = tile.getY();

        int idx = 0;

        if(words[tile.getValue()].Length > 1){
            idx = rand.Next() % words[tile.getValue()].Length; // select a random index for the production rule respective to the tile
        }

        string[,] set = words[tile.getValue().Trim()][idx]; // obtain the production rules
        
        // obtain the dimensions of the rule as indices in the map
        int upper = y - (set.GetLength(1)/2);
        int lower = y + (set.GetLength(1)/2);

        int left = x - (set.GetLength(0)/2);
        int right = x + (set.GetLength(0)/2);

        for (int i = left; i <= right; i += 1) {
        
            for (int j = upper; j <= lower; j += 1) {
            
                int possibility_x = i + 1 - x;
                int possibility_y = j + 1 - y;
                
                if (i > 0 & j > 0 &
                    i < width & j < height) {
                    string newtile = set[possibility_x, possibility_y]; // obtain the elements of the selected production rule 1 by 1

                    if (map[i, j] == null | map[i, j] == alphabet[1] | map[i,j] == alphabet[0]){ // existing is null or starting tile
                        
                        if (newtile != alphabet[0] & newtile != alphabet[alphabet.Length - 1] & newtile != null) { // non-null non-starting tile replacement
                            map[i, j] = newtile;
                            
                            if (i != y | j != x) {
                                frontier.Enqueue(new Tile(newtile, i, j)); // enqueue the tile for future growth
                            }
                        }   
                    }
                }
            }
        }
    }

}