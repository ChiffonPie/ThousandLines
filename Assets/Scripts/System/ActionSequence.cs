using System;
using System.Collections.Generic;

namespace ThousandLines_Data
{
    public class ActionSequence
    {
        private int m_Index;
        private List<Action> m_Actions = new List<Action>();
        private Action m_Completed;

        public void Clear()
        {
            this.m_Actions.Clear();
        }

        public int Count
        {
            get
            {
                return m_Actions.Count;
            }
        }

        public void Add(Action action)
        {
            this.m_Actions.Add(action);
        }

        public void Start(Action onCompleted = null)
        {
            this.m_Completed = onCompleted;

            this.m_Index = -1;
            this.Next();
        }

        public void Next()
        {
            this.m_Index++;

            var content = this.m_Actions.Find(this.m_Index);
            if (content == null)
            {
                if (this.m_Completed != null)
                    this.m_Completed();
            }
            else
            {
                content.Invoke();
            }
        }
    }
}