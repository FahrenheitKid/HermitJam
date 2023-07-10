using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    private IEqualityComparer<T> comparer;

    public SequenceComparer(IEqualityComparer<T> comparer = null)
    {
        this.comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
    {
        return x.SequenceEqual(y, comparer);
    }

    public int GetHashCode(IEnumerable<T> sequence)
    {
        unchecked
        {
            int hash = 19;
            foreach (var item in sequence)
                hash = hash * 79 + comparer.GetHashCode(item);
            return hash;
        }
    }
}

public class StringWithNumbersComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        var regex = new System.Text.RegularExpressions.Regex("(\\d+)");

        // run the regex on both strings
        var xRegexResult = regex.Match(x);
        var yRegexResult = regex.Match(y);

        // check if they are both numbers
        if (xRegexResult.Success && yRegexResult.Success)
        {
            return int.Parse(xRegexResult.Groups[1].Value).CompareTo(int.Parse(yRegexResult.Groups[1].Value));
        }

        // otherwise return as string comparison
        return x.CompareTo(y);
    }
}

public class GameObjectNamesComparer : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {

        StringWithNumbersComparer comparer = new StringWithNumbersComparer();
        return comparer.Compare(x.name, y.name);
    }
}

public class ListEqualityComparer<T> : IEqualityComparer<List<T>>
{
    private readonly IEqualityComparer<T> _itemEqualityComparer;

    public ListEqualityComparer() : this(null) { }

    public ListEqualityComparer(IEqualityComparer<T> itemEqualityComparer)
    {
        _itemEqualityComparer = itemEqualityComparer ?? EqualityComparer<T>.Default;
    }

    public static readonly ListEqualityComparer<T> Default = new ListEqualityComparer<T>();

    public bool Equals(List<T> x, List<T> y)
    {
        if (ReferenceEquals(x, y))return true;
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))return false;
        return x.Count == y.Count && !x.Except(y, _itemEqualityComparer).Any();
    }

    public int GetHashCode(List<T> list)
    {
        int hash = 17;
        foreach (var itemHash in list.Select(x => _itemEqualityComparer.GetHashCode(x))
                .OrderBy(h => h))
        {
            hash += 31 * itemHash;
        }
        return hash;
    }
}

public static class CollectionsExtensions
{
    public static List<int> FindAllIndex<T>(this List<T> container, System.Predicate<T> match)
    {
        var items = container.FindAll(match);
        List<int> indexes = new List<int>();
        foreach (var item in items)
        {
            indexes.Add(container.IndexOf(item));
        }

        return indexes;
    }

    public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
    {
        return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
    }

    public static List<T> Repeated<T>(this List<T> list, T value, int count)
    {
        List<T> ret = new List<T>(count);
        ret.AddRange(Enumerable.Repeat(value, count));
        return ret;
    }

}
//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class ExtensionMethods
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void AddUniqueItem<T>(this List<T> list, T item, bool throwException = false)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
        else
        {
            if (throwException)
            {
                throw new System.InvalidOperationException("Item already exists in the list");
            }

            //Debug.LogError("Item already exists in the list");
        }
        
    }
    public static bool IsUnique<T>(this List<T> list, IEqualityComparer<T> comparer)
    {
        return list.Count == list.Distinct(comparer).Count();
    }
    public static bool IsUnique<T>(this List<T> list)
    {
        return list.Count == list.Distinct().Count();
    }

}

static public class UtilityTools
{
    public enum Directions
    {
        up,
        upRight,
        right,
        downRight,
        down,
        downLeft,
        left,
        upLeft
    }

    public static Directions[] allDirections = {
        Directions.up,
        Directions.upRight,
        Directions.right,
        Directions.downRight,
        Directions.down,
        Directions.downLeft,
        Directions.left,
        Directions.upLeft
    };

    public static Directions[] axials = {
        Directions.up,
        Directions.right,
        Directions.down,
        Directions.left
    };

    public static Directions[] horizontals = {
        Directions.right,
        Directions.left
    };

    public static Directions[] verticals = {
        Directions.up,
        Directions.down
    };

    public static Directions[] diagonals = {

        Directions.upLeft,
        Directions.upRight,
        Directions.downRight,
        Directions.downLeft
    };

    public static Directions[] rights = {
        
        Directions.upRight,
        Directions.right,
        Directions.downRight
    };

    public static Directions[] lefts = {

        Directions.downLeft,
        Directions.left,
        Directions.upLeft
    };

    public static Directions[] ups = {

        Directions.upLeft,
        Directions.up,
        Directions.upRight
    };

    public static Directions[] downs = {

        Directions.downRight,
        Directions.down,
        Directions.downLeft
    };

    public static Directions GetNextDirection(UtilityTools.Directions startDirection, bool clockwise = true)
    {
        int index = 0;
        bool diagonals = false;
        if (axials.Contains(startDirection))
        {
            index = System.Array.FindIndex(axials, x => x == startDirection);
            if (clockwise)
            {
                if (index == axials.Length - 1)index = 0;
                else index++;
            }
            else
            {
                if (index <= 0)index = axials.Length - 1;
                else index--;
            }

        }
        else if (UtilityTools.diagonals.Contains(startDirection))
        {
            diagonals = true;
            index = System.Array.FindIndex(UtilityTools.diagonals, x => x == startDirection);
            if (clockwise)
            {
                if (index == axials.Length - 1)index = 0;
                else index++;
            }
            else
            {
                if (index <= 0)index = axials.Length - 1;
                else index--;
            }

        }

        if (!diagonals)return axials[Mathf.Clamp(index, 0, axials.Length - 1)];
        else return UtilityTools.diagonals[Mathf.Clamp(index, 0, UtilityTools.diagonals.Length - 1)];

    }

    public static Directions GetNextHalfDirection(UtilityTools.Directions startDirection, bool clockwise = true)
    {
        int index = 0;
        if (allDirections.Contains(startDirection))
        {
            index = System.Array.FindIndex(allDirections, x => x == startDirection);
            if (clockwise)
            {
                if (index == allDirections.Length - 1) index = 0;
                else index++;
            }
            else
            {
                if (index <= 0) index = allDirections.Length - 1;
                else index--;
            }

        }

        return UtilityTools.allDirections[Mathf.Clamp(index, 0, UtilityTools.allDirections.Length - 1)];
    }

    public static bool IsOpposite(Directions direction1, Directions direction2, bool horizontal)
    {

        if (horizontal)
        {
            if (UtilityTools.verticals.Any(x => x == direction1 || x == direction2))return false;

            bool isLeft1 = false;
            bool isLeft2 = false;

            isLeft1 = (direction1 == Directions.left) || (direction1 == Directions.upLeft) || (direction1 == Directions.downLeft);
            isLeft2 = (direction2 == Directions.left) || (direction2 == Directions.upLeft) || (direction2 == Directions.downLeft);

            return isLeft1 != isLeft2;
        }
        else
        {
            if (UtilityTools.horizontals.Any(x => x == direction1 || x == direction2))return false;

            bool isUp1 = false;
            bool isUp2 = false;

            isUp1 = (direction1 == Directions.up) || (direction1 == Directions.upLeft) || (direction1 == Directions.upRight);
            isUp2 = (direction2 == Directions.up) || (direction2 == Directions.upLeft) || (direction2 == Directions.upRight);

            return isUp1 != isUp2;
        }
    }

    public static Directions OppositeDirection(Directions dir, bool diagonoalsXYInverted = true)
    {
        switch (dir)
        {
            case UtilityTools.Directions.up:
                return Directions.down;
            case UtilityTools.Directions.upRight:
                return (diagonoalsXYInverted) ? Directions.downLeft : Directions.downRight;
            case UtilityTools.Directions.right:
                return Directions.left;
            case UtilityTools.Directions.downRight:
                return (diagonoalsXYInverted) ? Directions.upLeft : Directions.upRight;
            case UtilityTools.Directions.down:
                return Directions.up;
            case UtilityTools.Directions.downLeft:
                return (diagonoalsXYInverted) ? Directions.upRight : Directions.upLeft;
            case UtilityTools.Directions.left:
                return Directions.right;
            case UtilityTools.Directions.upLeft:
                return (diagonoalsXYInverted) ? Directions.downRight : Directions.downLeft;
            default:
                return Directions.right;
        }
    }

    public static Vector3 getDirectionVector(UtilityTools.Directions dir)
    {
        switch (dir)
        {
            case UtilityTools.Directions.up:
                return Vector3.up;
            case UtilityTools.Directions.upRight:
                return (Vector3.up + Vector3.right).normalized;
            case UtilityTools.Directions.right:
                return Vector3.right;
            case UtilityTools.Directions.downRight:
                return (Vector3.down + Vector3.right).normalized;
            case UtilityTools.Directions.down:
                return Vector3.down;
            case UtilityTools.Directions.downLeft:
                return (Vector3.down + Vector3.left).normalized;
            case UtilityTools.Directions.left:
                return Vector3.left;
            case UtilityTools.Directions.upLeft:
                return (Vector3.up + Vector3.left).normalized;
            default:
                return Vector3.zero;
        }
    }

    public static int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0)return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f)continue;

            s += w / t;
            if (s >= r)return i;
        }

        return -1;
    }

    public static bool isPointInViewport(Vector3 screenPoint) // returns true if point is inside viewport
    {
        if (screenPoint.z > 0f && screenPoint.x > 0f && screenPoint.x < 1f && screenPoint.y > 0f && screenPoint.y < 1f)
        {
            return true;
        }
        else
            return false;
    }

    public static bool isMousePointInsideRectTransform(RectTransform rect, Vector2 mousePoint)
    {
        if (!rect)return false;
        Vector2 localMousePosition = rect.InverseTransformPoint(mousePoint);
        return rect.rect.Contains(localMousePosition);
    }

    public static bool isPointInViewport(Vector3[] points) // returns true if all points are visible
    {
        bool result = true;
        foreach (Vector3 p in points)
        {
            if (!(p.z > 0f && p.x > 0f && p.x < 1f && p.y > 0f && p.y < 1f))
            {
                result = false;
            }
        }

        return result;
    }

    public static List<T> TwoDimensionalToList<T>(T[, ] source)
    {
        if (source == null)return null;
        List<T> list = new List<T>(source.GetLength(0) * source.GetLength(1));
        int size1 = source.GetLength(1);
        int size0 = source.GetLength(0);
        for (int i = 0; i < size0; i++)
        {
            for (int j = 0; j < size1; j++)
                list.Add(source[i, j]);
        }

        return list;
    }

    public static bool AreLowerEqual(string a, string b)
    {
        return a?.ToLower() == b?.ToLower();
    }

    public static List<T> FindComponentsWithTag<T>(string tag)
    {
        List<T> result = new List<T>();

        if (GameObject.FindGameObjectsWithTag(tag).Length == 0)return result;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
            T temp = go.GetComponent<T>();
            if (temp != null)
                result.Add(temp);
        }

        return result;
    }

    public static void updateTextWithPunch(TMPro.TextMeshProUGUI textUI, string newText, Vector3 punch, float animTime, bool relative = false)
    {
        textUI.text = newText;
        textUI.transform.DOPunchScale(punch, animTime).SetRelative(relative);
    }

    public static bool IsSameOrSubclass(System.Type potentialBase, System.Type potentialDescendant)
    {
        return potentialDescendant.IsSubclassOf(potentialBase) ||
            potentialDescendant == potentialBase;
    }

    public static System.Tuple<int, int> indexOf<T>(this T[, ] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (matrix[x, y].Equals(value))
                    return System.Tuple.Create(x, y);
            }
        }

        return System.Tuple.Create(-1, -1);
    }

    public static string ReplaceCharAtIndex(int i, char value, string word)
    {
        if (i < 0 || i > word.Length)return "";
        char[] letters = word.ToCharArray();
        letters[i] = value;
        return string.Join("", letters);
    }

    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }

    static public double linear(double x, double x0, double x1, double y0, double y1)
    {
        if ((x1 - x0) == 0)
        {
            return (y0 + y1) / 2;
        }
        return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
    }

    public static bool OnEndEditFocusCheck(string inputText = "")
    {
        bool pass = false;
        if (TouchScreenKeyboard.isSupported)
        {
            TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(inputText);
            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {

                pass = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
        {
            pass = true;
        }

        return pass;
    }

    // returns the index given the row and colum
    public static int GetLinearIndexFrom2D(int x, int y, int width, int height, bool leftToRight = true, bool topToBottom = true, bool verticalScan = false)
    {
        if (x >= width || y >= height || x < 0 || y < 0 || width < 0 || height < 0)return -1;

        int outer = height;
        int inner = width;
        if (verticalScan == true)
        {
            outer = width;
            inner = height;
        }

        if (verticalScan == false)
        {
            if (topToBottom) // i == y
            {
                if (leftToRight)
                {
                    return x + y * height;
                }
                else
                {
                    return width - 1 - x + y * height;
                    //10,0 == 0
                    //9,0 == 1
                    //10,1 == 11
                }
            }
            else
            {
                if (leftToRight)
                {
                    return x + (height - 1 - y) * height;
                }
                else
                {
                    return width - 1 - x + (height - 1 - y) * height;
                    //10,0 == 0
                }
            }
        }

        return -1;

    }

/*
    public static UtilityTools.Directions? GetDirectionToPoint(Point self, Point target)
    {
        if (target == null || self == null)return null;

        if (target.x > self.x && target.y == self.y)return UtilityTools.Directions.right;
        else if (target.x < self.x && target.y == self.y)return UtilityTools.Directions.left;
        else if (target.x > self.x && target.y < self.y)return UtilityTools.Directions.upRight;
        else if (target.x > self.x && target.y > self.y)return UtilityTools.Directions.downRight;
        else if (target.y > self.y && target.x == self.x)return UtilityTools.Directions.down;
        else if (target.y < self.y && target.x == self.x)return UtilityTools.Directions.up;
        else if (target.y > self.y && target.x < self.x)return UtilityTools.Directions.downLeft;
        if (target.y < self.y && target.x < self.x)return UtilityTools.Directions.upLeft;

        return null;
    }
*/
    private static bool HasGenericBase(System.Type myType, System.Type t)
    {
        Debug.Assert(t.IsGenericTypeDefinition);
        while (myType != typeof(object))
        {
            if (myType.IsGenericType && myType.GetGenericTypeDefinition() == t)
            {
                return true;
            }
            myType = myType.BaseType;
        }
        return false;
    }

    private static int getRealConnectedJoysticksCount()
    {
        int count = 0;

        foreach (string t in Input.GetJoystickNames())
        {
            if (t != "")count++;
        }

        return count;
    }

    // returns true if the direction is orthogonal
    public static bool IsOrthogonalDirection(Vector3 inputNormalized)
    {
        if(inputNormalized == Vector3.zero) return false;
        
        return  (inputNormalized.x == 0 && inputNormalized.y == 0) ||
                (inputNormalized.x == 0 && inputNormalized.z == 0) ||
                (inputNormalized.y == 0 && inputNormalized.z == 0);
    }

    public static Vector3 GetPenaltyRotatedDirection(Vector3 inputDirection, Vector3 axisToApply)
    {
        Vector3 rotationMod = Quaternion.Euler(0, -90f, 0) * inputDirection;
        return Vector3.Scale(rotationMod, axisToApply);
        
    }

    public static void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}