# Unity week 7: Two-dimensional scene-building and path-finding

Improvements from the original code:

1) We decided to implement the path-finding algorithm - A* (A-star).

2) We made "slow-tile", tiles which slow the player advancement and included it in our a* algorithm.

3) We created a map-generator that accepts an array of tiles and generate a map using those tiles. The current optimal map outcomes is using 3 tiles, just like in the game in this browser.

4) You can mine the rocks using the x + arrow buttons, you have to stand next to the rock and use the arrow that points to it!

*  After generating a random map you might get stuck inside a rock, you can just carve your way out!

Have fun!


## תקציר הרכיבים

על מנת לממש את האלגוריתם A* היה עלינו לממש 3 מחלקות עזר ועוד רכיב שמשתמש בהן. סה"כ 4 סקריפטים, נפרט כעת על הסקריפטים ושימושם:

1) תור עדיפות - MyPriorityQueue - מחלקה גנרית שממשת תור עדיפות, המחלקה מקבלת רק אובייקטים המממשים את הממשק IComparable כלומר רק אובייקטים הניתנים להשוואה.
2) נוד A* - NodeAstar - מחלקה של Nodes, כל אובייקט במחלקה מכיל מיקום ומשקל, המחלקה מממשת את הממשק IComparable על מנת שנוכל להכניס את הנודים לתור עדיפות.
3) מחלקת Astar - אלגוריתם ה-Path-Finding ממומש במחלקה זו, שאר הסקריפטים אשר רוצים להשתמש במחלקה קוראים לה חיצונית וספקים לה את הפרמטרים הרלוונטיים. 
4) סקריפט מנייד - TargetMoverAstar - סקריפט זה משתמש באלגוריתם A* על ידי קריאה למחלקת Astar וסיפוק פרמטרים מתאימים.

## פירוט הרכיבים

### תור עדיפות - MyPriorityQueue

במחלקה זו, נעשה שימוש ברשימה (List) על מנת לממש את הרעיון האלגוריתמי.
למעשה המימוש הוא די פשוט, אנו פשוט נותנים גישה רק לאלמנט הראשון של הרשימה ולא מאפשרים הכנסה אקראית אלא, עוטפים את הרשימה בפונקציית Enqueue אשר
מכניסה את האובייקט בצורה ממויינת לפי פונקציית ה-CompareTo אשר האובייקט חייב לממש מכיוון שהוא ממש את הממשק IComparable.

להלן הפונקציה Enqueue

```
    public void Enqueue(T item)
    {
        if (queue.Count == 0)
        {
            queue.Add(item);
        }
        else
        {
            if (queue[queue.Count - 1].CompareTo(item) <= 0)
            {
                queue.Add(item);
                return;
            }
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].CompareTo(item) >= 0)
                {
                    queue.Insert(i, item);
                    return;
                }
            }
        }
    }

```

למעשה בכל הכנסה של אובייקט כלשהו לרשימה אנו דואגים שהיא תישאר ממויינת בסדר עולה. על ידי שימוש בפוקנציה CompareTo אשר קובעת כי אם הערך המוחזר גדול מ-0 אז האיבר שקרא לה אמור להיות אחרי האיבר אשר הוא משווה מולו, במידה והוא קטן מ-0 אז הפוך ואם שווה ל-0 אז הם לכאורה שווים מבחינת סדר.

שאר הפונקציות הינן פשוטות:

```
    public void Deque()
    {
        queue.RemoveAt(0);
    }

    public T Peek()
    {
        return queue[0];
    }
        public bool isEmpty()
    {
        return queue.Count == 0;
    }

```

ניתן לראות כי המשתמש לא יכול לגשת לרשימה בצורה ישירה אלא דרך שיטות העוטפות אותה ובכך דואגות שלא יוכל לשבש את הסדר של הרשימה.

### נוד NodeAstar

מחלקת עזר למימוש אלגוריתם A* במחלקה זו לאובייקטים יש משקל, משקל משני ומיקום (Vector3Int).

מבנה המחלקה:
```
public class AstarNode : IComparable
{
    Vector3Int location;
    AstarNode father;
    int weight;
    int steps;
}
```

steps הוא משקל משני.

בנוסף ניתן לראות כי הפונקציה מממשת את הממשק IComparable, להלן הפונקציה אשר היא מחוייבת לממש -CompareTo:

```
    public int CompareTo(object obj)
    {
        if (!(obj is AstarNode))
        {
            throw new ArgumentException("Compared Object is not of car");
        }
        AstarNode node = obj as AstarNode;
        return weight.CompareTo(node.weight);
    }

```

למעשה מתבצעת בדיקה כי האובייקט שהתקבל הוא אכן מאותו סוג וניתן להשוואה ואז ההשוואה מתבצעת לפי המשתנה weight אשר מסוג int.
כל שאר השיטות במחלקה הן שיטות get ו-set פשוטות.


### מחלקת Astar

במחלקה זו מומשה הפונקציה למציאת יעד (Path-Finding) A*. 
המחלקה נעזרת ב-2 המחלקות לעיל על מנת לממש את האלגוריתם, הייצוג של הגרף נעשה על ידי קודקודים מסוג NodeAstar והמיון שלהם נעשה על ידי
התור עדיפיות הגנרי MyPriorityQueue. בנוסף, המחלקה נעזרת ממידע חיצוני שהיא מקבלת מהסקריפטים שקוראים לה.

משתני המחלקה:
```
public class Astar
{
    Vector3Int[] neighbors = { new Vector3Int(0, 1, 0),
    new Vector3Int(0, -1, 0),new Vector3Int(1, 0, 0),
    new Vector3Int(-1, 0, 0),};
    AstarNode src, dst;
    AllowedTiles allowedTiles; 
    MyPriorityQueue<AstarNode> queue;
    Tilemap tilemap;
    int maxIterations, currIter;
    HashSet<Vector3Int> openSet;
    TileBase tilebase;
}    
```
מערך הוקטורים מייצג את השכנים האפשריים של כל קודקוד, הקודקודים src ו-dst הינם קודקוד היעד והההתחלה.
ההאשסט שמכיל מיקומים מוודא כי לא מכניסים את אותו קודקוד פעמיים לתור עדיפות ומונע כפילויות.
משתני האיטרציות מגדירים באיזו איטרציה אנחנו כעת ומהו הגבול לכמות האיטרציות. 
שאר המשתנים אחראיים להמרה של מיקום בעולם למיקום על המפה.

```
    public List<Vector3Int> GetPath()
    {
        List<Vector3Int> path = new List<Vector3Int>();
        if (src.getLocation().Equals(dst.getLocation()))
        {
            path.Add(src.getLocation());
            return path;
        }
        queue.Enqueue(src);
        openSet.Add(src.getLocation());
        while (!queue.isEmpty() && currIter < maxIterations)
        {
            currIter++;
            AstarNode temp = queue.Peek();
            queue.Deque();
            foreach (var neighbor in neighbors)
            {
                Vector3Int currLocation = neighbor + temp.getLocation();
                if (!openSet.Contains(currLocation) && isContained(allowedTiles.Get(), currLocation))
                {
                    AstarNode neighborNode = new AstarNode(currLocation);
                    neighborNode.setSteps(temp.getSteps() + 1);
                    neighborNode.setWeight((neighborNode.getSteps() + calculateF(neighborNode, dst)));
                    if (isContained(allowedTiles.getSlow(), neighborNode.getLocation()))
                    {
                        neighborNode.setWeight((neighborNode.getWeight() + calculateF(neighborNode, dst)));
                    }
                    openSet.Add(neighborNode.getLocation());
                    queue.Enqueue(neighborNode);
                    neighborNode.setFather(temp);
/*                    tilemap.SetTile(neighborNode.getLocation(), tilebase);
*/                    if (neighborNode.getLocation().Equals(dst.getLocation()))
                    {
                        dst = neighborNode;
                        currIter = maxIterations;
                        break;
                    }
                }
            }
        }
        AstarNode tempReverse = dst;
        if (dst.getFather() == null)
        {
            path.Add(src.getLocation());
            return path;
        } 
        while (tempReverse.getLocation() != src.getLocation())
        {
            path.Add(tempReverse.getLocation());
            tempReverse = tempReverse.getFather();
        }
        path.Add(tempReverse.getLocation());
        path.Reverse();
        return path;
    }
```


בפונקציה זו נעשה שימוש בתור עדיפות, תחילה אנו ממירים את המיקום של קודקוד ההתחלה ל-NodeAstar ולאחר מכן מכניס אותו לתור.
בתור שלב שני אנו נכנסים ללואה אשר נעצרת רק במידה ועברנו את גבול האיטרציות או שהגענו לקודקוד היעד.
למעשה אנו כל הזמן עוברים על השכנים של הקודקוד עם המשקל הכי נמוך, ונותנים לשכנים של הקודקוד הנ"ל את המשקל שלו ועוד המרחק של השכנים מקודקוד היעד,
זהו למעשה המשמעות של אלגוריתם זה, אלגוריתם A* משתמש גם במשקל של הקודקוד וגם במרחק שלו מהיעד ועושה מן שקלול לסכום של שניהם ובכך קובע את המשקל של הקודקוד.
לבסוף אנו יוצאים מהלולאה, מסדרים את הקודקודים במערך ומחזירים את הרשימה לפי סדר הקודקודים.

זוהי הפונקציה לחישוב המרחק בין קודקוד היעד לקודקוד הרלוונטי:
```
    public int calculateF(AstarNode a, AstarNode b)
    {
        Vector3Int dist = a.getLocation() - b.getLocation();
        return (System.Math.Abs(dist.x) + System.Math.Abs(dist.y));
    }
```
למעשה, מכיוון שאנו בעולם של אריחים, על מנת להגיע מנקודה אחת לאחרת, מרחק הצעדים תמיד יהיה סכום של ערכי ה-X וה-Y בערך מוחלט של הפרש המיקומים של הקודקודים.

בנוסף, על מנת לקבוע אריחים (Tiles) אשר בעלי משקל יותר כבד, בדקנו האם כל אריח מוגדר -"אריח איטי" ונתנו לו אקסטרה משקל בהתאם לגורם ההאטה שלו!
```
    public bool isContained(TileBase[] tilebase,Vector3Int pos)
    {
        foreach(var tile in tilebase)
        {
            TileBase posTile = tilemap.GetTile(pos);
            if (posTile.Equals(tile)) return true;
        }
        return false;
    }
```



### TargetMoverAstar

במחלקה זו נעשה קראנו למחלקת Astar על מנת למצוא את הדרך הקצרה ביותר של השחקן אל היעד ומציאת הצעד הבא של השחקן.

משתני המחלקה:
```
public class TargetMoverAstar : MonoBehaviour
{
    [SerializeField] Tilemap tilemap = null;
    [Tooltip("The speed by which the object moves towards the target, in meters (=grid units) per second")]
    [SerializeField] float speed = 2f;
    [Tooltip("Maximum number of iterations before BFS algorithm gives up on finding a path")]
    [SerializeField] int maxIterations = 1000;
    [Tooltip("The target position in world coordinates")]
    [SerializeField] Vector3 targetInWorld;
    [Tooltip("The target position in grid coordinates")]
    [SerializeField] Vector3Int targetInGrid;
    [SerializeField] TileBase tilebase;
    [SerializeField] AllowedTiles allowedTiles;
    protected bool atTarget;  // This property is set to "true" whenever the object has already found the target.
    private float timeBetweenSteps=0.1f;
}    
```
כלל הרכיבים אשר אנו צריכים לקבל על מנת לבצע את אלגוריתם Astar נמצאים במחלקה זו.

קריאת המחלקה לפונקציית GetPath של מחלקת A*:
```
    private void MakeOneStepTowardsTheTarget()
    {
        Vector3Int startNode = tilemap.WorldToCell(transform.position);
        Vector3Int endNode = targetInGrid;
        Astar astarObject = new Astar(startNode, endNode, tilemap, maxIterations, tilebase,allowedTiles);
        List<Vector3Int> shortestPath = astarObject.GetPath();
        Debug.Log("shortestPath = " + string.Join(" , ", shortestPath));
        if (shortestPath.Count >= 2)
        {
            Vector3Int nextNode = shortestPath[1];
            transform.position = tilemap.GetCellCenterWorld(nextNode);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else
        {
            atTarget = true;
        }
    }
```

כמעט זהה למחלקת MoveTarget המקורית אך עם שינוי לפונקציה אשר אליה הוא קורא והארגומנטים אותם הוא מעביר.
