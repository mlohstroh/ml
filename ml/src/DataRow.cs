﻿using System;
using System.IO;
using System.Collections.Generic;

namespace mlAssignment1
{
    /// <summary>
    /// A data row represents a row of data that will be evaluated. It can be thought of as 
    /// a row in a database or a flat document. It will just be a KVP store with object casting safely
    /// </summary>
    public class DataRow
    {
        private Dictionary<string, object> _store = new Dictionary<string, object>();

        public List<string> Attributes
        {
            get { return new List<string>(_store.Keys); }
        }

        public DataRow(Dictionary<string, object> store)
        {
            _store = store;
        }

        public void AddAttribute(string key, object val)
        {
            _store.Add(key, val);
        }

        public object RetrieveValue(string key)
        {
            object val = null;
            _store.TryGetValue(key, out val);
            return val;
        }

        public bool RetrieveValueAsBool(string key)
        {
            try
            {
                return Convert.ToBoolean(RetrieveValue(key));
            }
            catch(Exception)
            {
                return false;
            }
        }

        public int RetrieveValueAsInt(string key)
        {
            try
            {
                return Convert.ToInt32(RetrieveValue(key));
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public double RetrieveValueAsDouble(string key)
        {
            try
            {
                return Convert.ToDouble(RetrieveValue(key));
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
        public string RetrieveValueAsString(string key)
        {
            object val = RetrieveValue(key);
            if (val == null)
                return "";
            return val.ToString();
        }

        public static List<DataRow> ReadFile(string file)
        {
            List<DataRow> read = new List<DataRow>();

            using (StreamReader reader = new StreamReader(file))
            {
                // read the header

                string[] attrs = reader.ReadLine().Split(new char[] { '\t' });

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    string[] data = line.Split(new char[] { '\t' });
                    for (int i = 0; i < attrs.Length; i++)
                    {
                        dict[attrs[i]] = int.Parse(data[i]);
                    }

                    read.Add(new DataRow(dict));

                }
            }

            return read;
        }
    }
}
