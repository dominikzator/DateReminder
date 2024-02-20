using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DateReminder
{
    public class SingletonWindow<T> where T : Window, new()
    {
        private static T _windowInstance;

        public T WindowInstance
        {
            get
            {
                if(_windowInstance == null)
                {
                    _windowInstance = new T();
                    _windowInstance.Closed += (object? sender, EventArgs e) => {
                        _windowInstance = null;
                    };
                }
                return _windowInstance;
            }
        }

        private static SingletonWindow<T> _instance;

        public static SingletonWindow<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SingletonWindow<T>();
                }
                return _instance;
            }
        }

    }
}
