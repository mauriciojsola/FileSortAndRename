using System;
using System.Web.Mvc;

namespace FileSortAndRename.Helpers
{
    public enum FlashKey
    {
        Notice,
        Error,
        Warning
    }

    public class Flash
    {
        private readonly TempDataDictionary _tempData;

        public Flash(TempDataDictionary tempData)
        {
            _tempData = tempData;
        }

        public string this[FlashKey key]
        {
            set { _tempData[string.Format("Flash_{0}", key)] = value; }
            get { return _tempData[string.Format("Flash_{0}", key)].ToString(); }
        }

        public void Reset()
        {
            foreach (FlashKey key in Enum.GetValues(typeof(FlashKey)))
            {
                this[key] = null;
            }
        }
    }
}