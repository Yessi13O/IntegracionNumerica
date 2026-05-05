namespace LinkedList
{
    public class DoublyLinkedList<T> where T : IComparable<T>
    {
        private Node<T>? _head;
        private Node<T>? _tail;

        public DoublyLinkedList()
        {
            _head = null;
            _tail = null;
        }

        // Inserts a new element keeping ascending order
        public void Add(T data)
        {
            var newNode = new Node<T>(data);

            if (_head == null)
            {
                _head = newNode;
                _tail = newNode;
                return;
            }

            var current = _head;
            while (current != null && current.Data!.CompareTo(data) <= 0)
                current = current.Next;

            if (current == null) // Insert at the end
            {
                newNode.Previous = _tail;
                _tail!.Next = newNode;
                _tail = newNode;
            }
            else if (current.Previous == null) // Insert at the beginning
            {
                newNode.Next = _head;
                _head.Previous = newNode;
                _head = newNode;
            }
            else // Insert in the middle
            {
                newNode.Previous = current.Previous;
                newNode.Next = current;
                current.Previous!.Next = newNode;
                current.Previous = newNode;
            }
        }

        // Traverses from head to tail
        public override string ToString()
        {
            var current = _head;
            var result = string.Empty;
            while (current != null)
            {
                result += $"{current.Data} <-> ";
                current = current.Next;
            }
            result += "null";
            return result;
        }

        // Traverses from tail to head
        public string ToStringReverse()
        {
            var current = _tail;
            var result = string.Empty;
            while (current != null)
            {
                result += $"{current.Data} <-> ";
                current = current.Previous;
            }
            result += "null";
            return result;
        }

        // Reverses the current order of the list
        public void SortDescending()
        {
            if (_head == null || _head == _tail)
                return;

            var current = _head;
            Node<T>? temp = null;

            while (current != null)
            {
                temp = current.Previous;
                current.Previous = current.Next;
                current.Next = temp;
                current = current.Previous;
            }

            temp = _head;
            _head = _tail;
            _tail = temp;
        }

        // Returns the element(s) with the highest frequency
        public List<T> GetModes()
        {
            var modes = new List<T>();

            if (_head == null)
                return modes;

            var frequencies = GetFrequencies();

            var maxFrequency = 0;
            foreach (var freq in frequencies.Values)
                if (freq > maxFrequency)
                    maxFrequency = freq;

            if (maxFrequency == 1)
                return modes;

            foreach (var pair in frequencies)
                if (pair.Value == maxFrequency)
                    modes.Add(pair.Key);

            return modes;
        }

        // Returns occurrence count per element
        public Dictionary<T, int> GetFrequencies()
        {
            var frequencies = new Dictionary<T, int>();
            var current = _head;

            while (current != null)
            {
                if (frequencies.ContainsKey(current.Data!))
                    frequencies[current.Data!]++;
                else
                    frequencies[current.Data!] = 1;

                current = current.Next;
            }

            return frequencies;
        }

        // Returns true if at least one occurrence is found
        public bool Exists(T data)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data!.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        // Removes the first occurrence found
        public bool DeleteOne(T data)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data!.Equals(data))
                {
                    DetachNode(current);
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        // Removes every occurrence of the given value
        public int DeleteAll(T data)
        {
            var count = 0;
            var current = _head;

            while (current != null)
            {
                var next = current.Next;
                if (current.Data!.Equals(data))
                {
                    DetachNode(current);
                    count++;
                }
                current = next;
            }

            return count;
        }

        // Unlinks a node from the list
        private void DetachNode(Node<T> node)
        {
            if (node == _head)
            {
                _head = _head.Next;
                if (_head != null)
                    _head.Previous = null;
                else
                    _tail = null;
            }
            else if (node == _tail)
            {
                _tail = _tail.Previous;
                _tail!.Next = null;
            }
            else
            {
                node.Previous!.Next = node.Next;
                node.Next!.Previous = node.Previous;
            }
        }
    }
}
