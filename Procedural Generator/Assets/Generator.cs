// CMPT 306
// ASSIGNMENT 1
// KODY MANSTYRSKI
// KOM607
// 11223681


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generator : MonoBehaviour
{

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
    private Queue<Tile> nexts;
    private System.Random rand;
    
    /**
    A method to initialize the algorithm variables used in map generation
    @Preconditions: nexts, map, and tiles are all uninitialized
    @Postconditions: the above noted variables are initialized for the current map
    **/
    void init_tiles() {
        // Initialize storage variables
        nexts = new Queue<Tile>(); 
        map = new GameObject[width, height];
        tiles = new string[width, height];
        tiles[(width - 1) / 2, (height - 1) / 2] = alphabet[1]; // place a starting tile at the center of the board
        nexts.Enqueue(new Tile(alphabet[1], (width - 1)/2, (height - 1)/2)); // enqueue the starting tile for use with grow
    }

    /**
    A method to replace an mxn space within the tile array with a random corresponding production rule for the centered value
    @Preconditions: the tile array is initialized, the central value at the least has a starting character, the nexts queue has been initialized,
    and production rules are available,
    @Parameter tile: A tile data type defined below
    @Postconditions: Supposing the area surrounding the central 'start' tile is completely empty (holds null placeholder strings), the area is filled
    in accordance with the production rule, adjusting for array bounds. If a tile is not a null tile, it is left alone. Finally the nexts queue has a
    new value enqueued for each newly placed tile
    **/
    void Swap(Tile tile) {

        int x = tile.getX(); // get the coordinates of the current tile
        int y = tile.getY();

        int idx = 0;

        if(words[tile.getValue()].Length > 1){
            idx = rand.Next() % words[tile.getValue()].Length; // select a random index for the production rule respective to the tile
        }

        string[][,] set = words[tile.getValue().Trim()]; // obtain the production rules
        
        for (int i = y - 1; i <= y + 1; i += 1) {
        
            for (int j = x - 1; j <= x + 1; j += 1) {
            
                int possibility_x = j + 1 - x;
                int possibility_y = i + 1 - y;
                
                if (i >= 0 & j >= 0 &
                    i < width & j < height) {
                    string newtile = words[tile.getValue()][idx][possibility_x, possibility_y]; // obtain the elements of the selected production rule 1 by 1

                    if (tiles[i, j] == null | tiles[i, j] == alphabet[2]){ // existing is null or starting tile
                        if (newtile != alphabet[0] & newtile != alphabet[alphabet.Length - 1] & newtile != null) { // non-null non-starting tile replacement
                            tiles[i, j] = newtile;
                            if (i != y | j != x) {
                                Tile enqueued_tile = new Tile(newtile, i, j);
                                nexts.Enqueue(new Tile(newtile, i, j)); // enqueue the tile for future growth
                            }
                        }   
                    }
                }
            }
        }
    }

    /**
    A method to automate the swapping of map tiles
    @Preconditions: nexts has been initialized and has a starting tile in it, the tiles array has been initialized, an alphabet is available for
    reference, and a set of production rules is available.
    @Postconditions: The tile array is filled, and the nexts queue is emptied
    **/
    void Grow() {
        while (nexts.Count > 0) {
            Tile current = nexts.Dequeue(); // obtain the oldest tile
            Swap(current); // grow that tile
        }
    }

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

        init_tiles(); // initialize storage
        Grow(); // fill map
        Display(); // display map
    }

    /**
    A method to instantiate the tiles of the map and store them in the event they need to be destroyed.
    @Preconditions: A set of sprites for each tile is defined, the tiles array has been filled in some way with 
    values from the alphabet available, and map has been initialized.
    @Postconditions: Map is filled with freshly instantiated gameobject references
    **/
    void Display(){
        Matrix4x4 view = Camera.main.worldToCameraMatrix; // obtain the view matrix to scale tiles to screen
        for (int i = 0; i < width; i += 1) {
            for (int j = 0; j < height; j += 1) {
                if (alphabet[0] != tiles[i, j] | null != tiles[i, j]) {
                    Vector4 pos = new Vector4((((float)i / (float)width) - 0.5f) * 10.0f, (((float)j / (float)height) - 0.5f)* 10.0f, -3.0f, 1.0f); // create a position for the current tile
                    if (null != tiles[i, j]) {
                        Destroy(map[i, j]); // if the current map position already has a tile, delete it to save memory
                        map[i, j] = Instantiate(sprites[letter_tile[tiles[i,j]]], view * pos, Quaternion.identity); // create new gameobject and store it in the map
                    }
                    else{
                        map[i,j] = Instantiate(sprites[0], view*pos, Quaternion.identity);
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
        E.G. (with the above first line) a: xxx xxx xxx, yyy yyy yyy
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
            
            string[] productions = line[1].Split(new String[]{","}, StringSplitOptions.RemoveEmptyEntries); // splits different rules appart i.e. 'abc def ghi, zyx wuv tsr' -> ['abc def ghi', 'zyx wuv tsr']
            
            int rule_count = productions.Length;

            words.Add(referent, new String[rule_count][,]);

            for(int subset = 0; subset < rule_count; subset += 1){
                words[referent][subset] = new String[size_x, size_y];
                String[,] word_production = words[referent][subset];
                for(int x = 0; x < size_x; x +=1){
                    String[] rule = productions[subset].Split(new String[]{" "}, StringSplitOptions.RemoveEmptyEntries);
                    for(int y = 0; y < size_y; y += 1){
                        word_production[x,y] = rule[x][y].ToString();
                    }
                }
            }
        }
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