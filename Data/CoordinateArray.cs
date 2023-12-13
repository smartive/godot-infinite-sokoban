global using Coordinates = (int X, int Y);
global using Dimensions = (int Width, int Height);

using System.Collections;
using System.Text;

namespace InfiniteSokoban.Data;

public class CoordinateArray<T> : IEnumerable<T>
{
    public CoordinateArray(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new T[width * height];
    }

    public CoordinateArray(Dimensions dimensions)
        : this(dimensions.Width, dimensions.Height)
    {
    }

    public CoordinateArray(int width, int height, Func<Coordinates, T> initializer)
        : this(width, height)
    {
        foreach (var (x, y) in IndexIterator())
        {
            this[x, y] = initializer((x, y));
        }
    }

    public CoordinateArray(int width, int height, T initialValue)
        : this(width, height, _ => initialValue)
    {
    }

    public CoordinateArray(int width, int height, T[] data)
    {
        Width = width;
        Height = height;
        Data = data;
    }

    public CoordinateArray(T[,] data)
        : this(data.GetLength(1), data.GetLength(0)) =>
        Data = data.Cast<T>().ToArray();

    public int Width { get; }

    public int Height { get; }

    public Dimensions Dimensions => (Width, Height);

    private T[] Data { get; }

    public T this[int x, int y]
    {
        get => Data[x + (y * Width)];
        set => Data[x + (y * Width)] = value;
    }

    public T this[Coordinates index]
    {
        get => this[index.X, index.Y];
        set => this[index.X, index.Y] = value;
    }

    public IEnumerable<(int X, int Y, T Data)> IndexedIterator()
    {
        for (var i = 0; i < Data.Length; i++)
        {
            var x = i % Width;
            var y = i / Width;
            yield return (x, y, Data[i]);
        }
    }

    public IEnumerable<Coordinates> IndexIterator()
    {
        for (var i = 0; i < Data.Length; i++)
        {
            var x = i % Width;
            var y = i / Width;
            yield return (x, y);
        }
    }

    public CoordinateArray<T> GetWindow(Coordinates from, Coordinates to)
    {
        var window = new CoordinateArray<T>(to.X - from.X + 1, to.Y - from.Y + 1);
        for (var x = from.X; x <= to.X; x++)
        {
            for (var y = from.Y; y <= to.Y; y++)
            {
                window[x - from.X, y - from.Y] = this[x, y];
            }
        }

        return window;
    }

    public IEnumerable<CoordinateArray<T>> Windows(int windowWidth, int windowHeight)
    {
        // Adjust the window size to ensure there is at least one window
        windowWidth = Math.Min(windowWidth, Width);
        windowHeight = Math.Min(windowHeight, Height);

        for (var x = 0; x < Width - windowWidth; x++)
        {
            for (var y = 0; y < Height - windowHeight; y++)
            {
                yield return GetWindow((x, y), (x + windowWidth - 1, y + windowHeight - 1));
            }
        }
    }

    public void ApplyWindow(CoordinateArray<T> window, Coordinates at)
    {
        for (var x = 0; x < window.Width; x++)
        {
            for (var y = 0; y < window.Height; y++)
            {
                this[x + at.X, y + at.Y] = window[x, y];
            }
        }
    }

    public bool IsInBounds(Coordinates index) =>
        index.X >= 0 && index.X < Width && index.Y >= 0 && index.Y < Height;

    public T[] GetRow(int index)
    {
        var row = new T[Width];
        for (var i = 0; i < Width; i++)
        {
            row[i] = this[i, index];
        }

        return row;
    }

    public T[] GetColumn(int index)
    {
        var column = new T[Height];
        for (var i = 0; i < Height; i++)
        {
            column[i] = this[index, i];
        }

        return column;
    }

    public IEnumerable<(int RowIndex, T[] Row)> RowIterator()
    {
        for (var i = 0; i < Height; i++)
        {
            yield return (i, GetRow(i));
        }
    }

    public IEnumerable<(int ColumnIndex, T[] Column)> ColumnIterator()
    {
        for (var i = 0; i < Width; i++)
        {
            yield return (i, GetColumn(i));
        }
    }

    public void Swap(Coordinates a, Coordinates b) =>
        (this[a], this[b]) = (this[b], this[a]);

    public CoordinateArray<T> Clone() => new(Width, Height, (T[])Data.Clone());

    public void Clear() => Array.Clear(Data, 0, Data.Length);

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Data).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            builder.Append("[");
            for (var x = 0; x < Width; x++)
            {
                builder.Append(this[x, y]);
                if (x < Width - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append("]\n");
        }

        return builder.ToString();
    }
}
