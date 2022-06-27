using System.Collections.Generic;

namespace Source
{
    public class AppData
    {
        private List<IAppData> _data;

        public AppData()
        {
            _data = new List<IAppData>();
        }

        public void Push(IAppData data)
        {
            _data.Add(data);
        }

        public bool TryPop<TData>(out TData data) where TData : IAppData
        {
            data = default;

            var dataIndex = _data.FindIndex(d => d is TData);

            if (dataIndex >= 0)
            {
                data = (TData) _data[dataIndex];
                _data.RemoveAt(dataIndex);
                return true;
            }
            
            return false;
        }
    }
}