﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubConsole
{
    public class ArgumentStack : IEnumerable<ArgumentStack.Argument>
    {
        public class Argument
        {
            private string key;
            private string[] values;

            public Argument(string key, IEnumerable<string> values)
            {
                this.key = key.ToLower();
                this.values = values.ToArray();
            }

            public string Key
            {
                get { return key; }
            }

            public int Count
            {
                get { return values.Length; }
            }

            public string this[int index]
            {
                get { return values[index]; }
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", key, string.Join(", ", values));
            }
        }

        private List<Argument> arguments;

        public ArgumentStack(string[] args)
        {
            this.arguments = new List<Argument>();
            string key = null;
            List<string> values = new List<string>();

            foreach (var a in args)
            {
                if (key == null && !a.StartsWith("-"))
                    this.arguments.Add(new Argument(a, new string[0]));

                else if (a.StartsWith("-"))
                {
                    if (key != null)
                    {
                        this.arguments.Add(new Argument(key, values));
                        key = null;
                        values.Clear();
                    }
                    key = a;
                }
                else
                    values.Add(a);
            }

            if (key != null)
                this.arguments.Add(new Argument(key, values));
        }

        public Argument Pop()
        {
            Argument a = arguments[0];
            arguments.RemoveAt(0);
            return a;
        }

        public Argument this[string key]
        {
            get { key = key.ToLower(); return arguments.FirstOrDefault(x => x.Key == key); }
        }
        public Argument this[int index]
        {
            get { return arguments[index]; }
        }

        public bool Contains(string key)
        {
            key = key.ToLower(); 
            return arguments.Any(x => x.Key == key);
        }

        public int Count
        {
            get { return arguments.Count; }
        }

        public IEnumerator<ArgumentStack.Argument> GetEnumerator()
        {
            foreach (var a in arguments)
                yield return a;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var a in arguments)
                yield return a;
        }
    }
}
