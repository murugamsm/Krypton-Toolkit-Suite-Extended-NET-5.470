﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Tooling.Enumerations;
using Tooling.Interfaces;

namespace Tooling.Classes.Colours
{
    /// <summary>
    /// Represents a collection of colors
    /// </summary>
    /// <remarks>
    /// 	<para>ColourCollection allows duplicate elements.</para>
    /// 	<para>Elements in this collection can be accessed using an integer index. Indexes in this collection are zero-based.</para>
    /// </remarks>
    public class ColourCollection : Collection<Color>, ICloneable, IEquatable<ColourCollection>
    {
        #region Instance Fields

        private readonly object _lock = new object();

        private IDictionary<int, int> _indexedLookup;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourCollection"/> class.
        /// </summary>
        public ColourCollection()
        {
#if USENAMEHACK
      this.SwatchNames = new List<string>();
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourCollection"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public ColourCollection(IEnumerable<Color> collection)
          : this()
        {
            this.AddRange(collection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourCollection"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public ColourCollection(ColourCollection collection)
          : this()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                this.Add(collection[i]);
#if USENAMEHACK
        this.SetName(i, collection.GetName(i));
#endif
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourCollection"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public ColourCollection(IEnumerable<int> collection)
          : this()
        {
            this.AddRange(collection.Select(Color.FromArgb));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourCollection"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public ColourCollection(System.Drawing.Imaging.ColorPalette collection)
          : this()
        {
            this.AddRange(collection.Entries);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when elements in the collection are added, removed or modified.
        /// </summary>
        public event EventHandler<ColourCollectionEventArgs> CollectionChanged;

        public event EventHandler<ColourCollectionEventArgs> ItemInserted;

        public event EventHandler<ColourCollectionEventArgs> ItemRemoved;

        public event EventHandler<ColourCollectionEventArgs> ItemReplaced;

        public event EventHandler<ColourCollectionEventArgs> ItemsCleared;

        #endregion

        #region Class Members

        /// <summary>
        /// Creates a new instance of the <see cref="ColourCollection" /> class that contains elements loaded from the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file to load.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the <c>fileName</c> argument is not specified.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file specified by <c>fileName</c> cannot be found.</exception>
        /// <exception cref="System.ArgumentException">Thrown if no <see cref="IPaletteSerialiser"/> is available for the file specified by <c>fileName</c>.</exception>
        public static ColourCollection LoadPalette(string fileName)
        {
            IPaletteSerialiser serialiser;

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format("Cannot find file '{0}'", fileName), fileName);
            }

            serialiser = PaletteSerialiser.GetSerialiser(fileName);
            if (serialiser == null)
            {
                throw new ArgumentException(string.Format("Cannot find a palette serialiser for '{0}'", fileName), nameof(fileName));
            }

            using (FileStream file = File.OpenRead(fileName))
            {
                return serialiser.Deserialise(file);
            }
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        protected override void ClearItems()
        {
            ColourCollectionEventArgs e;

            base.ClearItems();

            _indexedLookup = null;

#if USENAMEHACK
      this.SwatchNames.Clear();
#endif

            e = new ColourCollectionEventArgs(-1, Color.Empty);
            this.OnItemInserted(e);
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, Color item)
        {
            ColourCollectionEventArgs e;
            int key;

            base.InsertItem(index, item);

#if USENAMEHACK
      this.SwatchNames.Insert(index, string.Empty);
#endif
            key = item.ToArgb();

            if (_indexedLookup != null && index == this.Count - 1 && !_indexedLookup.ContainsKey(key))
            {
                lock (_lock)
                {
                    if (!_indexedLookup.ContainsKey(key))
                    {
                        _indexedLookup.Add(key, index);
                    }
                }
            }
            else
            {
                _indexedLookup = null;
            }

            e = new ColourCollectionEventArgs(index, item);
            this.OnItemInserted(e);
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            Color item;
            ColourCollectionEventArgs e;
            int key;

#if USENAMEHACK
      this.SwatchNames.RemoveAt(index);
#endif

            item = this[index];
            key = item.ToArgb();

            if (_indexedLookup != null && _indexedLookup.ContainsKey(key))
            {
                lock (_lock)
                {
                    _indexedLookup.Remove(key);
                }
            }

            base.RemoveItem(index);

            e = new ColourCollectionEventArgs(index, item);
            this.OnItemRemoved(e);
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, Color item)
        {
            Color oldItem;

            oldItem = this[index];

            if (oldItem != item)
            {
                ColourCollectionEventArgs e;

                if (_indexedLookup != null)
                {
                    int key;
                    int oldKey;

                    key = item.ToArgb();
                    oldKey = oldItem.ToArgb();

                    lock (_lock)
                    {
                        if (_indexedLookup.ContainsKey(oldKey))
                        {
                            _indexedLookup.Remove(oldKey);
                        }
                        if (!_indexedLookup.ContainsKey(key))
                        {
                            _indexedLookup.Add(key, index);
                        }
                    }
                }

                base.SetItem(index, item);

                e = new ColourCollectionEventArgs(index, item);
                this.OnItemReplaced(e);
                this.OnCollectionChanged(e);
            }
        }

        #endregion

        #region Public Members

        /// <summary>Adds the elements of the specified collection to the end of the <see cref="ColourCollection"/>.</summary>
        /// <param name="colors">The collection whose elements should be added to the end of the <see cref="ColourCollection"/>.</param>
        public void AddRange(IEnumerable<Color> colors)
        {
            foreach (Color color in colors)
            {
                this.Add(color);
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public virtual ColourCollection Clone()
        {
            return new ColourCollection(this);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="ColourCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="Color"/> to locate in the <see cref="ColourCollection"/>.</param>
        /// <returns>The zero-based index of the first occurrence of <c>item</c> within the entire <see cref="ColourCollection"/>, if found; otherwise, –1.</returns>
        public int Find(Color item)
        {
            return this.Find(item.ToArgb());
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="ColourCollection" />.
        /// </summary>
        /// <param name="item">The <see cref="Color"/> to locate in the <see cref="ColourCollection" />.</param>
        /// <param name="ignoreAlphaChannel">If set to <c>true</c> only the red, green and blue channels of items in the <see cref="ColourCollection"/> will be compared.</param>
        /// <returns>The zero-based index of the first occurrence of <c>item</c> within the entire <see cref="ColourCollection" />, if found; otherwise, –1.</returns>
        public int Find(Color item, bool ignoreAlphaChannel)
        {
            int result;

            if (!ignoreAlphaChannel)
            {
                result = this.Find(item);
            }
            else
            {
                // TODO: This is much much slower than the lookup based find

                result = -1;

                for (int i = 0; i < this.Count; i++)
                {
                    Color original;

                    original = this[i];
                    if (original.R == item.R && original.G == item.G && original.B == item.B)
                    {
                        result = i;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="ColourCollection"/>.
        /// </summary>
        /// <param name="item">The ARGB color to locate in the <see cref="ColourCollection"/>.</param>
        /// <returns>The zero-based index of the first occurrence of <c>item</c> within the entire <see cref="ColourCollection"/>, if found; otherwise, –1.</returns>
        public int Find(int item)
        {
            int result;

            if (_indexedLookup == null)
            {
                this.BuildIndexedLookup();
            }

            if (_indexedLookup == null || !_indexedLookup.TryGetValue(item, out result))
            {
                result = -1;
            }

            return result;
        }

        /// <summary>
        /// Populates this <see cref="ColourCollection"/> with items loaded from the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file to load.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the <c>fileName</c> argument is not specified.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file specified by <c>fileName</c> cannot be found.</exception>
        /// <exception cref="System.ArgumentException">Thrown if no <see cref="IPaletteSerializer"/> is available for the file specified by <c>fileName</c>.</exception>
        public void Load(string fileName)
        {
            ColourCollection palette;

            palette = LoadPalette(fileName);

            this.Clear();
            this.AddRange(palette);
        }

        /// <summary>
        /// Saves the contents of this <see cref="ColourCollection"/> into the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file to save.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the <c>fileName</c> argument is not specified.</exception>
        /// <exception cref="System.ArgumentException">Thrown if no <see cref="IPaletteSerializer"/> is available for the file specified by <c>fileName</c>.</exception>
        public void Save<T>(string fileName) where T : IPaletteSerialiser, new()
        {
            IPaletteSerialiser serializer;

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            serializer = Activator.CreateInstance<T>();

            using (FileStream file = File.OpenWrite(fileName))
            {
                serializer.Serialise(file, this);
            }
        }

        /// <summary>
        /// Sorts the elements in the entire %ColourCollection% using the specified order.
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <exception cref="System.ArgumentException">Thrown when an invalid sort order is specified</exception>
        public void Sort(ColourCollectionSortOrder sortOrder)
        {
            if (this.Count > 0)
            {
                Comparison<Color> sortDelegate;

                // HACK: This is a bit nasty

                switch (sortOrder)
                {
                    case ColourCollectionSortOrder.BRIGHTNESS:
                        sortDelegate = ColourComparer.Brightness;
                        break;
                    case ColourCollectionSortOrder.HUE:
                        sortDelegate = ColourComparer.Hue;
                        break;
                    case ColourCollectionSortOrder.VALUE:
                        sortDelegate = ColourComparer.Value;
                        break;
                    default:
                        throw new ArgumentException("Invalid sort order", nameof(sortOrder));
                }

#if USENAMEHACK
        this.SortWithNames(sortDelegate);
#else
                List<Color> orderedItems;

                orderedItems = new List<Color>(this);
                orderedItems.Sort(sortDelegate);
                this.ClearItems();
                this.AddRange(orderedItems);
#endif
            }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Raises the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(ColourCollectionEventArgs e)
        {
            EventHandler<ColourCollectionEventArgs> handler;

            handler = this.CollectionChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemInserted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ColourCollectionEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemInserted(ColourCollectionEventArgs e)
        {
            EventHandler<ColourCollectionEventArgs> handler;

            handler = this.ItemInserted;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemRemoved" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ColourCollectionEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemRemoved(ColourCollectionEventArgs e)
        {
            EventHandler<ColourCollectionEventArgs> handler;

            handler = this.ItemRemoved;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemReplaced" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ColourCollectionEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemReplaced(ColourCollectionEventArgs e)
        {
            EventHandler<ColourCollectionEventArgs> handler;

            handler = this.ItemReplaced;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemsCleared" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemsCleared(ColourCollectionEventArgs e)
        {
            EventHandler<ColourCollectionEventArgs> handler;

            handler = this.ItemsCleared;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Builds an indexed lookup for quick searching.
        /// </summary>
        private void BuildIndexedLookup()
        {
            lock (_lock)
            {
                _indexedLookup = new Dictionary<int, int>();

                for (int i = 0; i < this.Count; i++)
                {
                    Color colour;
                    int key;

                    colour = this[i];
                    key = colour.ToArgb();

                    if (!_indexedLookup.ContainsKey(key))
                    {
                        _indexedLookup.Add(key, i);
                    }
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

#if USENAMEHACK
    protected IList<string> SwatchNames { get; set; }

    public string GetName(int index)
    {
      return this.SwatchNames[index];
    }

    public void SetName(int index, string name)
    {
      if (!string.Equals(this.SwatchNames[index], name))
      {
        this.SwatchNames[index] = name;
        this.OnCollectionChanged(new ColourCollectionEventArgs(index, this[index]));
      }
    }

    private void SortWithNames(Comparison<Color> comparer)
    {
      int count;
      Color[] colors;
      string[] names;

      count = this.Count;
      colors = new Color[count];
      names = new string[count];

      this.CopyTo(colors, 0);
      this.SwatchNames.CopyTo(names, 0);

      this.Quicksort(colors, names, 0, count - 1, comparer);

      this.ClearItems();

      for (int i = 0; i < count; i++)
      {
        this.Add(colors[i]);
        this.SetName(i, names[i]);
      }
    }

    private void Quicksort(Color[] colors, string[] names, int left, int right, Comparison<Color> comparer)
    {
      int i = left, j = right;
      Color pivot = colors[(left + right) / 2];

      // derived from http://snipd.net/quicksort-in-c

      while (i <= j)
      {
        while (comparer(colors[i], pivot) < 0)
        {
          i++;
        }

        while (comparer(colors[j], pivot) > 0)
        {
          j--;
        }

        if (i <= j)
        {
          // Swap
          Color tmp = colors[i];
          colors[i] = colors[j];
          colors[j] = tmp;

          string z;

          z = names[i];
          names[i] = names[j];
          names[j] = z;

          i++;
          j--;
        }
      }

      // Recursive calls
      if (left < j)
      {
        Quicksort(colors, names, left, j, comparer);
      }

      if (i < right)
      {
        Quicksort(colors, names, i, right, comparer);
      }
    }
#endif

        /// <summary>
        /// Compares two <see cref="ColourCollection"/> objects. The result specifies whether the values of the two <see cref="ColourCollection"/> objects are equal.
        /// </summary>
        /// <param name="left">A <see cref="ColourCollection"/> to compare.</param>
        /// <param name="right">A <see cref="ColourCollection"/> to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ColourCollection left, ColourCollection right)
        {
            return ReferenceEquals(left, right) || !((object)left == null || (object)right == null) && left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="ColourCollection"/> objects. The result specifies whether the values of the two <see cref="ColourCollection"/> objects are unequal.
        /// </summary>
        /// <param name="left">A <see cref="ColourCollection"/> to compare.</param>
        /// <param name="right">A <see cref="ColourCollection"/> to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="left"/> and <paramref name="right"/> differ; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ColourCollection left, ColourCollection right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="ColourCollection"/> contains the same coordinates as the specified <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to test.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="ColourCollection"/> and has the same values as this <see cref="ColourCollection"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ColourCollection && this.Equals((ColourCollection)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ColourCollection other)
        {
            bool result;

            result = other != null && other.Count == this.Count;
            if (result)
            {
                // check colors - by value though, as Color.Cornflowerblue != Color.FromArgb(255, 100, 149, 237)
                for (int i = 0; i < this.Count; i++)
                {
                    Color expected;
                    Color actual;

                    expected = other[i];
                    actual = this[i];

                    if (expected.ToArgb() != actual.ToArgb())
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            // http://stackoverflow.com/a/10567511/148962

            return this.Aggregate(0, (current, value) => current ^ value.GetHashCode());
        }
    }
}