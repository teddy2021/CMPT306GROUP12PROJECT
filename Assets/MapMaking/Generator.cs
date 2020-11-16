// CMPT 306
// ASSIGNMENT 1
// KODY MANSTYRSKI
// KOM607
// 11223681


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Pathfinding;
using System;
using System.Threading;

public class Generator : MonoBehaviour{

    // handed in values from unity
    [Range(10,500)]
    [SerializeField] private int width, height;
    [SerializeField] private bool useSeed;
    [SerializeField] private string seed;
    [SerializeField] private TileBase[] sprites;
    [SerializeField] private String rules_file_path;
    [SerializeField] private Tilemap Walls, Ground;

    // values used for displaying in unity
    private string[,] tiles;
    
    // values used to perform algorithms
    private string[] alphabet;
    private Dictionary<string, string[][,]> words;
    private Dictionary<string, string> mapping;
    private System.Random rand;
    private int r_width, r_height;
    Tree map_generator;
    
    // Start is called before the first frame update
    void Start() {
        mapping = new Dictionary<String, String>();
        ReadFile(); // read in alphabet and production rules
        // initialze the random generator 
       
        if (useSeed) {
            rand = new System.Random(seed.GetHashCode());
        }
        else {
            rand = new System.Random();
        }
        map_generator = new Tree(0, width, 0, height, (int)(0.25f * (float)(r_width*r_height)), true, r_width, r_height);
        map_generator.GenerateMap(new ThreadArgs(alphabet, words, rand));
        tiles = map_generator.getMap();
        int x = 0;
        foreach(String s in tiles){
            if(s == "wall" | s == "ground"){
                x += 1;
            }
        }
        
        float w = (float)tiles.GetLength(0);
        float h = (float)tiles.GetLength(1);

        for(x = 0; x < tiles.GetLength(0); x += 1){
            for( int y = 0; y < tiles.GetLength(1); y += 1){
                tiles[x,0] = "Wall";
                tiles[0,y] = "Wall";
                tiles[x, tiles.GetLength(1) - 4] = "Wall";
                tiles[tiles.GetLength(0) - 4, y] = "Wall";
            }
        }

        print("Map has " + w + " width and " + h + " height and " + x + " non-null tiles out of " + (w*h) + " tiles (" + ((float)x/(w*h)*100.0f) + "%)");
        
        Display();
    }



    public void DisplayCustom(String[,] map){
        Ground.FloodFill(new Vector3Int(-width/2, -height/2, 0), sprites[0]);
        
        BoundsInt area = new BoundsInt(
            new Vector3Int(-width/2, -height/2, 0), 
            new Vector3Int(width/2, height/2, 0));
        
        TileBase[] wallset = new TileBase[width * height];

        for(int x = 0; x < map.GetLength(0); x += 1){
            for(int y = 0; y < map.GetLength(1); y += 1){
                String specifier, referent;
                try{
                    referent = map[x,y];
                    mapping.TryGetValue(referent, out specifier);
                }catch(Exception e){
                    String error = "[5]: Failed to get mapping for word at (" + x + ", " + y + ")"+
                    "\nValue of tile array was: '" + tiles[x,y] + "'";
                    print(error + "\n" + e); 
                    specifier = null;
                }
                if(specifier == "Walls"){
                    wallset[Math.Min(width, height) * x + y] = sprites[0];
                }
            }
        }

        Walls.SetTilesBlock(area, wallset);
    }



    public void Display(){
        Ground.FloodFill(new Vector3Int(-width/2, -height/2, 0), sprites[0]);
        
        BoundsInt area = new BoundsInt(
            new Vector3Int(-width/2, -height/2, 0), 
            new Vector3Int(width/2, height/2, 0));
        
        TileBase[] wallset = new TileBase[width * height];

        for(int x = 0; x < tiles.GetLength(0); x += 1){
            for(int y = 0; y < tiles.GetLength(1); y += 1){
                String specifier, referent;
                try{
                    referent = tiles[x,y];
                    mapping.TryGetValue(referent, out specifier);
                }catch(Exception e){
                    String error = "[5]: Failed to get mapping for word at (" + x + ", " + y + ")"+
                    "\nValue of tile array was: '" + tiles[x,y] + "'";
                    print(error + "\n" + e); 
                    specifier = null;
                }
                if(specifier == "Walls"){
                    wallset[Math.Min(width, height) * x + y] = sprites[0];
                }
            }
        }
        Walls.SetTilesBlock(area, wallset);
    }


    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }

    /**
    A method to read the ruleset of the grammar from a plaintext file
    @Preconditions: The path to the rules file is valid, and the file is organized as follows:
        First line: two numbers defining the dimensions of any production rule seperated by a space. 
        E.G. 3 3 says any production rule will be 3x3
        Second line: a set of words where the first is the null placeholder, the second is the starter placeholder, and the last is the terminal (unused)
        Third and Fourth lines: Listing which referents are walls and floors. These are handed in as comma seperated lists of the format
            walls: wall_ref1, wall_ref2, etc
            floors: floor_ref1, floor_ref2, etc
            such that when they are read in, only the items after the colon are in the list.
        All following lines: words to production rule sets. Words are seperated from their production rules by a colon (:), production rules are seperated by commas (,)
        and lines of a production rule are seperated by spaces. The production rules must fit the dimensions defined in the First line
        E.G. (with the above first line) a: x x x  x x x  x x x, y y y  y y y  y y y
    @Postconditions: the alphabet set is filled, and the words dicitonary is filled, both in accordance with the above definitions.
    **/
    private void ReadFile(){
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
        r_width = size_x;
        r_height = size_y;
        



        // obtain the alphabet
        String[] temp_alphabet = input[1].Split(new String[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        alphabet = new String[temp_alphabet.Length]; // obtain the alphabet
        Array.Copy(temp_alphabet, alphabet, temp_alphabet.Length);





        // prepare and obtain the mappings for walls and floors
        String[] walls = input[2].Split(new String[] {":"}, StringSplitOptions.RemoveEmptyEntries)[1].Split(new String[]{","}, StringSplitOptions.RemoveEmptyEntries);
        String[] floors = input[3].Split(new String[] {":"}, StringSplitOptions.RemoveEmptyEntries)[1].Split(new String[]{","}, StringSplitOptions.RemoveEmptyEntries);
        // Add the key and value for walls
        foreach(String s in walls){
            mapping.Add(s.Trim(), "Walls");
        }
        // Add the key and value for floors
        foreach (String s in floors){
            mapping.Add(s.Trim(), "Floors");
        }



        for(int i = 4; i < input.Length; i += 1){

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



    public void GenerateNewMap(){
        Array.Clear(tiles, 0, tiles.Length);
        map_generator.Clear();
        map_generator.GenerateMap(new ThreadArgs(alphabet, words, rand));
        tiles = map_generator.getMap();
    }




    public void Regenerate(){
        Array.Clear(tiles, 0, tiles.Length);
        map_generator.Clear();
        if(seed != "" & seed != null){
            rand = new System.Random(seed.GetHashCode());
        }
        else{
            int val = rand.Next();
            rand = new System.Random(val);
        }
        map_generator.GenerateMap(new ThreadArgs(alphabet, words, rand));
        tiles = map_generator.getMap();
    }




    public void ChangeSeedIndirect(){
        InputField seeder = GameObject.Find("Seeder").GetComponent(typeof(InputField)) as InputField;
        string input_value = seeder.text;
        this.seed = input_value;
        rand = new System.Random(seed.GetHashCode());
    }




    public void ChangeSeedDirect(String seed){
        rand = new System.Random(seed.GetHashCode());
    }




    public String[,] getStringMap(){
        return tiles;
    }
}




/** A placeholder class for storing a tile and its coordinates**/
public class Subtile {

    private string value;
    private int x, y;

    public Subtile(String value, int x, int y) {
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
    private Queue<Subtile> frontier;
    private int min_width, min_height, min_area;
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
    public Tree(int left, int right, int upper, int lower, int threshold, bool vertical, int min_w, int min_h){
        
        left_bound = left;
        right_bound = right;
        upper_bound = upper;
        lower_bound = lower;

        width = right - left;
        height = lower - upper;
        
        try{
        	map = new String[width, height];
        }
        catch (Exception e){
        	Debug.Log("Failed to create map string for tree\n" +
        	e +
        	"\nBounds: (" + left_bound + ", " + upper_bound + "), (" + right_bound + ", " + lower_bound + ")" +
        	"\nWidth: " + width + "\tHeight: " + height);
        }

        min_width = min_w;
        min_height = min_h;
        min_area = threshold;
        
        this.vertical = vertical;

        frontier = new Queue<Subtile>();
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
        return (width * height) <= min_area & width > min_width & height > min_height;
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
        if(!isLeaf() & min_width < width - min_width & min_height < height - min_height){
            children = new Tree[2];
            int division;
            if(vertical){
            
                division = left_bound + arguments.rand.Next(min_width, width - min_width);
                if(division < left_bound + min_width){
                    division = left_bound + min_width;
                }
                else if(division > right_bound - min_width){
                	division = right_bound - min_width;
                }



                children[0] = new Tree(left_bound, division-1, upper_bound, lower_bound, min_area, false, min_width, min_height);
                children[1] = new Tree(division, right_bound, upper_bound, lower_bound, min_area, false, min_width, min_height);

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

                String val;

                for(int i = 0; i < c_map_1.GetLength(0); i += 1){
                    for(int j = 0; j < c_map_1.GetLength(1); j +=1){
                        
                        try{
                            if(c_map_1[i,j] == null){
                                val = arguments.alphabet[0];
                            }
                            else{
                                val = c_map_1[i,j];
                            }
                        }
                        catch (Exception e){
                            Debug.Log("[1.1] Failed to get element from minor array at index (" + i + ", " + j + ")\n" + e);
                            val = arguments.alphabet[0];
                        }

                        try{
                        	map[i,j] = val;
                    	}
                    	catch (Exception E){
                    		Debug.Log("[1.2] Failed to add element '" + val + "' to array\n" + E + 
                    			"\nIndex: (" + i +", " + j +")" + 
                    			"\nMajor Array goes from (" + left_bound + ", " + upper_bound +") to (" + right_bound + ", " + lower_bound +")" +
                    			" With width " + width + " and height " + height +
                    			"\nMinor array has size (" + c_map_1.GetLength(0) + ", " + c_map_1.GetLength(1) + ")" + 
                    			"\nDivision Point: " + (division + left_bound));
                    	}
                    }
                }
                
                int offset = c_map_1.GetLength(0);
                
                for(int i = 0; i < c_map_2.GetLength(0); i += 1){
                    for(int j = 0; j < c_map_2.GetLength(1); j +=1){

                        try{
                            if(c_map_2[i,j] == null){
                                val = arguments.alphabet[0];
                            }
                            else{
                                val = c_map_2[i,j];
                            }
                        }
                        catch (Exception e){
                            Debug.Log("[2.1] Failed to get element from minor array at index (" + i + ", " + j + ")\n" + e);
                            val = arguments.alphabet[0];
                        }


                        try{
                        	map[i + offset, j] = val;
                    	}
                    	catch (Exception E){
                    		Debug.Log("[2.2]: Failed to add element '"+ val +"' to array\n" + 
                    			E + 
                    		"\nIndex in Minor Array: (" + i +", " + j +")" +
                    		"\nIndex in Major Array: (" + (i + division) + ", " + j + ")" +
                    		"\nMajor Array goes from (" + left_bound + ", " + upper_bound +") to (" + right_bound + ", " + lower_bound+")" +
                    		" With width " + width + " and height " + height +
                    		"\nMinor array has size (" + c_map_2.GetLength(0) + ", " + c_map_2.GetLength(1) + ")" + 
                    		"\nDivision Point: " + division);
                    	}

                    }
                }
            }
            else{
                division = upper_bound + (arguments.rand.Next(min_height, height - min_height));
                if(division < upper_bound + min_height ){
                    division = upper_bound + min_height;
                }
                else if(division > lower_bound - min_height){
                	division = lower_bound - min_height;
                }
            
                children[0] = new Tree(left_bound, right_bound, upper_bound, division-1, min_area, true, min_width, min_height);
                children[1] = new Tree(left_bound, right_bound, division, lower_bound, min_area, true, min_width, min_height);
            
                Thread child1 = new Thread(new ParameterizedThreadStart(children[0].GenerateMap));
                Thread child2 = new Thread(new ParameterizedThreadStart(children[1].GenerateMap));

                child1.Start(arguments);
                child2.Start(arguments);

                child1.Join();
                child2.Join();

                String[,] c_map_1 = children[0].getMap();
                String[,] c_map_2 = children[1].getMap();

                String val;

                for(int i = 0; i < c_map_1.GetLength(0); i += 1){
                    for(int j = 0; j < c_map_1.GetLength(1); j +=1){


                        try{
                            if(c_map_1[i,j] == null){
                                val = arguments.alphabet[0];
                            }
                            else{
                                val = c_map_1[i,j];
                            }
                        }
                        catch (Exception e){
                            Debug.Log("[3.1] Failed to get element from minor array at index (" + i + ", " + j + ")\n" + e);
                            val = arguments.alphabet[0];
                        }

                    	try{
                        	map[i,j] = val;
                    	}
                    	catch (Exception E){
                    		Debug.Log("[3.2] Failed to add element '" + val +"' to array\n" + E + 
                    			"\nIndex array: (" + i +", " + j +")" + 
                    			"\nMajor Array goes from (" + left_bound + ", " + upper_bound +") to (" + right_bound + ", " + lower_bound +")" +
                    			" With width " + width + " and height " + height +
                    			"\nMinor array has size (" + c_map_1.GetLength(0) + ", " + c_map_1.GetLength(1) + ")" + 
                    			"\nDivision Point: " + (division + upper_bound));
                    	}
                    }
                }
            

                int offset = c_map_1.GetLength(1) - 1;
                for(int i = 0; i < c_map_2.GetLength(0); i += 1){
                    for(int j = 0; j < c_map_2.GetLength(1); j +=1){


                        try{
                            if(c_map_2[i,j] == null){
                                val = arguments.alphabet[0];
                            }
                            else{
                                val = c_map_2[i,j];
                            }

                        }
                        catch (Exception e){
                            Debug.Log("[4.1] Failed to get element from minor array at index (" + i + ", " + j + ")\n" + e);
                            val = arguments.alphabet[0];
                        }

                    	try{
                        	map[i,j + offset] = val;
                    	}
                    	catch (Exception E){
                    		Debug.Log("[4.2] Failed to add element '" + val + "' to array\n" + 
                    			E + 
                    		"\nIndex in Minor Array: (" + i +", " + j +")" +
                    		"\nIndex in Major Array: (" + i + ", " + (j + division) + ")" +
                    		"\nMajor Array goes from (" + left_bound + ", " + upper_bound +") to (" + right_bound + ", " + lower_bound+")" +
                    		" With Major width " + width + " and height " + height +
                    		"\nMinor array has size (" + c_map_2.GetLength(0) + ", " + c_map_2.GetLength(1) + ")" + 
                    		"\nDivision Point: " + division);
                    	}
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
        int x, y;
        try{
        	x = rand.Next() % width;
        }
        catch (Exception e){
        	Debug.Log("Failed to create random x starting location\n" +
        	e +
        	"\nWidth: " + width);
        	x = width / 2;
        }
	
		try{
			y = rand.Next() % height;
        }
        catch (Exception e){
        	Debug.Log("Failed to create random y starting location\n" + 
        	e +
        	"\nHeight: " + height);
        	y = height / 2;
        }
        
        map[x, y] = alphabet[1]; // place a starting tile at the center of the board
        frontier.Enqueue(new Subtile(alphabet[1], x, y)); // enqueue the starting tile for use with grow
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
            Subtile current = frontier.Dequeue(); // obtain the oldest tile
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
    void Swap(Subtile tile, string[] alphabet, 
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
                
                if (i >= 0 & j >= 0 &
                    i < width & j < height) {
                    string newtile = set[possibility_x, possibility_y]; // obtain the elements of the selected production rule 1 by 1

                    if (map[i, j] == null | map[i, j] == alphabet[1] | map[i,j] == alphabet[0]){ // existing is null or starting tile
                        
                        if (newtile != alphabet[0] & newtile != alphabet[alphabet.Length - 1] & newtile != null) { // non-null non-starting tile replacement
                            map[i, j] = newtile;
                            
                            if (i != y | j != x) {
                                frontier.Enqueue(new Subtile(newtile, i, j)); // enqueue the tile for future growth
                            }
                        }   
                    }
                }
            }
        }
    }
}